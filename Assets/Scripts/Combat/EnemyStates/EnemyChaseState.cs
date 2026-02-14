using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    public string Name => "Chase";
    public EnemyChaseState(EnemyContext ctx, EnemyStateMachine fsm)
    {
        this.ctx = ctx;
        this.fsm = fsm;
    }

    public void Enter()
    {
        if (!ctx.agent) return;
        ctx.agent.isStopped = false;
        ctx.agent.stoppingDistance = ctx.stopRange;
        ctx.repathTimer = 0f; // repath ＩＭＭＥＤＩＡＴＥＬＹ
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget)
        {
            fsm.SetState(new EnemyIdleState(ctx, fsm));
            return;
        }

        float dist = ctx.DistanceToTarget();

        // lose aggro
        if (dist > ctx.aggroRange)
        {
            fsm.SetState(new EnemyIdleState(ctx, fsm));
            return;
        }

        // enter attack
        if (dist <= ctx.attackRange)
        {
            fsm.SetState(new EnemyAttackState(ctx, fsm));
            return;
        }

        // pathfind
        if (ctx.agent)
        {
            ctx.repathTimer -= dt;
            if (ctx.repathTimer <= 0f)
            {
                ctx.repathTimer = 1f / Mathf.Max(1f, ctx.repathRateHz);
                ctx.agent.SetDestination(ctx.target.position);
            }
        }
    }

    public void Exit() { }
}
