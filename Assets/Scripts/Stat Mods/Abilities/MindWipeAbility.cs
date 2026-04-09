using UnityEngine;

public class MindWipeAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        for (int i = 0; i < 3; i++)
            DeckManager.Instance.RemoveLastChosenDeal();

        UIEvents.ForceHUDRefresh();
    }
}
