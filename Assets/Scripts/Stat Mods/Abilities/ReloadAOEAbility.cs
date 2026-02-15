using UnityEngine;

public class ReloadAOEAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.AOE_RELOAD;
    }
    public override bool startFunction()
    {
        PlayerScriptRefHolder.Instance.gunHitscan.aoeOnReload = true;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerScriptRefHolder.Instance.gunHitscan.aoeOnReload = false;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
