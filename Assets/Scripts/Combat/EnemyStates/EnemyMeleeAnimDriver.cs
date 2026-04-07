using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(Animator))]
public class EnemyMeleeAnimDriver : MonoBehaviour
{
    public NavMeshAgent agent;
    public string moveSpeedParam = "MoveSpeed";
    public float dampTime = 0.08f;
    public EventReference footstepEvent;

    [Header("Speed mapping")]
    public float speedForMinInterval = 9.0f;
    public float speedForMaxInterval = 2.0f; // slow walk
    public float minMoveSpeed = 0.1f;
    public float minInterval = 0.28f;
    public float maxInterval = 0.50f;
    float timer;

    Animator anim;
    int moveSpeedHash;

    void Awake()
    {
        anim = GetComponent<Animator>();
        moveSpeedHash = Animator.StringToHash(moveSpeedParam);
        if (!agent) agent = GetComponent<NavMeshAgent>();
        timer = Random.Range(0f, 0.1f);
    }

    void Update()
    {
        float speed = agent ? agent.velocity.magnitude : 0f;
        anim.SetFloat(moveSpeedHash, speed, dampTime, Time.deltaTime);

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