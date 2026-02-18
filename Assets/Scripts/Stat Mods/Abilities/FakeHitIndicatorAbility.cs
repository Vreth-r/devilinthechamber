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
        float t = 0;
        while (t < duration)
        {
            float k = Random.Range(0.5f, duration / 5);
            t += k;
            Debug.Log(t);
            TimerHandler.Instance.CreateTimerHandle(t, FakeHit);
        }
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public bool FakeHit ()
    {
        UIEvents.IndicateHit();
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}

