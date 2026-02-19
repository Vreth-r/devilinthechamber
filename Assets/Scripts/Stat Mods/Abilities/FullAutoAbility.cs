using UnityEngine;

public class FullAutoAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.FULL_AUTO;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerManager.Instance.gunHitscan.autoFireMod = true;
        PlayerManager.Instance.gunHitscan.fireRateMod += 2;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerManager.Instance.gunHitscan.autoFireMod = false;
        PlayerManager.Instance.gunHitscan.fireRateMod -= 2;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
