using UnityEngine;

public class NoJumpAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.NO_JUMPING;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerScriptRefHolder.Instance.playerMotor.canJumpMod = false;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerScriptRefHolder.Instance.playerMotor.canJumpMod = true;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
