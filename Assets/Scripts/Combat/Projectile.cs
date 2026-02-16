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

    [Header("Lifetime")]
    public float lifetime = 5f;

    [Header("Impact FX (optional)")]
    public GameObject impactPrefab;

    Rigidbody rb;
    float dieAt;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Launch(Vector3 position, Vector3 direction, float speedOverride = -1f)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);

        float spd = speedOverride > 0f ? speedOverride : speed;
        rb.useGravity = useGravity;
        rb.linearVelocity = direction.normalized * spd;

        dieAt = Time.time + lifetime;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (Time.time >= dieAt)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        var col = collision.collider;

        Vector3 hitPoint = collision.GetContact(0).point;
        Vector3 hitNormal = collision.GetContact(0).normal;

        var dmg =
            col.GetComponentInParent<IDamageable>() ??
            col.GetComponent<IDamageable>() ??
            col.transform.root.GetComponentInChildren<IDamageable>();

        if (dmg != null && damage > 0)
            dmg.TakeDamage(damage, hitPoint, hitNormal);

        if (impactPrefab)
        {
            var fx = Instantiate(impactPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
            Destroy(fx, 2f);
        }

        Destroy(gameObject);
    }
}