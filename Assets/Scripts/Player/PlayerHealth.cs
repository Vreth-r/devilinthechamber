using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("References")]
    public PlayerSound sound;
    public int maxHealth = 15;
    public int maxHealthMod = 0;
    public int currentHealth;
    public int lives = 10;

    public bool takesKnockback = false;
    private float knockbackForce = 10f;
    private Vector3 knockbackVelocity;
    private float knockbackDuration = 0.2f;
    private float knockbackTimer;

    public bool invincible = false;
    public bool doubleHeal = false;
    public bool AOEOnDamage = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
        UIEvents.SetHealth(currentHealth, maxHealth); // timing thing
        
    }

    public void ForceUpdateHealth ()
    {
        UIEvents.SetHealth(currentHealth, maxHealth + maxHealthMod);
    }

    public void Heal (int amount)
    {
        if (doubleHeal)
            currentHealth += amount * 2;
        else
            currentHealth += amount;

        currentHealth = math.min(currentHealth, maxHealth + maxHealthMod);
        UIEvents.UpdateHealth(currentHealth, maxHealth + maxHealthMod);
    }

    void Update()
    {
        if (takesKnockback && knockbackTimer > 0)
        {
            PlayerManager.Instance.controller.Move(knockbackVelocity * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitNormal)
    {

        if (takesKnockback)
        {
            Vector3 knockbackDirection = hitNormal.normalized;
            knockbackVelocity = -knockbackDirection * knockbackForce;
            knockbackTimer = knockbackDuration;
        }
        
        if (invincible) return;
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);

        if (AOEOnDamage)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 100, 7);

            foreach (Collider enemy in enemies)
            {
                Debug.Log(enemy.gameObject.name);
                if (enemy.gameObject.TryGetComponent(out IDamageable damageable))
                    damageable.Stun(2f); // stun for 2 seconds
            }
        }

        UIEvents.SetHealth(currentHealth, maxHealth + maxHealthMod);
        UIEvents.IndicateHit();
        sound.PlayPlayerDamage();
        if (currentHealth <= 0)
        {
            Die(true);
        }
    }

    public void Stun (float f) {}

    public async void Die(bool respawn)
    {
        Debug.Log("died");
        if (respawn)
        {
            lives -= 1;
            CheckpointManager.Instance.RespawnPlayer(gameObject);
            DealMenu.Instance.OpenMenu();
        }
        else
        {
            StatModManager.ResetStatMods();
            GameManager.Instance.StopMusic();
            Destroy(GameManager.Instance.gameObject);
            await SceneFader.Instance.FadeToScene("GameOver");
        }
    }
}