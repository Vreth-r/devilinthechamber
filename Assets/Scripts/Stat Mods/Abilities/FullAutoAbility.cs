using UnityEngine;

public class FullAutoAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.FULL_AUTO;
    }
    public override bool startFunction()
    {
        PlayerScriptRefHolder.Instance.gunHitscan.autoFireMod = true;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerScriptRefHolder.Instance.gunHitscan.autoFireMod = false;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
