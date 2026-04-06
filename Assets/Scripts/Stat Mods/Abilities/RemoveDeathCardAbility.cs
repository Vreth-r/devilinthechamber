using UnityEngine;

public class RemoveDeathCardAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        DeckManager.Instance.RemoveDeathCard();
    }
}
