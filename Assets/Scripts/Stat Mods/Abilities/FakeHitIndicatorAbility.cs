using UnityEngine;

public class FakeHitIndicatorAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.FAKE_HIT_INDICATOR;
        this.duration = duration;
    }
    public override bool startFunction()
    {
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), Random.Range(1, 3), FakeHit);

        Debug.Log($"START: {abilityName}");
        return true;
    }

    public bool FakeHit ()
    {
        UIEvents.Hit();
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), Random.Range(1, 3), FakeHit);
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}

