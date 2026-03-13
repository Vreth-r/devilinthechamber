using Unity.Mathematics;
using UnityEngine;

public class HalfHealthAbility  : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.HALF_HEALTH;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerManager.Instance.health.maxHealth = math.max(PlayerManager.Instance.health.maxHealth / 2, 1);
        PlayerManager.Instance.health.currentHealth = math.max(PlayerManager.Instance.health.currentHealth / 2, 1);
        PlayerManager.Instance.health.ForceUpdateHealth();
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerManager.Instance.health.maxHealth = PlayerManager.Instance.health.maxHealth * 2;
        PlayerManager.Instance.health.currentHealth = PlayerManager.Instance.health.currentHealth * 2;
        PlayerManager.Instance.health.ForceUpdateHealth();
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}

