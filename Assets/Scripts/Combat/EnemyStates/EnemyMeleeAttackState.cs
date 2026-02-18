using UnityEngine;

public class EnemyMeleeAttackState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    public string Name => "MeleeAttack";

    EnemyMeleeAttack melee;

    float windupTime = 0.20f;
    float cooldownTime = 0.55f;

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

        timer = windupTime;
        didHit = false;
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget)
        {
            fsm.SetState(new EnemyIdleState(ctx, fsm, () => new EnemyMeleeChaseState(ctx, fsm)));
            return;
        }

        float dist = ctx.DistanceToTarget();
        if (dist > ctx.attackRange)
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
            timer = cooldownTime * StatModManager.GetStatModifier(StatName.DOG_RECOVERY_SPEED);
        }
        else if (didHit && timer <= 0f)
        {
            didHit = false;
            timer = windupTime;
        }
    }

    public void Exit()
    {
        if (ctx.agent) ctx.agent.isStopped = false;
    }
}
