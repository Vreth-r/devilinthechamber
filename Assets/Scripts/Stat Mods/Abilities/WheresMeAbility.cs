using UnityEngine;

public class WheresMeAbility : AbilityBase
{
    SkinnedMeshRenderer[] smr;
    MeshRenderer[] mr;
    public override void startFunction()
    {
        base.startFunction();
        UIEvents.ForceHUDRefresh();
        smr = PlayerManager.Instance.playerMotor.GetComponentsInChildren<SkinnedMeshRenderer>();
        mr = PlayerManager.Instance.playerMotor.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < smr.Length; i++) // what even is this brother
        {
            if (smr[i].gameObject.name != "Cube.002") 
                smr[i].enabled = false;
        }
        for (int i = 0; i < mr.Length; i++) mr[i].enabled = false;
    }

    public override void endFunction()
    {
        base.endFunction();
        UIEvents.ForceHUDRefresh();
        for (int i = 0; i < smr.Length; i++) smr[i].enabled = true;
        for (int i = 0; i < mr.Length; i++) mr[i].enabled = true;
    }
}
