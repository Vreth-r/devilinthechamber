using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    [Header("References")]
    public Transform cameraPivot;
    public PlayerSound sound;

    [Header("Movement")]
    public float maxGroundSpeed = 9f;
    public float maxAirSpeed = 9f;
    public float groundAcceleration = 60f;
    public float airAcceleration = 25f;
    public float groundFriction = 14f;

    [Header("Jump")]
    public float jumpHeight = 3.4f;
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
    public float slideAdditiveBoost = 10f;

    // decay
    public float slideDecayHalfLife = 0.10f;
    public float slideMinEndSpeed = 2.0f;

    // steering
    public float slideSteerAccel = 10f;

    public float slideFriction = 2.5f;
    public float slideCooldown = 0.12f;

    [Header("Air -> Slide Buffer")]
    public float slideLandBuffer = 0.20f;
    public float slideLandGrace = 0.06f;

    float slideBufferTimer;
    bool wasGrounded;
    float landGraceTimer;

    [Header("Slide Jump Launch")]
    public float slideJumpWindow = 0.12f;
    public float slideJumpHeight = 0.65f;
    public float slideJumpForwardBoost = 7f;
    public float slideJumpForwardBoostScale = 0.12f;

    bool slideJumpUsed;

    [Header("Ceiling Check")]
    public LayerMask obstructionMask = ~0;

    [Header("FX")]
    public FovKick fovKick;
    public FovKick viewportFovKick;

    CharacterController cc;
    PlayerControls controls;

    public CameraMovement cameraMovement;

    Vector3 velocity;
    Vector3 planarVelocity;
    public Vector3 PlanarVelocity => planarVelocity;
    public float PlanarSpeed => planarVelocity.magnitude;
    public bool IsStanding => stance == Stance.Stand;
    public Vector3 FullVelocity => planarVelocity + Vector3.up * velocity.y;

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
        if (GameManager.Instance.gamePaused) return;
        
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

        bool justLanded = grounded && !wasGrounded;
        wasGrounded = grounded;

        if (justLanded)
        {
            landGraceTimer = slideLandGrace;
            sound.PlayLand();
        }
        else
        {
            landGraceTimer -= Time.deltaTime;
        }

        if (crouchPressed && !grounded)
        {
            slideBufferTimer = slideLandBuffer;
        }
        else
        {
            slideBufferTimer -= Time.deltaTime;
        }

        Vector3 wishDir = GetWishDirection(moveInput);

        if (stance != Stance.Slide)
        {
            bool slideBuffered = slideBufferTimer > 0f && (justLanded || landGraceTimer > 0f);

            if (slideBuffered && CanStartSlideNow(grounded))
            {
                // consume buffer
                slideBufferTimer = 0f;
                StartSlide(wishDir);
            }
            
            else if (crouchPressed && CanStartSlideNow(grounded) && 
            !AbilityModManager.abilityFlags[AbilityName.NO_SLIDING] && 
            !(AbilityModManager.abilityFlags[AbilityName.NO_SLIDE_OR_JUMP_LOW_HP] && 
            (PlayerManager.Instance.health.currentHealth / (float)PlayerManager.Instance.health.maxHealth) <= 0.25))
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

            if (!slideJumpUsed && controls.Player.Jump.WasPressedThisFrame() && slideTimer <= slideJumpWindow)
            {
                DoSlideJumpLaunch(wishDir);
            }

            if (stance == Stance.Slide) // **** DoSlideJumpLaunch might wipe slide state, so this needs another check to not run slide code if we're not sliding
            {
                float halfLife = Mathf.Max(0.001f, slideDecayHalfLife);
                float decay = Mathf.Pow(0.5f, Time.deltaTime / halfLife);
                planarVelocity *= decay;

                slideTimer -= Time.deltaTime;

                if (fovKick)
                {
                    fovKick.UpdateSlideSpeed(planarVelocity.magnitude);
                    viewportFovKick.UpdateSlideSpeed(planarVelocity.magnitude);
                }

                if (slideTimer <= 0f || planarVelocity.magnitude <= slideMinEndSpeed || !grounded)
                {
                    EndSlide(controls.Player.Crouch.IsPressed());
                }
            }
        }
        else
        {
            float speedMult = (stance == Stance.Crouch) ? crouchSpeedMultiplier : 1f;

            if (grounded)
            {
                planarVelocity = ApplyFriction(planarVelocity, groundFriction, Time.deltaTime);
                planarVelocity = Accelerate(planarVelocity, wishDir, maxGroundSpeed * speedMult * StatModManager.GetStatModifier(StatName.MOVEMENT_SPEED), groundAcceleration, Time.deltaTime);
            }
            else
            {
                float aa = airAcceleration;
                if (AbilityModManager.abilityFlags[AbilityName.FROG_LEGS]) 
                { 
                    aa = 50;
                    speedMult *= 1.3f;
                }
                planarVelocity = Accelerate(planarVelocity, wishDir, maxAirSpeed * speedMult *  StatModManager.GetStatModifier(StatName.MOVEMENT_SPEED), aa, Time.deltaTime);
            }
        }

        if (jumpBufferTimer > 0f && coyoteTimer > 0f && stance != Stance.Slide && 
        !AbilityModManager.abilityFlags[AbilityName.NO_JUMPING] &&
        !(AbilityModManager.abilityFlags[AbilityName.NO_SLIDE_OR_JUMP_LOW_HP] && 
        (PlayerManager.Instance.health.currentHealth / (float)PlayerManager.Instance.health.maxHealth) <= 0.25))
        {
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;

            if (stance == Stance.Crouch) TryStandUp(force: true);

            velocity.y = Mathf.Sqrt(2f * gravity * jumpHeight) * StatModManager.GetStatModifier(StatName.JUMP_HEIGHT);
            sound.PlayJump();
        }

        velocity.y -= gravity * Time.deltaTime;

        Vector3 move = planarVelocity + Vector3.up * velocity.y;
        cc.Move(move * Time.deltaTime);

        if (fovKick)
        {
            fovKick.SetSpeed(planarVelocity.magnitude);
            viewportFovKick.UpdateSlideSpeed(planarVelocity.magnitude);
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

        slideJumpUsed = false;
        sound.StartSlideLoop();

        slideTimer = slideDuration * StatModManager.GetStatModifier(StatName.SLIDE_DISTANCE);
        slideCooldownTimer = slideCooldown * StatModManager.GetStatModifier(StatName.SLIDE_COOLDOWN);;

        // direction prefer current velocity, else wishDir, else forward
        Vector3 dir =
            planarVelocity.sqrMagnitude > 0.01f ? planarVelocity.normalized :
            wishDir.sqrMagnitude > 0.01f ? wishDir :
            (cameraPivot ? Vector3.ProjectOnPlane(cameraPivot.forward, Vector3.up).normalized : transform.forward);

        float currentSpeed = planarVelocity.magnitude;
        float startSpeed = Mathf.Max(currentSpeed, slideMinStartSpeed);

        float boosted = startSpeed + slideAdditiveBoost;
        planarVelocity = dir * Mathf.Max(boosted, slideBurstSpeed * StatModManager.GetStatModifier(StatName.SLIDE_SPEED));
        planarVelocity = dir * Mathf.Max(startSpeed, slideBurstSpeed * StatModManager.GetStatModifier(StatName.SLIDE_SPEED));
        if (fovKick)
        {
            fovKick.BeginSlide(planarVelocity.magnitude);
            viewportFovKick.BeginSlide(planarVelocity.magnitude);
        }
    }

    bool CanStartSlideNow(bool grounded)
    {
        if (!grounded) return false;
        if (stance == Stance.Slide) return false;
        if (slideCooldownTimer > 0f) return false;
        return planarVelocity.magnitude >= slideMinStartSpeed;
    }

    void EndSlide(bool crouchHeldAfter)
    {
        sound.StopSlideLoop();
        if (fovKick)
        {
            fovKick.EndSlide();
            viewportFovKick.EndSlide();
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

    void DoSlideJumpLaunch(Vector3 wishDir)
    {
        slideJumpUsed = true;
        sound.StopSlideLoop();
        sound.PlayJump();

        if (fovKick)
        {
            fovKick.EndSlide();
            viewportFovKick.EndSlide();
        }

        if (CanStandUp()) SetStance(Stance.Stand);
        else SetStance(Stance.Crouch);

        Vector3 dir = planarVelocity.sqrMagnitude > 0.01f
            ? planarVelocity.normalized
            : (wishDir.sqrMagnitude > 0.01f ? wishDir : Vector3.ProjectOnPlane(cameraPivot.forward, Vector3.up).normalized);

        float speed = planarVelocity.magnitude;
        float bonus = slideJumpForwardBoost + speed * slideJumpForwardBoostScale;

        planarVelocity += dir * bonus;

        velocity.y = Mathf.Sqrt(2f * gravity * slideJumpHeight);

        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
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
