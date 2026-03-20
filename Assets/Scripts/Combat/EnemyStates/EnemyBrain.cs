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

    [Header("Projectile")]
    public Transform firePoint;
    public Projectile projectilePrefab;
    public LayerMask projectileHitMask = ~0;

    EnemyStateMachine fsm;
    EnemyContext ctx;
    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!target)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player) target = player.transform;
        }

        ctx = new EnemyContext(
            self: transform,
            agent: agent,
            target: target,
            firePoint: firePoint,
            projectilePrefab: projectilePrefab,
            projectileHitMask: projectileHitMask,
            stats: stats
        );

        fsm = new EnemyStateMachine();
        ApplyStatsToAgent();
    }

    void OnEnable()
    {
        if (!behaviour || !stats)
        {
            Debug.LogError($"{name}: Missing EnemyBehaviour or EnemyStats.");
            enabled = false;
            return;
        }

        fsm.SetState(behaviour.CreateInitialState(ctx, fsm));
    }

    void Update()
    {
        ctx.target = target;
        ctx.firePoint = firePoint;
        ctx.projectilePrefab = projectilePrefab;
        ctx.projectileHitMask = projectileHitMask;
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
}