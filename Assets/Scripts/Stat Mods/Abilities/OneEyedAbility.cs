using UnityEngine;

public class OneEyedAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.ONE_EYED;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
