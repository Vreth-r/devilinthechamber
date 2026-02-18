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
    public float rangeMod = 1f;
    public int damage = 25;
    public float headShotDamageBonus = 2f;
    public float fireRateMod = 1f; // stat mods
    public float damageMod = 1f; // stat mods
    public float headShotDamageMod = 1f; // stat mods

    public int magazineSize = 3;
    public int currentMagazine = 3;
    public int magazineSizeMod = 0; // stat mods

    public bool reloading = false;
    public float reloadSpeed = 1.25f;
    public float reloadSpeedMod = 1f; // stat mods
    public bool autoFireMod = false; // stat mods
    public bool aoeOnReload = false;
    public bool ddLowHPMod = false;


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
    void Start()
    {
        UIEvents.SetAmmo(currentMagazine, magazineSize);
    }

    void Update()
    {
        if (GameManager.Instance.gamePaused) return;
        
        if (controls.Player.Reload.IsPressed() && !reloading)
        {
            PlayerManager.Instance.health.Die();
            StartCoroutine(Reload());
        }
        if ((controls.Player.Fire.IsPressed() || autoFireMod) && Time.time >= nextFireTime && !reloading)
        {
            nextFireTime = Time.time + (1f / (fireRate * fireRateMod));
            Fire();
        }
    }

    public void ForceUpdateMagazine ()
    {
        UIEvents.SetAmmo(currentMagazine, magazineSize + magazineSizeMod);
    }

    void Fire()
    {
        if (reloading) return;
        if (!cam) cam = Camera.main;

        Ray aimRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;
        if (Physics.Raycast(aimRay, out RaycastHit aimHit, range * rangeMod, hitMask, QueryTriggerInteraction.Ignore))
            aimPoint = aimHit.point;
        else
            aimPoint = aimRay.origin + aimRay.direction * range * rangeMod;

        Vector3 origin = muzzle.position;
        Vector3 dir = (aimPoint - origin).normalized;

        Vector3 endPoint = origin + dir * range * rangeMod;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, range * rangeMod, hitMask, QueryTriggerInteraction.Ignore))
        {
            endPoint = hit.point;

            var dmg = hit.collider.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                if (ddLowHPMod && PlayerManager.Instance.health.currentHealth <= 2)
                    dmg.TakeDamage((int)(damage * damageMod * 2), hit.point, hit.normal);
                else
                    dmg.TakeDamage((int)(damage * damageMod), hit.point, hit.normal);
                
            }
        }

        if (muzzleFlash) muzzleFlash.Play();
        if (muzzleLight) StartCoroutine(FlashLight());
        AudioEvents.Play("Gunshot");
        animator.SetTrigger("Fire");
        animator.speed = fireRate * fireRateMod;

        if (tracerPrefab) SpawnTracer(origin, endPoint);

        currentMagazine -= 1;
        UIEvents.SetAmmo(currentMagazine, magazineSize);
        if (currentMagazine == 0) StartCoroutine(Reload());
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

    IEnumerator Reload()
    {
        reloading = true;
        animator.SetTrigger("Reload");
        animator.speed = reloadSpeed * reloadSpeedMod;
        yield return new WaitForSeconds(reloadSpeed / reloadSpeedMod);
        if (aoeOnReload)
        {
            RaycastHit hit;
            if (Physics.SphereCast(gameObject.transform.position, 10, Vector3.zero, out hit))
            {   
            }
            Debug.Log("boom!");
        }
        animator.speed = 1;
        currentMagazine = magazineSize + magazineSizeMod;
        UIEvents.SetAmmo(currentMagazine, magazineSize + magazineSizeMod);
        reloading = false;
    }
}