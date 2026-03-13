using UnityEngine;

public class NoHitIndicatorAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.NO_HIT_INDICATOR;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        UIEvents.SetShowHitIndicator(false);
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        UIEvents.SetShowHitIndicator(true);
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
