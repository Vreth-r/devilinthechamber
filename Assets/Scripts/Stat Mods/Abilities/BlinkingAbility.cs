using UnityEngine;
using System.Collections;

public class BlinkingAbility : AbilityBase
{
    public override void startFunction()
    {
        float t = Random.Range(1, 4);
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), t, DoBlink);
        base.startFunction();
    }
    void DoBlink ()
    {
        UIEvents.DoBlink();
        float t = Random.Range(1, 4);
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), t, DoBlink);
    }
}
