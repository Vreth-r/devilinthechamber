using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(CharacterController))]
public class PlayerFootstepsFMOD : MonoBehaviour
{
    [Header("FMOD")]
    public EventReference footstepEvent;

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

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        cc = GetComponent<CharacterController>();
        timer = Random.Range(0f, 0.1f);
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
        if (footstepEvent.IsNull) return;
        RuntimeManager.PlayOneShotAttached(footstepEvent, gameObject);
    }
}
