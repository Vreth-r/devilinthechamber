using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBrain : MonoBehaviour
{
    [Header("Behaviour")]
    public EnemyBehaviour behaviour;

    [Header("Stats")]
    public EnemyStats stats;

    [Header("Target")]
    public Transform target;

    [Header("Combat")]
    public Transform firePoint;

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
            firePoint: firePoint,
            stats: stats
        );

        fsm = new EnemyStateMachine();

        ApplyStatsToAgent();
    }

    void OnEnable()
    {
        if (!behaviour)
        {
            Debug.LogError($"{name}: No EnemyBehaviour assigned.");
            enabled = false;
            return;
        }

        if (!stats)
        {
            Debug.LogError($"{name}: No EnemyStats assigned.");
            enabled = false;
            return;
        }

        fsm.SetState(behaviour.CreateInitialState(ctx, fsm));
    }

    void Update()
    {
        ctx.target = target;
        ctx.firePoint = firePoint;
        ctx.stats = stats;

        ApplyStatsToAgent();

        fsm.Tick(Time.deltaTime);
    }

    void ApplyStatsToAgent()
    {
        if (!agent || stats == null) return;

        agent.speed = stats.moveSpeed;
        agent.stoppingDistance = stats.stopRange;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!stats) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.aggroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stats.leashRange);
    }
#endif
}