using UnityEngine;

public class BlindnessAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.BLINDNESS; // cringe but whatever
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
