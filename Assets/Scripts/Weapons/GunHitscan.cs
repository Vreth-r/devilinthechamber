using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunHitscan : MonoBehaviour
{
    public Transform muzzle;

    [Header("Fire")]
    public float fireRate = 3f;
    public float range = 120f;
    public int damage = 25;

    [Header("FX")]
    public ParticleSystem muzzleFlash;
    public Light muzzleLight;
    public GameObject tracerPrefab;
    public float tracerDuration = 0.05f;

    public LayerMask hitMask = ~0;

    PlayerControls controls;
    float nextFireTime;

    void Awake() => controls = new PlayerControls();
    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    void Update()
    {
        if (controls.Player.Fire.IsPressed() && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate);
            Fire();
        }
    }

    void Fire()
    {
        Vector3 origin = muzzle.position;
        Vector3 dir = muzzle.forward;

        Vector3 hitPoint = origin + dir * range;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            hitPoint = hit.point;

            var health = hit.collider.GetComponentInParent<Health>();
            if (health) health.TakeDamage(damage);
        }

        if (muzzleFlash) muzzleFlash.Play();
        AudioEvents.Play("Gunshot");
        //recoil
        //transform.localRotation *= Quaternion.Euler(-2f, Random.Range(-0.5f, 0.5f), 0f);

        if (muzzleLight) StartCoroutine(FlashLight());

        if (tracerPrefab) SpawnTracer(origin, hitPoint);
    }

    IEnumerator FlashLight()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(0.03f);
        muzzleLight.enabled = false;
    }

    void SpawnTracer(Vector3 start, Vector3 end)
    {
        GameObject tracer = Instantiate(tracerPrefab);
        var fx = tracer.GetComponent<TracerFX>();
        if (fx != null)
        {
            fx.Init(start, end);
        }
        else
        {
            var lr = tracer.GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.positionCount = 2;
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
            }
            Destroy(tracer, 0.08f);
        }
    }
}