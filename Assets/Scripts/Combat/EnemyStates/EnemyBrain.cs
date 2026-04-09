using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using FMOD.Studio;

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

    [Header("Sounds")]
    public EventReference attack;

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
            stats: stats,
            OnAttack: HandleAttack
        );

        fsm = new EnemyStateMachine();
        ApplyStatsToAgent();
    }

    private void HandleAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShot(attack, gameObject.transform.position);
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

        agent.speed = stats.moveSpeed * (stats.type == EnemyType.LADY ? StatModManager.GetStatModifier(StatName.LADY_MOVEMENT_SPEED) : StatModManager.GetStatModifier(StatName.DOG_MOVEMENT_SPEED));
        agent.stoppingDistance = stats.stopRange;
    }
}