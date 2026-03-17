using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 10;
    public LayerMask hitMask = ~0;

    [Header("Motion")]
    public float speed = 25f;
    public bool useGravity = false;

    [Header("Fallback Lifetime")]
    public float lifetime = 5f;

    [Header("Impact FX (optional)")]
    public GameObject impactPrefab;

    private Rigidbody rb;

    private Vector3 spawnPosition;
    private float maxTravelDistance;
    private float dieAt;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Launch(
        Vector3 position,
        Vector3 direction,
        float speedOverride = -1f,
        float maxDistanceOverride = -1f,
        float safetyBuffer = 0.25f)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);

        float finalSpeed = speedOverride > 0f ? speedOverride : speed;
        speed = finalSpeed;

        rb.useGravity = useGravity;
        rb.linearVelocity = direction.normalized * finalSpeed;

        spawnPosition = position;
        maxTravelDistance = maxDistanceOverride > 0f ? maxDistanceOverride : finalSpeed * lifetime;

        float derivedLifetime = maxTravelDistance / Mathf.Max(0.01f, finalSpeed);
        dieAt = Time.time + derivedLifetime + Mathf.Max(0f, safetyBuffer);
        Debug.DrawRay(spawnPosition, direction * 2f, Color.red, 1f);
        gameObject.SetActive(true);
    }

    void Update()
    {
        float traveled = Vector3.Distance(spawnPosition, transform.position);

        if (traveled >= maxTravelDistance || Time.time >= dieAt)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (((1 << col.gameObject.layer) & hitMask) == 0)
            return;

        Vector3 hitPoint = col.ClosestPoint(transform.position);
        Vector3 hitNormal = (transform.position - hitPoint).normalized;
        if (hitNormal.sqrMagnitude < 0.0001f)
            hitNormal = -transform.forward;

        IDamageable dmg =
            col.GetComponentInParent<IDamageable>() ??
            col.GetComponent<IDamageable>() ??
            col.transform.root.GetComponentInChildren<IDamageable>();

        if (dmg != null && damage > 0)
        {
            dmg.TakeDamage(damage, hitPoint, hitNormal);
        }

        if (impactPrefab)
        {
            GameObject fx = Instantiate(impactPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
            Destroy(fx, 2f);
        }
        Destroy(gameObject);
    }
}