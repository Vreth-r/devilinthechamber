using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Transform attackOrigin;       // optional (chest/hands). If null, uses transform
    public float attackRadius = 1.2f;
    public float attackRangeForward = 0.6f;
    public int damage = 12;

    public LayerMask hitMask = ~0;

    public void DoMeleeHit()
    {
        Transform o = attackOrigin ? attackOrigin : transform;

        Vector3 center = o.position + o.forward * attackRangeForward + Vector3.up * 1.0f;

        Collider[] hits = new Collider[8];
        int count = Physics.OverlapSphereNonAlloc(center, attackRadius, hits, hitMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < count; i++)
        {
            var col = hits[i];
            if (!col) continue;

            if (col.transform.IsChildOf(transform)) continue;

            var dmg = col.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                Vector3 hitPoint = col.ClosestPoint(center);
                Vector3 normal = (hitPoint - center).sqrMagnitude > 0.0001f ? (hitPoint - center).normalized : -o.forward;

                dmg.TakeDamage(damage, hitPoint, normal);
                return;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Transform o = attackOrigin ? attackOrigin : transform;
        Vector3 center = o.position + o.forward * attackRangeForward + Vector3.up * 1.0f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
#endif
}
