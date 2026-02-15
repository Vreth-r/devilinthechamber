using System.Dynamic;
using UnityEngine;

public class PhantomNoisesAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.PHANTOM_NOISES;
        length = 10f;
    }
    public override bool startFunction()
    {
        AudioEvents.Play("PhantomNoise");
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        AudioEvents.Stop("PhantomNoise");
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
