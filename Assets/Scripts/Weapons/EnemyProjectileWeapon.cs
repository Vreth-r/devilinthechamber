using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyProjectileWeapon : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;

    [Header("Collision")]
    [SerializeField] private LayerMask projectileHitMask = ~0;

    [Header("FX (optional)")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Light muzzleLight;

    private EnemyBrain brain;
    private EnemyStats stats;
    private NavMeshAgent agent;
    private LadyEnemySound sound;

    private Vector3 smoothedRelVel;

    public Transform FirePoint => firePoint;

    void Awake()
    {
        CacheDependencies();
    }

    void OnValidate()
    {
        if (!firePoint)
            firePoint = transform;
    }

    void CacheDependencies()
    {
        if (!brain) brain = GetComponent<EnemyBrain>();
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!sound) sound = GetComponent<LadyEnemySound>();

        stats = brain != null ? brain.stats : null;
    }

    bool HasValidSetup()
    {
        if (!brain) brain = GetComponent<EnemyBrain>();
        if (brain != null) stats = brain.stats;

        if (!firePoint || !projectilePrefab || stats == null)
            return false;

        return true;
    }

    public void FireNow(Transform target, Vector3 targetVelocity = default)
    {
        if (!HasValidSetup() || !target)
            return;

        Vector3 shooterVel = agent ? agent.velocity : Vector3.zero;
        float projectileSpeedMod = StatModManager.GetStatModifier(StatName.LADY_PROJECTILE_SPEED);

        Vector3 relVel = targetVelocity - shooterVel;
        float smoothing = Mathf.Max(0.01f, stats.velocitySmoothing);
        smoothedRelVel = Vector3.Lerp(
            smoothedRelVel,
            relVel,
            1f - Mathf.Exp(-smoothing * Time.deltaTime)
        );

        Vector3 origin = firePoint.position;
        Vector3 targetPos = GetTargetAimPosition(target, stats.projectileAimHeight);
        Vector3 aimPoint = targetPos;

        if (stats.leadTarget)
        {
            float projectileSpeed = stats.projectileSpeed * projectileSpeedMod;

            if (TryGetInterceptPoint(
                origin,
                projectileSpeed,
                targetPos,
                smoothedRelVel,
                stats.maxLeadTime,
                out Vector3 intercept))
            {
                aimPoint = intercept;
            }
            else
            {
                float dist = Vector3.Distance(origin, targetPos);
                float t = Mathf.Clamp(dist / Mathf.Max(0.01f, projectileSpeed), 0f, 0.20f);
                Vector3 partial = targetPos + targetVelocity * t;
                aimPoint = Vector3.Lerp(targetPos, partial, Mathf.Clamp01(stats.fallbackLeadBlend));
            }
        }


        Vector3 dir = aimPoint - origin;
        if (dir.sqrMagnitude < 0.0001f)
            dir = firePoint.forward;
        dir.Normalize();

        float finalSpeed = stats.projectileSpeed * projectileSpeedMod;

        Projectile projectileInstance = Instantiate(projectilePrefab);
        IgnoreShooterCollisions(projectileInstance.gameObject, transform);
        sound.PlayShootSound();

        projectileInstance.damage = stats.damage;
        projectileInstance.hitMask = projectileHitMask;
        projectileInstance.Launch(
            origin,
            dir,
            finalSpeed,
            stats.projectileMaxTravelDistance,
            stats.projectileLifetimeSafetyBuffer
        );

        if (muzzleFlash) muzzleFlash.Play();
        if (muzzleLight) StartCoroutine(FlashLight());
    }

    static Vector3 GetTargetAimPosition(Transform target, float fallbackAimHeight)
    {
        if (target.TryGetComponent<CharacterController>(out var characterController))
            return target.TransformPoint(characterController.center);

        if (target.TryGetComponent<Collider>(out var collider))
            return collider.bounds.center;

        return target.position + Vector3.up * fallbackAimHeight;
    }

    static void IgnoreShooterCollisions(GameObject projectileGO, Transform shooterRoot)
    {
        var projectileColliders = projectileGO.GetComponentsInChildren<Collider>();
        var shooterColliders = shooterRoot.GetComponentsInChildren<Collider>();

        for (int i = 0; i < projectileColliders.Length; i++)
        {
            for (int j = 0; j < shooterColliders.Length; j++)
            {
                Physics.IgnoreCollision(projectileColliders[i], shooterColliders[j], true);
            }
        }
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

        float a = Vector3.Dot(targetVel, targetVel) - (s * s);
        float b = 2f * Vector3.Dot(r, targetVel);
        float c = Vector3.Dot(r, r);

        if (Mathf.Abs(a) < 1e-6f)
        {
            if (Mathf.Abs(b) < 1e-6f)
                return false;

            float linearT = -c / b;
            if (linearT <= 0f || linearT > maxTime)
                return false;

            interceptPoint = targetPos + targetVel * linearT;
            return true;
        }

        float disc = (b * b) - 4f * a * c;
        if (disc < 0f)
            return false;

        float sqrtDisc = Mathf.Sqrt(disc);

        float t1 = (-b - sqrtDisc) / (2f * a);
        float t2 = (-b + sqrtDisc) / (2f * a);

        float t = float.PositiveInfinity;

        if (t1 > 0f) t = t1;
        if (t2 > 0f) t = Mathf.Min(t, t2);

        if (!float.IsFinite(t) || t > maxTime)
            return false;

        interceptPoint = targetPos + targetVel * t;
        return true;
    }

    IEnumerator FlashLight()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(0.03f);
        muzzleLight.enabled = false;
    }
}