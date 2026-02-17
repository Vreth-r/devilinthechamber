using UnityEngine;

public class BlinkingAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.BLINDNESS;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        UIEvents.SetBlind(true);
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        UIEvents.SetBlind(false);
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
