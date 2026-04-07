using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 25f;
    public float lifetime = 3f;
    public LayerMask hitMask;

    public GameObject impactPrefab;

    [Header("Sound")]
    public EventReference travelSound;
    private EventInstance travelInstance;

    private Vector3 direction;
    private Vector3 lastPosition;

    public void Launch(Vector3 position, Vector3 dir, float speedOverride = -1f)
    {
        transform.position = position;
        direction = dir.normalized;

        if (speedOverride > 0f)
            speed = speedOverride;

        lastPosition = position;

        if (!travelSound.IsNull)
        {
            travelInstance = RuntimeManager.CreateInstance(travelSound);
            RuntimeManager.AttachInstanceToGameObject(travelInstance, gameObject);
            travelInstance.start();
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        Vector3 newPosition = transform.position + direction * step;

        if (Physics.Raycast(lastPosition, direction, out RaycastHit hit, step, hitMask))
        {
            OnHit(hit);
            return;
        }

        transform.position = newPosition;
        lastPosition = newPosition;
    }

    void OnHit(RaycastHit hit)
    {
        IDamageable dmg = hit.collider.GetComponentInParent<IDamageable>();

        if (dmg != null)
        {
            dmg.TakeDamage(damage, hit.point, hit.normal);
        }

        if (impactPrefab)
        {
            Instantiate(impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }

        StopSound();
        Destroy(gameObject);
    }

    void StopSound()
    {
        if (travelInstance.isValid())
        {
            travelInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            travelInstance.release();
        }
    }

    void OnDestroy()
    {
        StopSound();
    }
}