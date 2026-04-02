using UnityEngine;

public class HideCrosshairAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        UIEvents.SetReticleVisibility(false);
    }
}
