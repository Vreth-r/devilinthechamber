using Unity.Mathematics;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    public int maxHealthMod = 0;
    public int currentHealth;

    public bool takesKnockback = false;
    private float knockbackForce = 10f;
    private Vector3 knockbackVelocity;
    private float knockbackDuration = 0.2f;
    private float knockbackTimer;

    public bool invincible = false;
    public bool doubleHeal = false;

    void Awake()
    {
        currentHealth = maxHealth;
        UIEvents.SetHealth(currentHealth, maxHealth);
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

        currentHealth = math.min(currentHealth, maxHealth);
        UIEvents.UpdateHealth(currentHealth, maxHealth);
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
        if (invincible) return;
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);

        if (takesKnockback)
        {
            Vector3 knockbackDirection = hitNormal.normalized;
            knockbackVelocity = knockbackDirection * knockbackForce;
            knockbackTimer = knockbackDuration;
        }

        UIEvents.SetHealth(currentHealth, maxHealth + maxHealthMod);
        UIEvents.IndicateHit();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("died");
        CheckpointManager.Instance.RespawnPlayer(gameObject);
    }
}