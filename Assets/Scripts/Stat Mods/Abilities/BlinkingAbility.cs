using UnityEngine;
using System.Collections;

public class BlinkingAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.BLINKING;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        float t = Random.Range(2, 5);
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), t, DoBlink);
        Debug.Log($"START: {abilityName}");
        return true;
    }
    bool DoBlink ()
    {
        UIEvents.DoBlink();
        float t = Random.Range(2, 5);
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), t, DoBlink);
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
