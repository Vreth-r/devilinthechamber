using UnityEngine;

public class InfiniteMagAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.INFINITE_MAG; // cringe but whatever
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
