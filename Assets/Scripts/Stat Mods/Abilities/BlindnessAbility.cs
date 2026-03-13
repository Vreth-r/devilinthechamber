using UnityEngine;

public class BlindnessAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.BLINDNESS;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        UIEvents.SetIsBlind(true);
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        UIEvents.SetIsBlind(false);
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
