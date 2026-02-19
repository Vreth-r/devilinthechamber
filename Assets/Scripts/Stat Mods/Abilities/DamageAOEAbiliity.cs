using UnityEngine;

public class DamageAOEAbiliity : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.AOE_RELOAD;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerManager.Instance.health.AOEOnDamage = true;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
