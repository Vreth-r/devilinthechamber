using UnityEngine;

public class KnockbackAbilty : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.KNOCKBACK_ABILITY;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerManager.Instance.health.takesKnockback = true;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
