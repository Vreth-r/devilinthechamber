using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    EnemyAnimDriver fireAnim;

    public string Name => "Turret";

    float nextShotAt;
    readonly Queue<float> scheduledProjectileTimes = new Queue<float>(8);

    public EnemyTurretState(EnemyContext ctx, EnemyStateMachine fsm)
    { 
        this.ctx = ctx; 
        this.fsm = fsm; 
    }

    public void Enter()
    {
        if (ctx.agent)
        {
            if (ctx.agent.enabled && ctx.agent.isOnNavMesh)
            {
                ctx.agent.isStopped = true;
                ctx.agent.ResetPath();
            }
            else
            {
                ctx.agent.isStopped = true;
            }
        }

        fireAnim = ctx.self.GetComponent<EnemyAnimDriver>();
        fireAnim?.PlayWindUp();

        scheduledProjectileTimes.Clear();

        float interval = 1f / Mathf.Max(0.01f, ctx.fireRate);
        nextShotAt = Time.time + interval;
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget) return;

        float dist = ctx.DistanceToTarget();
        if (dist > ctx.aggroRange) return;

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

    public void Exit()
    {
        scheduledProjectileTimes.Clear();
        fireAnim?.PlayWindDown();

        if (ctx.agent)
            ctx.agent.isStopped = false;
    }
}
