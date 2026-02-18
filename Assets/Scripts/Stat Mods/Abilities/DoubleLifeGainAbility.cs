using Unity.VisualScripting;
using UnityEngine;

public class DoubleLifeGainAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.DOUBLE_LIFEGAIN_ABILITY;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerManager.Instance.health.doubleHeal = true;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
