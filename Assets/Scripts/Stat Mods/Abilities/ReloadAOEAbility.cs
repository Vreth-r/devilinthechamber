using UnityEngine;

public class ReloadAOEAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.AOE_RELOAD; // cringe but whatever
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
