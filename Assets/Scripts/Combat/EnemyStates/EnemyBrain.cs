using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBrain : MonoBehaviour
{
    [Header("Behaviour")]
    public EnemyBehaviour behaviour;

    [Header("Target")]
    public Transform target;

    [Header("Ranges")]
    public float aggroRange = 25f;
    public float attackRange = 2.0f;
    public float stopRange = 1.8f;

    [Header("Chase")]
    public float repathRateHz = 10f;

    [Header("Facing")]
    public bool faceTargetWhenStopped = true;
    public float faceTurnSpeed = 12f;

    [Header("Combat")]
    public Transform firePoint;
    public float fireRate = 3f;
    public int damage = 10;

    [Header("Ranged Spacing")]
    public float preferredRange = 12f;
    public float rangeTolerance = 2f;

    EnemyStateMachine fsm;
    EnemyContext ctx;
    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!target)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) target = go.transform;
        }

        ctx = new EnemyContext(
            self: transform,
            agent: agent,
            target: target,
            aggroRange: aggroRange,
            attackRange: attackRange,
            stopRange: stopRange,
            repathRateHz: repathRateHz,
            faceTargetWhenStopped: faceTargetWhenStopped,
            faceTurnSpeed: faceTurnSpeed
        );

        fsm = new EnemyStateMachine();
    }

    void OnEnable()
    {
        if (!behaviour)
        {
            Debug.LogError($"{name}: No EnemyBehaviour assigned #stupidbitch");
            return;
        }
        fsm.SetState(behaviour.CreateInitialState(ctx, fsm));
    }

    void Update()
    {
        ctx.target = target;
        ctx.aggroRange = aggroRange;
        ctx.attackRange = attackRange;
        ctx.stopRange = stopRange;
        ctx.repathRateHz = repathRateHz;
        ctx.faceTargetWhenStopped = faceTargetWhenStopped;
        ctx.faceTurnSpeed = faceTurnSpeed;

        ctx.firePoint = firePoint;
        ctx.fireRate = fireRate;
        ctx.damage = damage;

        ctx.preferredRange = preferredRange;
        ctx.rangeTolerance = rangeTolerance;

        fsm.Tick(Time.deltaTime);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
