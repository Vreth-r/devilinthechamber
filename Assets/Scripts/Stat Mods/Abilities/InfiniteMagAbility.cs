using Unity.VisualScripting;
using UnityEngine;

public class InfiniteMagAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.INFINITE_MAG;
    }
    public override bool startFunction()
    {
        PlayerScriptRefHolder.Instance.gunHitscan.magazineSize = int.MaxValue;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerScriptRefHolder.Instance.gunHitscan.magazineSize = 10 + PlayerScriptRefHolder.Instance.gunHitscan.magazineSizeMod;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
