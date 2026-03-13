using UnityEngine;

public class DoubleDamageAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.DOUBLE_DAMAGE_LOW_HP;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerManager.Instance.gunHitscan.ddLowHPMod = true;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
