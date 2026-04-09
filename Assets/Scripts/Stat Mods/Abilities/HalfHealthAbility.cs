using Unity.Mathematics;
using UnityEngine;

public class HalfHealthAbility  : AbilityBase
{
    public override void startFunction()
    {
        PlayerManager.Instance.health.maxHealth = math.max(PlayerManager.Instance.health.maxHealth / 2, 1);
        PlayerManager.Instance.health.currentHealth = math.max(PlayerManager.Instance.health.currentHealth / 2, 1);
        PlayerManager.Instance.health.ForceUpdateHealth();
        base.startFunction();
    }

    public override void endFunction()
    {
        PlayerManager.Instance.health.maxHealth = PlayerManager.Instance.health.maxHealth * 2;
        PlayerManager.Instance.health.currentHealth = PlayerManager.Instance.health.currentHealth * 2;
        PlayerManager.Instance.health.ForceUpdateHealth();
        base.endFunction();
    }
}

