using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform attackOrigin;

    [Header("Collision")]
    [SerializeField] private LayerMask hitMask = ~0;

    private EnemyBrain brain;
    private EnemyStats stats;

    void Awake()
    {
        CacheDependencies();
    }

    void CacheDependencies()
    {
        if (!brain) brain = GetComponent<EnemyBrain>();
        stats = brain != null ? brain.stats : null;
    }

    bool HasValidSetup()
    {
        if (!brain) brain = GetComponent<EnemyBrain>();
        if (brain != null) stats = brain.stats;

        return stats != null;
    }

    public void DoMeleeHit()
    {
        if (!HasValidSetup())
            return;

        Transform originTransform = attackOrigin ? attackOrigin : transform;

        Vector3 center =
            originTransform.position +
            originTransform.forward * stats.meleeAttackRangeForward +
            Vector3.up * stats.meleeAttackHeightOffset;

        Collider[] hits = new Collider[8];
        int count = Physics.OverlapSphereNonAlloc(
            center,
            stats.meleeAttackRadius,
            hits,
            hitMask,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < count; i++)
        {
            Collider col = hits[i];
            if (!col) continue;

            if (col.transform.IsChildOf(transform)) continue;

            IDamageable damageable = col.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                Vector3 hitPoint = col.ClosestPoint(center);
                Vector3 normal =
                    (hitPoint - center).sqrMagnitude > 0.0001f
                        ? (hitPoint - center).normalized
                        : -originTransform.forward;

                damageable.TakeDamage(stats.damage, hitPoint, normal);
                return;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        EnemyBrain localBrain = GetComponent<EnemyBrain>();
        EnemyStats localStats = localBrain ? localBrain.stats : null;
        if (!localStats) return;

        Transform originTransform = attackOrigin ? attackOrigin : transform;

        Vector3 center =
            originTransform.position +
            originTransform.forward * localStats.meleeAttackRangeForward +
            Vector3.up * localStats.meleeAttackHeightOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, localStats.meleeAttackRadius);
    }
#endif
}