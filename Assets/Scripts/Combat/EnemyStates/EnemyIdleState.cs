using UnityEngine;
using System;

public class EnemyIdleState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;
    readonly Func<IEnemyState> nextOnAggro;

    public string Name => "Idle";

    public EnemyIdleState(EnemyContext ctx, EnemyStateMachine fsm, Func<IEnemyState> nextOnAggro)
    {
        this.ctx = ctx;
        this.fsm = fsm;
        this.nextOnAggro = nextOnAggro;
    }

    public void Enter()
    {
        if (ctx.agent && ctx.agent.enabled && ctx.agent.isOnNavMesh)
        {
            ctx.agent.isStopped = true;
            ctx.agent.ResetPath();
        }
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget) return;

        if (ctx.IsTargetInAggroRange())
            fsm.SetState(nextOnAggro());
    }

    public void Exit() { }
}