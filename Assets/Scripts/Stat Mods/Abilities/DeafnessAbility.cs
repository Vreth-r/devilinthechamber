using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class DeafnessAbility : AbilityBase
{
    public override void startFunction()
    {
        RuntimeManager.GetBus("bus:/").setVolume(0);
        base.startFunction();
    }
    public override void endFunction()
    {
        RuntimeManager.GetBus("bus:/").setVolume(1f);
        base.endFunction();
    }
}


