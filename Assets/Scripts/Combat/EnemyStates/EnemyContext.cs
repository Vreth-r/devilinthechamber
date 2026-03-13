using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public readonly Transform self;
    public readonly NavMeshAgent agent;

    public Transform target;
    public Transform firePoint;
    public EnemyStats stats;

    // runtime / working data
    public float repathTimer;
    public float lastFireTime;

    public EnemyContext(
        Transform self,
        NavMeshAgent agent,
        Transform target,
        Transform firePoint,
        EnemyStats stats)
    {
        this.self = self;
        this.agent = agent;
        this.target = target;
        this.firePoint = firePoint;
        this.stats = stats;
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

    public bool IsTargetInAttackRange()
    {
        return HasTarget && DistanceToTarget() <= stats.attackRange;
    }

    public bool IsTargetOutOfLeashRange()
    {
        return !HasTarget || DistanceToTarget() > stats.leashRange;
    }
}