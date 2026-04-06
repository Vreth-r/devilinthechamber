using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using FMODUnity;
using FMOD.Studio;
using System;
using Unity.VisualScripting;

public class GunHitscan : MonoBehaviour
{
    public Transform muzzle;

    public Animator animator;
    public Camera cam;

    [Header("Sound")]
    public EventReference gunshot;
    public EventReference reload;
    public EventReference jam;

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

    public int consecutiveJams = 0;

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
            if (AbilityModManager.abilityFlags[AbilityName.IN_A_JAM] && UnityEngine.Random.value <= 0.2)
            {
                animator.SetTrigger("Jam");
                RuntimeManager.PlayOneShotAttached(jam, gameObject);
                consecutiveJams++;
            }
            else
                Fire();
        }
    }


    void Fire(float offset = 0)
    {
        if (reloading) return;
        if (!cam) cam = Camera.main;


        Ray aimRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;

        Vector3 right = Camera.main.transform.right;

        aimPoint = aimRay.origin + aimRay.direction * 
                    (AbilityModManager.abilityFlags[AbilityName.BULLET_RANGED] 
                    ? range * StatModManager.GetStatModifier(StatName.BULLET_RANGE) 
                    : 100000)
                + right * offset;

        Vector3 gunOrigin = muzzle.position;
        Vector3 camOrigin = cam.transform.position;
        Vector3 dir = (aimPoint - camOrigin).normalized;

        Vector3 endPoint = camOrigin + dir * (AbilityModManager.abilityFlags[AbilityName.BULLET_RANGED] ? range * StatModManager.GetStatModifier(StatName.BULLET_RANGE) : 100000); // Magic 2!

        RaycastHit[] hits = Physics.RaycastAll(camOrigin, dir, AbilityModManager.abilityFlags[AbilityName.BULLET_RANGED] ? range * StatModManager.GetStatModifier(StatName.BULLET_RANGE) : 100000, hitMask, QueryTriggerInteraction.Ignore);

        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var hit in hits)
        {
            var dmg = hit.collider.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                int finalDamage = (int)(damage * StatModManager.GetStatModifier(StatName.DAMAGE_OUTPUT));

                if (AbilityModManager.abilityFlags[AbilityName.DOUBLE_DAMAGE_OUTPUT_LOW_HP] &&
                    PlayerManager.Instance.health.currentHealth <= 0.15f *
                    (PlayerManager.Instance.health.maxHealth + StatModManager.GetStatModifier(StatName.PERMA_HEALTH)))
                    finalDamage *= 2;

                if (AbilityModManager.abilityFlags[AbilityName.DAMAGE_BONUS_LOW_HEALTH] &&
                    PlayerManager.Instance.health.currentHealth <= 0.15f *
                    (PlayerManager.Instance.health.maxHealth + StatModManager.GetStatModifier(StatName.PERMA_HEALTH)))
                    finalDamage = (int)(finalDamage * 1.15f);

                if (AbilityModManager.abilityFlags[AbilityName.DAMAGE_BONUS_HIGH_HEALTH] &&
                    PlayerManager.Instance.health.currentHealth >= 0.75f *
                    (PlayerManager.Instance.health.maxHealth + StatModManager.GetStatModifier(StatName.PERMA_HEALTH)))
                    finalDamage = (int)(finalDamage * 1.25f);

                if (UnityEngine.Random.value <= StatModManager.GetStatModifier(StatName.CRITICAL_HIT_CHANCE)) finalDamage *= 2;

                if (AbilityModManager.abilityFlags[AbilityName.NEAR_SIGHTED] && Vector3.Distance(hit.point, cam.transform.position) >= 8) finalDamage = (int)(finalDamage * 1.5);

                finalDamage = (int)(finalDamage * (1 + consecutiveJams * 0.2));
                consecutiveJams = 0;

                dmg.TakeDamage(finalDamage, hit.point, hit.normal);

                if (!AbilityModManager.abilityFlags[AbilityName.BULLET_PIERCE])
                {
                    endPoint = hit.point;
                    break;
                }
            }

            endPoint = hit.point;
        }

        if (muzzleFlash) muzzleFlash.Play();
        if (muzzleLight) StartCoroutine(FlashLight());
        RuntimeManager.PlayOneShotAttached(gunshot, gameObject);
        animator.SetTrigger("Fire");
        animator.speed = fireRate * StatModManager.GetStatModifier(StatName.FIRE_SPEED);

        if (tracerPrefab) SpawnTracer(gunOrigin, endPoint);

        if (AbilityModManager.abilityFlags[AbilityName.THREE_GUNS_IN_ONE] && offset == 0)
        {
            Fire(-5);
            Fire(5);
        }

        if (AbilityModManager.abilityFlags[AbilityName.INFINITE_MAG]) return;
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
        currentMagazine = magazineSize + (int)StatModManager.GetStatModifier(StatName.MAGAZINE_SIZE);
        UIEvents.SetAmmo();
        yield return new WaitForSeconds(reloadSpeed / StatModManager.GetStatModifier(StatName.RELOAD_SPEED));

        animator.speed = 1;
        
        reloading = false;
        PlayerManager.Instance.willpower.AddWillpower(2);
        if (AbilityModManager.abilityFlags[AbilityName.DAMAGE_ON_RELOAD])
        {
            PlayerManager.Instance.health.TakeDamage(5, Vector3.zero, Vector3.zero);
        }
    }

    public void AddBulletToMagazine ()
    {
        if (currentMagazine == magazineSize + (int)StatModManager.GetStatModifier(StatName.MAGAZINE_SIZE)) return;
        currentMagazine += 1;
    }
}