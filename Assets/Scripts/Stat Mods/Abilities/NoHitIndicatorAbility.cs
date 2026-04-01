using UnityEngine;

public class NoHitIndicatorAbility : AbilityBase
{
    public override void startFunction()
    {
        UIEvents.SetShowHitIndicator(false);
        base.startFunction();
    }

    public override void endFunction()
    {
        UIEvents.SetShowHitIndicator(true);
        base.startFunction();
    }
}
