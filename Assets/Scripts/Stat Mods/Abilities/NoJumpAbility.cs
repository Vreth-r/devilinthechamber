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
        PlayerManager.Instance.playerMotor.canJumpMod = false;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerManager.Instance.playerMotor.canJumpMod = true;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
