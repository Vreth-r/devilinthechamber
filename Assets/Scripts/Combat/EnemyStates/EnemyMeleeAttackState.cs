using UnityEngine;

public class EnemyMeleeAttackState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    public string Name => "MeleeAttack";
    float windup = 0.25f;

    public EnemyMeleeAttackState(EnemyContext ctx, EnemyStateMachine fsm)
    { this.ctx = ctx; this.fsm = fsm; }

    public void Enter()
    {
        ctx.agent.isStopped = true;
        ctx.agent.ResetPath();
        windup = 0.25f;
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget) { fsm.SetState(new EnemyIdleState(ctx, fsm, () => new EnemyMeleeChaseState(ctx, fsm))); return; }

        float dist = ctx.DistanceToTarget();
        if (dist > ctx.attackRange) { fsm.SetState(new EnemyMeleeChaseState(ctx, fsm)); return; }

        EnemyCombatUtil.FaceTarget(ctx, dt);

        windup -= dt;
        if (windup <= 0f)
        {
            // TODO later actually damage player (hitbox / overlap / interface)
            windup = 0.6f; // attack cooldown loop
        }
    }

    public void Exit()
    {
        ctx.agent.isStopped = false;
    }
}
