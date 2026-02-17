using Unity.VisualScripting;
using UnityEngine;

public class InfiniteMagAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.INFINITE_MAG;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerManager.Instance.gunHitscan.magazineSize = int.MaxValue;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerManager.Instance.gunHitscan.magazineSize = 10 + PlayerManager.Instance.gunHitscan.magazineSizeMod;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
