using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedKiteState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    EnemyAnimDriver fireAnim;

    public string Name => "RangedKite";

    const float orbitStep = 10f;
    const float retreatStep = 11f;
    const float approachStep = 6f;
    const float orbitRecalcHz = 6f;
    float orbitTimer;
    int orbitDir = 1;

    float nextShotAt;

    readonly Queue<float> scheduledProjectileTimes = new Queue<float>(8);

    public EnemyRangedKiteState(EnemyContext ctx, EnemyStateMachine fsm)
    {
        this.ctx = ctx;
        this.fsm = fsm;
    }

    public void Enter()
    {
        if (!ctx.agent) return;

        ctx.agent.isStopped = false;
        ctx.agent.stoppingDistance = 0f;
        ctx.repathTimer = 0f;
        orbitTimer = 0f;

        fireAnim = ctx.self.GetComponent<EnemyAnimDriver>();
        fireAnim?.PlayWindUp();

        nextShotAt = Time.time + (1f / Mathf.Max(0.01f, ctx.fireRate));

        scheduledProjectileTimes.Clear();

        orbitDir = Random.value < 0.5f ? -1 : 1;
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget)
        {
            fsm.SetState(new EnemyIdleState(ctx, fsm, () => new EnemyRangedKiteState(ctx, fsm)));
            return;
        }

        float dist = ctx.DistanceToTarget();
        if (dist > ctx.aggroRange)
        {
            fsm.SetState(new EnemyIdleState(ctx, fsm, () => new EnemyRangedKiteState(ctx, fsm)));
            return;
        }

        EnemyCombatUtil.FaceTarget(ctx, dt);

        float interval = 1f / Mathf.Max(0.01f, ctx.fireRate);

        if (Time.time >= nextShotAt)
        {
            nextShotAt += interval;

            fireAnim?.PlayShoot();

            float delay = fireAnim ? fireAnim.FireDelaySeconds : 0f;
            scheduledProjectileTimes.Enqueue(Time.time + delay);
        }

        if (scheduledProjectileTimes.Count > 0 && Time.time >= scheduledProjectileTimes.Peek())
        {
            scheduledProjectileTimes.Dequeue();
            FireProjectileNow();
        }

        float min = ctx.preferredRange - ctx.rangeTolerance;
        float max = ctx.preferredRange + ctx.rangeTolerance;

        ctx.repathTimer -= dt;

        if (dist < min)
        {
            if (ctx.repathTimer <= 0f)
            {
                ctx.repathTimer = 1f / Mathf.Max(1f, ctx.repathRateHz);

                Vector3 away = (ctx.self.position - ctx.target.position);
                away.y = 0f;
                if (away.sqrMagnitude < 0.001f) away = ctx.self.forward;
                away.Normalize();

                Vector3 desired = ctx.self.position + away * retreatStep;
                SetAgentDestinationSafe(desired);
            }
            return;
        }

        if (dist > max)
        {
            if (ctx.repathTimer <= 0f)
            {
                ctx.repathTimer = 1f / Mathf.Max(1f, ctx.repathRateHz);

                Vector3 to = (ctx.target.position - ctx.self.position);
                to.y = 0f;
                if (to.sqrMagnitude < 0.001f) to = ctx.self.forward;
                to.Normalize();

                Vector3 desired = ctx.self.position + to * approachStep;
                SetAgentDestinationSafe(desired);
            }
            return;
        }

        orbitTimer -= dt;
        if (orbitTimer <= 0f)
        {
            orbitTimer = 1f / orbitRecalcHz;

            if (Random.value < 0.15f) orbitDir *= -1;

            Vector3 to = ctx.target.position - ctx.self.position;
            to.y = 0f;
            if (to.sqrMagnitude < 0.001f) to = ctx.self.forward;
            to.Normalize();

            Vector3 side = Vector3.Cross(Vector3.up, to) * orbitDir;

            float bias = Mathf.InverseLerp(min, max, dist);
            float radial = Mathf.Lerp(+1.2f, -1.2f, bias);

            Vector3 desired = ctx.self.position + side * orbitStep + to * radial;
            SetAgentDestinationSafe(desired);
        }
    }

    void FireProjectileNow()
    {
        var weapon = ctx.self.GetComponent<EnemyProjectileWeapon>();
        if (!weapon) return;

        Vector3 playerVel = Vector3.zero;
        var pm = ctx.target.GetComponent<PlayerMotor>();
        playerVel = pm ? pm.FullVelocity : Vector3.zero;

        weapon.FireNow(ctx.target, playerVel);
    }

    void SetAgentDestinationSafe(Vector3 desiredWorldPos)
    {
        if (!ctx.agent) return;

        ctx.agent.isStopped = false;

        if (NavMesh.SamplePosition(desiredWorldPos, out NavMeshHit hit, 2.5f, NavMesh.AllAreas))
            ctx.agent.SetDestination(hit.position);
        else
        {
            NavMesh.SamplePosition(ctx.self.position, out hit, 2.5f, NavMesh.AllAreas);
            ctx.agent.SetDestination(hit.position);
        }
    }

    public void Exit()
    {
        if (ctx.agent) ctx.agent.isStopped = false;
        scheduledProjectileTimes.Clear();

        fireAnim?.PlayWindDown();
    }
}
