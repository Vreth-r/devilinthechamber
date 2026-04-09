using UnityEngine;

public class OneEyedAbility : AbilityBase
{
    public override void startFunction()
    {
        UIEvents.SetOneEyed();
        Debug.Log($"START: {abilityName}");
    }

    public override void endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
    }
}
