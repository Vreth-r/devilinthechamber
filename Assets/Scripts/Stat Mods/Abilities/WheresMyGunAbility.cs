using UnityEngine;

public class WheresMyGunAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        UIEvents.SetReticleVisibility(false);
        UIEvents.ForceHUDRefresh();
        PlayerManager.Instance.gunHitscan.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }

    public override void endFunction()
    {
        base.endFunction();
        UIEvents.SetReticleVisibility(true);
        UIEvents.ForceHUDRefresh();
        PlayerManager.Instance.gunHitscan.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
    }
}
