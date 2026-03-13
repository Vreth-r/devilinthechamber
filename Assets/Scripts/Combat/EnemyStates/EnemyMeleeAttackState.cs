using UnityEngine;

public class EnemyMeleeAttackState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    public string Name => "MeleeAttack";

    EnemyMeleeAttack melee;

    float timer;
    bool didHit;

    public EnemyMeleeAttackState(EnemyContext ctx, EnemyStateMachine fsm)
    {
        this.ctx = ctx;
        this.fsm = fsm;
        melee = ctx.self.GetComponent<EnemyMeleeAttack>();
    }

    public void Enter()
    {
        if (ctx.agent)
        {
            ctx.agent.isStopped = true;
            ctx.agent.ResetPath();
        }

        timer = ctx.stats.meleeWindupTime;
        didHit = false;
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget || ctx.IsTargetOutOfLeashRange())
        {
            fsm.SetState(new EnemyIdleState(ctx, fsm, () => new EnemyMeleeChaseState(ctx, fsm)));
            return;
        }

        if (!ctx.IsTargetInAttackRange())
        {
            fsm.SetState(new EnemyMeleeChaseState(ctx, fsm));
            return;
        }

        EnemyCombatUtil.FaceTarget(ctx, dt);

        timer -= dt;

        if (!didHit && timer <= 0f)
        {
            didHit = true;
            melee?.DoMeleeHit();

            timer = ctx.stats.meleeCooldownTime * StatModManager.GetStatModifier(StatName.DOG_RECOVERY_SPEED);
        }
        else if (didHit && timer <= 0f)
        {
            didHit = false;
            timer = ctx.stats.meleeWindupTime;
        }

        if (ctx.agent && ctx.agent.enabled && ctx.agent.isOnNavMesh)
        {
            ctx.agent.isStopped = false;
            ctx.agent.stoppingDistance = ctx.stats.meleeAttackMoveStoppingDistance;
            ctx.agent.SetDestination(ctx.target.position);
        }
    }

    public void Exit()
    {
        if (ctx.agent) ctx.agent.isStopped = false;
    }
}