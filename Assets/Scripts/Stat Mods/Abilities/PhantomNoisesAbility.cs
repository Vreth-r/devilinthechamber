using System.Dynamic;
using UnityEngine;

public class PhantomNoisesAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.PHANTOM_NOISES; // cringe but whatever
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
