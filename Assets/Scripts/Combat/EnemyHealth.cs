using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 100;
    public bool destroyOnDeath = true;

    [Header("Hit Feedback")]
    public Renderer[] renderersToFlash;
    public float flashTime = 0.06f;
    public GameObject deathParticleFX;

    int hp;
    float flashTimer;

    void Awake()
    {
        hp = maxHealth;

        if (renderersToFlash == null || renderersToFlash.Length == 0)
            renderersToFlash = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f) SetFlash(false);
        }
    }

    public void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        hp -= amount;
        hp = Mathf.Max(hp, 0);

        if (!AbilityModManager.abilityFlags[AbilityName.NO_ENEMY_HIT_INDICATOR]) Flash();

        if (hp <= 0)
            Die();
    }

    public void Stun (float duration)
    {
        // help michael!
        Debug.Log($"Stunned for {duration}s");
    }

    void Flash()
    {
        flashTimer = flashTime;
        SetFlash(true);
    }

    void SetFlash(bool on)
    {
        foreach (var r in renderersToFlash)
        {
            if (!r) continue;

            foreach (var mat in r.materials)
            {
                if (!mat) continue;

                if (on)
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", Color.white * 2f);
                }
                else
                {
                    mat.SetColor("_EmissionColor", Color.black);
                }
            }
        }
    }

    void Die()
    {
        GameManager.Instance.enemyKilled(this.gameObject);

        if (deathParticleFX != null)
        {
            Debug.Log("explode");
            Instantiate(deathParticleFX, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }

        if (AbilityModManager.abilityFlags[AbilityName.KILL_BULLET_RESTORE])
            PlayerManager.Instance.gunHitscan.AddBulletToMagazine();

        if (destroyOnDeath)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}