using UnityEngine;
using UnityEngine.AI;

public class EnemyContext
{
    public readonly Transform self;
    public readonly NavMeshAgent agent;
    public Transform target;

    // tuning
    public float aggroRange;
    public float attackRange;
    public float stopRange;
    public float repathRateHz;

    // combat
    public Transform firePoint;
    public float fireRate;
    public float projectileSpeed;
    public float lastFireTime;
    public int damage;

    // ranged spacing
    public float preferredRange;
    public float rangeTolerance;

    public bool faceTargetWhenStopped;
    public float faceTurnSpeed;

    // working data
    public float repathTimer;

    public EnemyContext(
        Transform self,
        NavMeshAgent agent,
        Transform target,
        float aggroRange,
        float attackRange,
        float stopRange,
        float repathRateHz,
        bool faceTargetWhenStopped,
        float faceTurnSpeed)
    {
        this.self = self;
        this.agent = agent;
        this.target = target;
        this.aggroRange = aggroRange;
        this.attackRange = attackRange;
        this.stopRange = stopRange;
        this.repathRateHz = repathRateHz;
        this.faceTargetWhenStopped = faceTargetWhenStopped;
        this.faceTurnSpeed = faceTurnSpeed;
    }

    public float DistanceToTarget()
    {
        if (!target) return float.PositiveInfinity;
        return Vector3.Distance(self.position, target.position);
    }

    public bool HasTarget => target != null;
}