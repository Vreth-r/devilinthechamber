using UnityEngine;

[CreateAssetMenu(menuName = "DrawConditions/FrogLegs")]
public class FrogLegsCondition : DrawCondition
{
    public override bool ConditionMet()
    {
        return DeckManager.Instance.pickedDeals.Contains("Hop"); // not optimal lmao
    }
}