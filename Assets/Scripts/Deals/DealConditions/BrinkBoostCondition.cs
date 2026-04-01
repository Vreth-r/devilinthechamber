using UnityEngine;

[CreateAssetMenu(menuName = "DrawConditions/Brink Boost")]
public class BrinkBoostCondition : DrawCondition
{
    public override bool ConditionMet()
    {
        return DeckManager.Instance.pickedDeals.Contains("Backwards Logic");
    }
}