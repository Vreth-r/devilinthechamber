using System.Collections;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("References")]
    public PlayerSound sound;
    public int maxHealth = 100;
    public int currentHealth;
    public int deaths = 0;

    private float knockbackForce = 10f;
    private Vector3 knockbackVelocity;
    private float knockbackDuration = 0.2f;
    private float knockbackTimer;

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
        currentHealth += amount;

        currentHealth = math.min(currentHealth, maxHealth + (int)StatModManager.GetStatModifier(StatName.PERMA_HEALTH));
        UIEvents.SetHealth();
    }

    void Update()
    {
        if (AbilityModManager.abilityFlags[AbilityName.KNOCKBACK_ABILITY] && knockbackTimer > 0)
        {
            PlayerManager.Instance.controller.Move(knockbackVelocity * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (AbilityModManager.abilityFlags[AbilityName.NO_DAMAGE_CHANCE] && UnityEngine.Random.value <= 0.1) return; 
        if (AbilityModManager.abilityFlags[AbilityName.KNOCKBACK_ABILITY])
        {
            Vector3 knockbackDirection = hitNormal.normalized;
            knockbackVelocity = -knockbackDirection * knockbackForce;
            knockbackTimer = knockbackDuration;
        }
        
        if (AbilityModManager.abilityFlags[AbilityName.INVINCIBILITY]) return;
        int dmg = AbilityModManager.abilityFlags[AbilityName.DOUBLE_DAMAGE_TAKEN_LOW_HP] && currentHealth / (maxHealth + (int)StatModManager.GetStatModifier(StatName.PERMA_HEALTH)) <= 0.15 ? 2 * amount : amount;
        currentHealth -= AbilityModManager.abilityFlags[AbilityName.HALF_DAMAGE_TAKEN_LOW_HP] && currentHealth / (maxHealth + (int)StatModManager.GetStatModifier(StatName.PERMA_HEALTH)) <= 0.15 ? (int)(0.5f * dmg) : dmg;
        currentHealth = Mathf.Max(0, currentHealth);
        Debug.Log(currentHealth);
        UIEvents.SetHealth();

        if (!AbilityModManager.abilityFlags[AbilityName.NO_PLAYER_HIT_INDICATOR])
        {
            UIEvents.Hit();
        }
        sound.PlayPlayerDamage();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        deaths += 1;
        DeckManager.Instance.AddDeathCard(1);
        UIEvents.DoDeathAnim();
        AbilityModManager.abilityFlags[AbilityName.KNOCKBACK_ABILITY] = true;
        currentHealth = maxHealth + (int)StatModManager.GetStatModifier(StatName.PERMA_HEALTH);
    }
    void DiePart2()
    {
        AbilityModManager.abilityFlags[AbilityName.KNOCKBACK_ABILITY] = false;

        CheckpointManager.Instance.RespawnPlayer(gameObject);
        DealMenu.Instance.OpenMenu();
        
    }
}