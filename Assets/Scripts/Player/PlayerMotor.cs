using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    [Header("References")]
    public Transform cameraPivot;

    [Header("Movement")]
    public float maxGroundSpeed = 9f;
    public float maxAirSpeed = 9f;
    public float groundAcceleration = 60f;
    public float airAcceleration = 25f;
    public float groundFriction = 14f;

    [Header("Jump")]
    public float jumpHeight = 1.2f;
    public float gravity = 22f;
    public float coyoteTime = 0.08f;
    public float jumpBuffer = 0.08f;

    [Header("Crouch")]
    public float standingHeight = 1.8f;
    public float crouchHeight = 1.1f;
    public float crouchSpeedMultiplier = 0.75f;

    [Header("Slide")]
    public float slideDuration = 0.40f;

    // burst
    public float slideBurstSpeed = 50f;
    public float slideMinStartSpeed = 4.5f;

    // decay
    public float slideDecayHalfLife = 0.10f;
    public float slideMinEndSpeed = 2.0f;

    // steering
    public float slideSteerAccel = 10f;

    public float slideFriction = 2.5f;
    public float slideCooldown = 0.12f;

    [Header("Ceiling Check")]
    public LayerMask obstructionMask = ~0;

    [Header("FX")]
    public FovKick fovKick;

    CharacterController cc;
    PlayerControls controls;

    public CameraMovement cameraMovement;

    Vector3 velocity;
    Vector3 planarVelocity;

    float coyoteTimer;
    float jumpBufferTimer;

    enum Stance { Stand, Crouch, Slide }
    Stance stance = Stance.Stand;

    float slideTimer;
    float slideCooldownTimer;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        controls = new PlayerControls();

        if (standingHeight <= 0.01f) standingHeight = cc.height;
        if (cc.height > 0.01f && Mathf.Abs(cc.height - standingHeight) < 0.001f)
            standingHeight = cc.height;

        ApplyControllerHeightKeepBottom(standingHeight);
        stance = Stance.Stand;
    }

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    void Update()
    {
        slideCooldownTimer -= Time.deltaTime;

        Vector2 moveInput = Vector2.ClampMagnitude(controls.Player.Move.ReadValue<Vector2>(), 1f);

        if (controls.Player.Jump.WasPressedThisFrame())
            jumpBufferTimer = jumpBuffer;
        else
            jumpBufferTimer -= Time.deltaTime;

        bool crouchHeld = controls.Player.Crouch.IsPressed();
        bool crouchPressed = controls.Player.Crouch.WasPressedThisFrame();

        bool grounded = cc.isGrounded;
        if (grounded)
        {
            coyoteTimer = coyoteTime;
            if (velocity.y < 0f) velocity.y = -2f;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        Vector3 wishDir = GetWishDirection(moveInput);

        if (stance != Stance.Slide)
        {
            if (crouchPressed && grounded && planarVelocity.magnitude >= slideMinStartSpeed && slideCooldownTimer <= 0f)
            {
                StartSlide(wishDir);
            }
            else
            {
                if (crouchHeld) SetStance(Stance.Crouch);
                else TryStandUp();
            }
        }

        if (stance == Stance.Slide)
        {
            if (slideSteerAccel > 0f)
            {
                float steerTargetSpeed = Mathf.Max(planarVelocity.magnitude, maxGroundSpeed);
                planarVelocity = Accelerate(planarVelocity, wishDir, steerTargetSpeed, slideSteerAccel, Time.deltaTime);
            }

            float halfLife = Mathf.Max(0.001f, slideDecayHalfLife);
            float decay = Mathf.Pow(0.5f, Time.deltaTime / halfLife);
            planarVelocity *= decay;

            slideTimer -= Time.deltaTime;

            if (fovKick)
            {
                fovKick.UpdateSlideSpeed(planarVelocity.magnitude);
            }

            if (slideTimer <= 0f || planarVelocity.magnitude <= slideMinEndSpeed || !grounded)
            {
                EndSlide(controls.Player.Crouch.IsPressed());
            }
        }
        else
        {
            float speedMult = (stance == Stance.Crouch) ? crouchSpeedMultiplier : 1f;

            if (grounded)
            {
                planarVelocity = ApplyFriction(planarVelocity, groundFriction, Time.deltaTime);
                planarVelocity = Accelerate(planarVelocity, wishDir, maxGroundSpeed * speedMult, groundAcceleration, Time.deltaTime);
            }
            else
            {
                planarVelocity = Accelerate(planarVelocity, wishDir, maxAirSpeed * speedMult, airAcceleration, Time.deltaTime);
            }
        }

        if (jumpBufferTimer > 0f && coyoteTimer > 0f && stance != Stance.Slide)
        {
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;

            if (stance == Stance.Crouch) TryStandUp(force: true);

            velocity.y = Mathf.Sqrt(2f * gravity * jumpHeight);
        }

        velocity.y -= gravity * Time.deltaTime;

        Vector3 move = planarVelocity + Vector3.up * velocity.y;
        cc.Move(move * Time.deltaTime);

        if (fovKick)
        {
            fovKick.SetSpeed(planarVelocity.magnitude);
        }

        if ((cc.collisionFlags & CollisionFlags.Above) != 0 && velocity.y > 0f)
            velocity.y = 0f;
    }

    // -------------------------
    // Slide
    // -------------------------

    void StartSlide(Vector3 wishDir)
    {
        SetStance(Stance.Slide);

        slideTimer = slideDuration;
        slideCooldownTimer = slideCooldown;

        // direction prefer current velocity, else wishDir, else forward
        Vector3 dir =
            planarVelocity.sqrMagnitude > 0.01f ? planarVelocity.normalized :
            wishDir.sqrMagnitude > 0.01f ? wishDir :
            (cameraPivot ? Vector3.ProjectOnPlane(cameraPivot.forward, Vector3.up).normalized : transform.forward);

        float currentSpeed = planarVelocity.magnitude;
        float startSpeed = Mathf.Max(currentSpeed, slideMinStartSpeed);

        // big snap up to burst speed 
        planarVelocity = dir * Mathf.Max(startSpeed, slideBurstSpeed);
        if (fovKick)
        {
            fovKick.BeginSlide(planarVelocity.magnitude);
        }
    }

    void EndSlide(bool crouchHeldAfter)
    {
        if (fovKick)
        {
            fovKick.EndSlide();
        }

        if (crouchHeldAfter) 
        {
            SetStance(Stance.Crouch);
        }
        else 
        {
            TryStandUp(force: true);
        }
    }

    // -------------------------
    // Stances / Controller sizing
    // -------------------------

    void SetStance(Stance newStance)
    {
        if (stance == newStance) return;
        stance = newStance;
        cameraMovement.isCrouching = (stance != Stance.Stand);

        if (stance == Stance.Stand)
        {
            ApplyControllerHeightKeepBottom(standingHeight);
        }
        else
        {
            ApplyControllerHeightKeepBottom(crouchHeight);
        }
    }

    void TryStandUp(bool force = false)
    {
        if (stance == Stance.Stand) return;

        if (!CanStandUp())
        {
            if (stance != Stance.Slide) SetStance(Stance.Crouch);
            return;
        }

        SetStance(Stance.Stand);
    }

    bool CanStandUp()
    {
        float radius = cc.radius * 0.98f;

        Vector3 worldCenter = transform.position + cc.center;

        float currentHalf = cc.height * 0.5f;
        Vector3 feet = worldCenter + Vector3.down * (currentHalf - radius);

        float targetHalf = standingHeight * 0.5f;
        Vector3 head = feet + Vector3.up * ((targetHalf - radius) * 2f);

        Collider[] hits = new Collider[8];
        int count = Physics.OverlapCapsuleNonAlloc(
            feet,
            head,
            radius,
            hits,
            obstructionMask,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < count; i++)
        {
            var col = hits[i];
            if (!col) continue;

            if (col == cc) continue;

            if (col.transform.IsChildOf(transform)) continue;

            return false;
        }

        return true;
    }

    void ApplyControllerHeightKeepBottom(float targetHeight)
    {
        float bottomY = transform.position.y + cc.center.y - (cc.height * 0.5f);

        cc.height = targetHeight;

        float newCenterY = bottomY - transform.position.y + (cc.height * 0.5f);

        cc.center = new Vector3(cc.center.x, newCenterY, cc.center.z);
    }

    // -------------------------
    // Movement helpers
    // -------------------------

    Vector3 GetWishDirection(Vector2 moveInput)
    {
        Vector3 forward = cameraPivot ? cameraPivot.forward : transform.forward;
        Vector3 right = cameraPivot ? cameraPivot.right : transform.right;

        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();

        Vector3 wish = forward * moveInput.y + right * moveInput.x;
        if (wish.sqrMagnitude > 1e-6f) wish.Normalize();
        return wish;
    }

    static Vector3 ApplyFriction(Vector3 v, float friction, float dt)
    {
        float speed = v.magnitude;
        if (speed < 0.001f) return Vector3.zero;

        float drop = speed * friction * dt;
        float newSpeed = Mathf.Max(speed - drop, 0f);
        return v * (newSpeed / speed);
    }

    static Vector3 Accelerate(Vector3 current, Vector3 wishDir, float wishSpeed, float accel, float dt)
    {
        if (wishDir.sqrMagnitude < 1e-6f) return current;

        float currentSpeed = Vector3.Dot(current, wishDir);
        float addSpeed = wishSpeed - currentSpeed;
        if (addSpeed <= 0f) return current;

        float accelSpeed = accel * wishSpeed * dt;
        if (accelSpeed > addSpeed) accelSpeed = addSpeed;

        return current + wishDir * accelSpeed;
    }
}
