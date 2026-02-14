using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    public string Name => "Idle";

    public EnemyIdleState(EnemyContext ctx, EnemyStateMachine fsm)
    {
        this.ctx = ctx;
        this.fsm = fsm;
    }

    public void Enter()
    {
        if (ctx.agent)
        {
            ctx.agent.isStopped = true;
            ctx.agent.ResetPath();
        }
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget) return;

        if (ctx.DistanceToTarget() <= ctx.aggroRange)
        {
            fsm.SetState(new EnemyChaseState(ctx, fsm));
        }
    }

    public void Exit() { }
}
