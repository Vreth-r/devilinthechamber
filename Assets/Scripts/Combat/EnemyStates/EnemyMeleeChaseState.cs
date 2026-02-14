using UnityEngine;

public class EnemyMeleeChaseState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    public string Name => "MeleeChase";

    public EnemyMeleeChaseState(EnemyContext ctx, EnemyStateMachine fsm)
    { this.ctx = ctx; this.fsm = fsm; }

    public void Enter()
    {
        ctx.agent.isStopped = false;
        ctx.agent.stoppingDistance = ctx.attackRange;
        ctx.repathTimer = 0f;
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget) { fsm.SetState(new EnemyIdleState(ctx, fsm, () => new EnemyMeleeChaseState(ctx, fsm))); return; }

        float dist = ctx.DistanceToTarget();
        if (dist > ctx.aggroRange) { fsm.SetState(new EnemyIdleState(ctx, fsm, () => new EnemyMeleeChaseState(ctx, fsm))); return; }

        if (dist <= ctx.attackRange)
        {
            fsm.SetState(new EnemyMeleeAttackState(ctx, fsm));
            return;
        }

        ctx.repathTimer -= dt;
        if (ctx.repathTimer <= 0f)
        {
            ctx.repathTimer = 1f / Mathf.Max(1f, ctx.repathRateHz);
            ctx.agent.SetDestination(ctx.target.position);
        }
    }

    public void Exit() { }
}
