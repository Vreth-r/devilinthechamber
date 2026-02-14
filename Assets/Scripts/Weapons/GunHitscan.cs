using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunHitscan : MonoBehaviour
{
    public Transform muzzle;

    public Animator animator;
    public Camera cam;

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
        if (!cam) cam = Camera.main;

        Ray aimRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;
        if (Physics.Raycast(aimRay, out RaycastHit aimHit, range, hitMask, QueryTriggerInteraction.Ignore))
            aimPoint = aimHit.point;
        else
            aimPoint = aimRay.origin + aimRay.direction * range;

        Vector3 origin = muzzle.position;
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
        AudioEvents.Play("Gunshot");
        animator.SetTrigger("Fire");

        if (tracerPrefab) SpawnTracer(origin, endPoint);
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