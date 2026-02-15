using UnityEngine;

public class EnemyProjectileWeapon : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;
    public Projectile projectilePrefab;

    [Header("Fire")]
    public float fireRate = 3f;
    public float projectileSpeed = 28f;
    public int damage = 10;
    public LayerMask projectileHitMask = ~0;

    [Header("Aim")]
    public float aimHeight = 1.2f;
    public bool leadTarget = true;
    public float maxLeadTime = 0.75f;
    public float fallbackLeadBlend = 0.35f;

    [Header("Aim Smoothing")]
    public float velocitySmoothing = 12f;
    Vector3 smoothedRelVel;

    [Header("FX (optional)")]
    public ParticleSystem muzzleFlash;
    public Light muzzleLight;

    float nextFireTime;

    public bool TryFireAt(Transform target, Vector3 targetVelocity = default)
    {
        Vector3 shooterVel = Vector3.zero;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent) shooterVel = agent.velocity;

        Vector3 relVel = targetVelocity - shooterVel;
        smoothedRelVel = Vector3.Lerp(smoothedRelVel, relVel, 1f - Mathf.Exp(-velocitySmoothing * Time.deltaTime));

        if (!firePoint || !projectilePrefab || !target) return false;
        if (Time.time < nextFireTime) return false;

        nextFireTime = Time.time + (1f / Mathf.Max(0.01f, fireRate));

        Vector3 origin = firePoint.position;
        Vector3 targetPos;
        if (target.TryGetComponent<CharacterController>(out var tcc))
        {
            targetPos = target.TransformPoint(tcc.center);
        }
        else if (target.TryGetComponent<Collider>(out var col))
        {
            targetPos = col.bounds.center;
        }
        else
        {
            targetPos = target.position + Vector3.up * aimHeight;
        }

        Vector3 aimPoint = targetPos;

        if (leadTarget)
        {
            if (TryGetInterceptPoint(origin, projectileSpeed, targetPos, relVel, maxLeadTime, out Vector3 intercept))
            {
                aimPoint = intercept;
            }
            else
            {
                float dist = Vector3.Distance(origin, targetPos);
                float t = Mathf.Clamp(dist / Mathf.Max(0.01f, projectileSpeed), 0f, 0.20f);
                Vector3 partial = targetPos + targetVelocity * t;

                aimPoint = Vector3.Lerp(targetPos, partial, Mathf.Clamp01(fallbackLeadBlend));
            }
        }

        Vector3 dir = (aimPoint - origin);
        if (dir.sqrMagnitude < 0.0001f) dir = firePoint.forward;
        dir.Normalize();

        Projectile p = Instantiate(projectilePrefab);
        IgnoreShooterCollisions(p.gameObject, transform);
        p.damage = damage;
        p.hitMask = projectileHitMask;
        p.Launch(origin, dir, projectileSpeed);

        if (muzzleFlash) muzzleFlash.Play();
        if (muzzleLight) StartCoroutine(FlashLight());

        return true;
    }

    static void IgnoreShooterCollisions(GameObject projectileGO, Transform shooterRoot)
    {
        var projCols = projectileGO.GetComponentsInChildren<Collider>();
        var shooterCols = shooterRoot.GetComponentsInChildren<Collider>();

        for (int i = 0; i < projCols.Length; i++)
        for (int j = 0; j < shooterCols.Length; j++)
            Physics.IgnoreCollision(projCols[i], shooterCols[j], true);
    }

    static bool TryGetInterceptPoint(
        Vector3 shooterPos,
        float projectileSpeed,
        Vector3 targetPos,
        Vector3 targetVel,
        float maxTime,
        out Vector3 interceptPoint)
    {
        interceptPoint = targetPos;

        Vector3 r = targetPos - shooterPos;
        float s = Mathf.Max(0.01f, projectileSpeed);

        // Solve |r + v t|^2 = (s t)^2
        // => (v·v - s^2)t^2 + 2(r·v)t + (r·r) = 0
        float a = Vector3.Dot(targetVel, targetVel) - (s * s);
        float b = 2f * Vector3.Dot(r, targetVel);
        float c = Vector3.Dot(r, r);

        if (Mathf.Abs(a) < 1e-6f)
        {
            if (Mathf.Abs(b) < 1e-6f)
                return false;

            float ta = -c / b;
            if (ta <= 0f) return false;
            if (ta > maxTime) return false;

            interceptPoint = targetPos + targetVel * ta;
            return true;
        }

        float disc = (b * b) - 4f * a * c;
        if (disc < 0f) return false;

        float sqrtDisc = Mathf.Sqrt(disc);

        float t1 = (-b - sqrtDisc) / (2f * a);
        float t2 = (-b + sqrtDisc) / (2f * a);

        float t = float.PositiveInfinity;

        if (t1 > 0f) t = t1;
        if (t2 > 0f) t = Mathf.Min(t, t2);

        if (!float.IsFinite(t)) return false;
        if (t > maxTime) return false;

        interceptPoint = targetPos + targetVel * t;
        return true;
    }

    System.Collections.IEnumerator FlashLight()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(0.03f);
        muzzleLight.enabled = false;
    }
}
