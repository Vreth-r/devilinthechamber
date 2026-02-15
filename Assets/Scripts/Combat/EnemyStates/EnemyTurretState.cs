using UnityEngine;

public class EnemyTurretState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    public string Name => "Turret";

    public EnemyTurretState(EnemyContext ctx, EnemyStateMachine fsm)
    { this.ctx = ctx; this.fsm = fsm; }

    public void Enter()
    {
        if (ctx.agent)
        {
            ctx.agent.isStopped = true;
            ctx.agent.ResetPath();
            // optional disable agent entirely if giving bugs
            // ctx.agent.enabled = false;
        }
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget) return;

        float dist = ctx.DistanceToTarget();
        if (dist > ctx.aggroRange) return;

        EnemyCombatUtil.FaceTarget(ctx, dt);

        if (ctx.firePoint && EnemyCombatUtil.CanFire(ctx, Time.time))
        {
            ctx.lastFireTime = Time.time;
            var weapon = ctx.self.GetComponent<EnemyProjectileWeapon>();
            if (weapon)
            {
                Vector3 playerVel = Vector3.zero;
                var pm = ctx.target.GetComponent<PlayerMotor>();
                if (pm) playerVel = pm.PlanarVelocity; // or pm.FullVelocity

                weapon.TryFireAt(ctx.target, playerVel);
            }
        }
    }

    public void Exit() { }
}
