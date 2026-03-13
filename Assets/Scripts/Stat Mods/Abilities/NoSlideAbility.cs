using UnityEngine;

public class NoSlideAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.NO_SLIDING;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        PlayerManager.Instance.playerMotor.canSlideMod = false;
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        PlayerManager.Instance.playerMotor.canSlideMod = true;
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
