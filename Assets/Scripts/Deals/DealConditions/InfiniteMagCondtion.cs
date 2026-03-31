using UnityEngine;

[CreateAssetMenu(menuName = "DrawConditions/InfiniteMag")]
public class InfiniteMagCondition : DrawCondition
{
    public override bool ConditionMet()
    {
        return PlayerManager.Instance.gunHitscan.doesReload;
    }
}