using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(CharacterController))]
public class PlayerSound : MonoBehaviour
{
    [Header("FMOD")]
    public EventReference footstepEvent;
    public EventReference balloonstepEvent;
    public EventReference slide;
    public EventReference land;
    public EventReference balloonLand;
    public EventReference jump;
    public EventReference balloonJump;
    public EventReference playerDamage;

    [Header("When to play")]
    public float minMoveSpeed = 0.15f;
    public float minInterval = 0.18f;
    public float maxInterval = 0.50f;

    [Header("Speed mapping")]
    public float speedForMinInterval = 9.0f;
    public float speedForMaxInterval = 2.0f; // slow walk

    [Header("Ground check")]
    public bool requireGrounded = true;

    PlayerMotor motor;
    CharacterController cc;

    float timer;

    EventInstance slideInst;
    bool slidePlaying;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        cc = GetComponent<CharacterController>();
        timer = Random.Range(0f, 0.1f);
    }

    void OnDestroy()
    {
        StopSlideLoop(immediate: false);
    }

    void Update()
    {
        // Only in Stand stance
        if (!motor.IsStanding) { timer = 0f; return; }

        // Only when grounded
        if (requireGrounded && !cc.isGrounded) { timer = 0f; return; }

        float speed = motor.PlanarSpeed; // horizontal speed

        // Only when moving
        if (speed < minMoveSpeed) { timer = 0f; return; }

        // cadence from speed
        float t = Mathf.InverseLerp(speedForMaxInterval, speedForMinInterval, speed);
        float interval = Mathf.Lerp(maxInterval, minInterval, t);

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer -= interval;
            PlayFootstep();
        }
    }

    void PlayFootstep()
    {
        if(AbilityModManager.abilityFlags[AbilityName.BALLOON_SHOES])
        {
            if (balloonstepEvent.IsNull) return;
            RuntimeManager.PlayOneShotAttached(balloonstepEvent, gameObject);
        }
        else
        {
            if (footstepEvent.IsNull) return;
            RuntimeManager.PlayOneShotAttached(footstepEvent, gameObject);
        }
    }

    public void StartSlideLoop()
    {
        if (slide.IsNull) return;
        if (slidePlaying && slideInst.isValid()) return;

        slideInst = RuntimeManager.CreateInstance(slide);
        RuntimeManager.AttachInstanceToGameObject(slideInst, gameObject, cc); // CC is fine as velocity source
        slideInst.start();
        slidePlaying = true;
    }

    public void StopSlideLoop(bool immediate = false)
    {
        if (!slideInst.isValid()) { slidePlaying = false; return; }

        slideInst.stop(immediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        slideInst.release();
        slideInst.clearHandle();
        slidePlaying = false;
    }

    public void PlayJump()
    {
        if(AbilityModManager.abilityFlags[AbilityName.BALLOON_SHOES])
        {
            if (balloonJump.IsNull) return;
            RuntimeManager.PlayOneShotAttached(balloonJump, gameObject);
        }
        else
        {
            if (jump.IsNull) return;
            RuntimeManager.PlayOneShotAttached(jump, gameObject);
        }
    }

    public void PlayLand()
    {
        if(AbilityModManager.abilityFlags[AbilityName.BALLOON_SHOES])
        {
            if (balloonLand.IsNull) return;
            RuntimeManager.PlayOneShotAttached(balloonLand, gameObject);
        }
        else
        {
            if (land.IsNull) return;
            RuntimeManager.PlayOneShotAttached(land, gameObject);
        }
    }

    public void PlayPlayerDamage()
    {
        if (playerDamage.IsNull) return;
        RuntimeManager.PlayOneShotAttached(playerDamage, gameObject);
    }
}
