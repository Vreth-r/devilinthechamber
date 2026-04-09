using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public readonly Transform self;
    public readonly NavMeshAgent agent;

    public Transform target;
    public Transform firePoint;
    public Projectile projectilePrefab;
    public LayerMask projectileHitMask;
    public EnemyStats stats;

    public System.Action OnAttack;

    public EnemyContext(
        Transform self,
        NavMeshAgent agent,
        Transform target,
        Transform firePoint,
        Projectile projectilePrefab,
        LayerMask projectileHitMask,
        EnemyStats stats,
        System.Action OnAttack = null)
    {
        this.self = self;
        this.agent = agent;
        this.target = target;
        this.firePoint = firePoint;
        this.projectilePrefab = projectilePrefab;
        this.projectileHitMask = projectileHitMask;
        this.stats = stats;
        this.OnAttack = OnAttack;
    }

    public bool HasTarget => target != null;

    public float DistanceToTarget()
    {
        if (!target) return float.PositiveInfinity;
        return Vector3.Distance(self.position, target.position);
    }

    public bool IsTargetInAggroRange()
    {
        return HasTarget && DistanceToTarget() <= stats.aggroRange;
    }
}