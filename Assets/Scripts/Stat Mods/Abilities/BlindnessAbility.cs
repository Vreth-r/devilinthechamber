using UnityEngine;

public class BlindnessAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        UIEvents.SetIsBlind(true);
    }

    public override void endFunction()
    {
        base.endFunction();
        UIEvents.SetIsBlind(false);
    }
}
