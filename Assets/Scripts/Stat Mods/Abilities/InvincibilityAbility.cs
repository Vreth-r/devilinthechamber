using UnityEngine;

public class InvincibilityAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.INVINCIBILITY;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerManager.Instance.health.invincible = true;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerManager.Instance.health.invincible = false;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
