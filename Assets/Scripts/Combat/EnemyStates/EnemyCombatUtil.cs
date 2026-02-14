using UnityEngine;

public static class EnemyCombatUtil
{
    public static void FaceTarget(EnemyContext ctx, float dt)
    {
       if (!ctx.target) return;
        Vector3 to = ctx.target.position - ctx.self.position;
        to.y = 0f;
        if (to.sqrMagnitude < 0.0001f) return;

        Quaternion desired = Quaternion.LookRotation(to.normalized, Vector3.up);
        ctx.self.rotation = Quaternion.Slerp(ctx.self.rotation, desired, 1f - Mathf.Exp(-ctx.faceTurnSpeed * dt));
    }

    public static bool CanFire(EnemyContext ctx, float timeNow)
    {
        float interval = 1f / Mathf.Max(0.01f, ctx.fireRate);
        return timeNow - ctx.lastFireTime >= interval;
    }
}