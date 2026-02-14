using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    readonly EnemyContext ctx;
    readonly EnemyStateMachine fsm;

    public string Name => "Attack";

    float attackTimer;
    const float attackWindup = 0.35f;

    public EnemyAttackState(EnemyContext ctx, EnemyStateMachine fsm)
    {
        this.ctx = ctx;
        this.fsm = fsm;
    }

    public void Enter()
    {
        attackTimer = attackWindup;

        if (ctx.agent)
        {
            ctx.agent.isStopped = true;
            ctx.agent.ResetPath();
        }
    }

    public void Tick(float dt)
    {
        if (!ctx.HasTarget)
        {
            fsm.SetState(new EnemyIdleState(ctx, fsm));
            return;
        }

        float dist = ctx.DistanceToTarget();

        if (dist > ctx.attackRange)
        {
            fsm.SetState(new EnemyChaseState(ctx, fsm));
            return;
        }

        if (ctx.faceTargetWhenStopped)
            FaceTarget(dt);
        
        // placeholder attack
        attackTimer -= dt;
        if (attackTimer <= 0f)
        {
            // later actually deal damage / shoot projectile here
            // for now just loop
            attackTimer = attackWindup;
        }
    }

    void FaceTarget(float dt)
    {
        Vector3 to = ctx.target.position - ctx.self.position;
        to.y = 0f;
        if (to.sqrMagnitude < 0.0001f) return;

        Quaternion desired = Quaternion.LookRotation(to.normalized, Vector3.up);
        ctx.self.rotation = Quaternion.Slerp(ctx.self.rotation, desired, 1f - Mathf.Exp(-ctx.faceTurnSpeed * dt));
    }

    public void Exit()
    {
        if (ctx.agent)
            ctx.agent.isStopped = false;
    }
}
