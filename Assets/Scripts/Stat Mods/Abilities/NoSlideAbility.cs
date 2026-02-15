using UnityEngine;

public class NoSlideAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.NO_SLIDING;
    }
    public override bool startFunction()
    {
        PlayerScriptRefHolder.Instance.playerMotor.canSlideMod = false;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerScriptRefHolder.Instance.playerMotor.canSlideMod = true;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
