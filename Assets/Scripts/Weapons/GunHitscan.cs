using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using FMODUnity;
using FMOD.Studio;
using System;

public class GunHitscan : MonoBehaviour
{
    public Transform muzzle;

    public Animator animator;
    public Camera cam;

    [Header("Sound")]
    public EventReference gunshot;
    public EventReference reload;

    [Header("Fire")]
    public float fireRate = 3f;
    public float range = 120f;
    public int damage = 25;
    public float headShotDamageBonus = 2f;

    public int magazineSize = 10;
    public int currentMagazine = 10;

    public bool reloading = false;
    public float reloadSpeed = 1.25f;


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
        UIEvents.SetAmmo();
    }

    void Update()
    {
        if (GameManager.Instance.gamePaused) return;
        
        if (AbilityModManager.abilityFlags[AbilityName.RELOAD] && controls.Player.Reload.IsPressed() && !reloading)
        {
            //PlayerManager.Instance.health.Die(true);
            StartCoroutine(Reload());
        }
        if ((controls.Player.Fire.IsPressed() || AbilityModManager.abilityFlags[AbilityName.FULL_AUTO]) && Time.time >= nextFireTime && !reloading)
        {
            nextFireTime = Time.time + (1f / (fireRate * StatModManager.GetStatModifier(StatName.FIRE_SPEED)));
            Fire();
        }
    }


    void Fire()
    {
        if (reloading) return;
        if (!cam) cam = Camera.main;

        Ray aimRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;

        aimPoint = aimRay.origin + aimRay.direction * (AbilityModManager.abilityFlags[AbilityName.BULLET_RANGED] ? range * StatModManager.GetStatModifier(StatName.BULLET_RANGE) : 100000); // Magic!

        Vector3 origin = muzzle.position;
        Vector3 dir = (aimPoint - origin).normalized;

        Vector3 endPoint = origin + dir * (AbilityModManager.abilityFlags[AbilityName.BULLET_RANGED] ? range * StatModManager.GetStatModifier(StatName.BULLET_RANGE) : 100000); // Magic 2!

        if (Physics.Raycast(origin, dir, out RaycastHit hit, Mathf.Infinity, hitMask, QueryTriggerInteraction.Ignore))
        {
            endPoint = hit.point;

            var dmg = hit.collider.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                if (AbilityModManager.abilityFlags[AbilityName.DOUBLE_DAMAGE_OUTPUT_LOW_HP] && PlayerManager.Instance.health.currentHealth <= 2)
                    dmg.TakeDamage((int)(damage * StatModManager.GetStatModifier(StatName.DAMAGE_OUTPUT) * 2), hit.point, hit.normal);
                else
                    dmg.TakeDamage((int)(damage * StatModManager.GetStatModifier(StatName.DAMAGE_OUTPUT)), hit.point, hit.normal);
                
            }
        }

        if (muzzleFlash) muzzleFlash.Play();
        if (muzzleLight) StartCoroutine(FlashLight());
        RuntimeManager.PlayOneShotAttached(gunshot, gameObject);
        animator.SetTrigger("Fire");
        animator.speed = fireRate * StatModManager.GetStatModifier(StatName.FIRE_SPEED);

        if (tracerPrefab) SpawnTracer(origin, endPoint);

        if (AbilityModManager.abilityFlags[AbilityName.RELOAD])
        {
            currentMagazine -= 1;
            UIEvents.SetAmmo();
            if (currentMagazine == 0) StartCoroutine(Reload());   
        }
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
        if (currentMagazine == magazineSize + (int)StatModManager.GetStatModifier(StatName.MAGAZINE_SIZE)) yield break;
        RuntimeManager.PlayOneShotAttached(reload, gameObject);
        reloading = true;
        animator.SetTrigger("Reload");
        animator.speed = reloadSpeed * StatModManager.GetStatModifier(StatName.RELOAD_SPEED);
        yield return new WaitForSeconds(reloadSpeed / StatModManager.GetStatModifier(StatName.RELOAD_SPEED));

        animator.speed = 1;
        currentMagazine = magazineSize + (int)StatModManager.GetStatModifier(StatName.MAGAZINE_SIZE);
        UIEvents.SetAmmo();
        reloading = false;
    }
}