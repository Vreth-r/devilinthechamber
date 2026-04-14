using UnityEngine;

public class NearSigtedAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        ApplyFog();
        ApplyFog();
    }

    void ApplyFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogEndDistance = 15;
    }

    public override void endFunction()
    {
        base.endFunction();
    }
}
