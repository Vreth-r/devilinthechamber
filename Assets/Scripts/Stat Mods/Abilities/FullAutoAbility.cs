using UnityEngine;

public class FullAutoAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.FULL_AUTO; // cringe but whatever
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
