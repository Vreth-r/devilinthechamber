using UnityEngine;

public class EnemyHitscanWeapon : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;

    [Header("Fire")]
    public float range = 60f;
    public int damage = 8;
    public float fireRate = 3f;
    public LayerMask hitMask = ~0;

    [Header("FX")]
    public ParticleSystem muzzleFlash;
    public Light muzzleLight;

    public GameObject tracerPrefab;

    float nextFireTime;

    public bool TryFireAt(Transform target)
    {
        if (!firePoint || !target) return false;
        if (Time.time < nextFireTime) return false;

        nextFireTime = Time.time + (1f / Mathf.Max(0.01f, fireRate));

        Vector3 origin = firePoint.position;

        Vector3 aimPoint = target.position + Vector3.up * 1.2f;
        Vector3 dir = (aimPoint - origin).normalized;

        Vector3 endPoint = origin + dir * range;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            endPoint = hit.point;

            var dmg = hit.collider.GetComponentInParent<IDamageable>();
            if (dmg != null)
                dmg.TakeDamage(damage, hit.point, hit.normal);
        }

        if (muzzleFlash) muzzleFlash.Play();
        if (muzzleLight) StartCoroutine(FlashLight());

        if (tracerPrefab)
        {
            var go = Instantiate(tracerPrefab);
            var tracer = go.GetComponent<TracerFX>();
            if (tracer) tracer.Init(origin, endPoint);
            else
            {
                var lr = go.GetComponent<LineRenderer>();
                if (lr)
                {
                    lr.positionCount = 2;
                    lr.SetPosition(0, origin);
                    lr.SetPosition(1, endPoint);
                }
                Destroy(go, 0.06f);
            }
        }

        return true;
    }

    System.Collections.IEnumerator FlashLight()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(0.03f);
        muzzleLight.enabled = false;
    }
}