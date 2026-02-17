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
        PlayerManager.Instance.health.maxHealth = math.min(PlayerManager.Instance.health.maxHealth / 2, 1);
        PlayerManager.Instance.health.currentHealth = math.min(PlayerManager.Instance.health.currentHealth / 2, 1);
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public bool FakeHit ()
    {
        UIEvents.IndicateHit();
        return true;
    }

    public override bool endFunction()
    {
        PlayerManager.Instance.health.maxHealth = PlayerManager.Instance.health.maxHealth * 2;
        PlayerManager.Instance.health.currentHealth = PlayerManager.Instance.health.currentHealth * 2;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}

