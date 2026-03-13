using System.Collections;
using System.Threading.Tasks;
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
        UIEvents.DeathAnimFinished += DiePart2;
    }
    void Start()
    {
        UIEvents.SetHealth(); // timing thing
        
    }

    public void ForceUpdateHealth ()
    {
        UIEvents.SetHealth();
    }

    public void Heal (int amount)
    {
        if (doubleHeal)
            currentHealth += amount * 2;
        else
            currentHealth += amount;

        currentHealth = math.min(currentHealth, maxHealth + maxHealthMod);
        UIEvents.SetHealth();
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

        UIEvents.SetHealth();
        UIEvents.Hit();
        sound.PlayPlayerDamage();
        if (currentHealth <= 0)
        {
            lives -= 1;
            Die();
        }
    }

    public void Stun (float f) {}

    public void Die()
    {
        UIEvents.DoDeathAnim();
        invincible = true;
    }
    async void DiePart2()
    {
        invincible = false;
        if (lives > 0)
        {
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