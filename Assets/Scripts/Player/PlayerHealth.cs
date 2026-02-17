using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    public int currentHealth;

    public bool invincible = false;

    void Awake()
    {
        currentHealth = maxHealth;
        UIEvents.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (invincible) return;
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);

        UIEvents.SetHealth(currentHealth, maxHealth);
        UIEvents.IndicateHit();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died (placeholder)");
    }
}