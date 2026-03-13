using System.Dynamic;
using UnityEngine;

public class PhantomNoisesAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.PHANTOM_NOISES;
        this.duration = duration;
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
