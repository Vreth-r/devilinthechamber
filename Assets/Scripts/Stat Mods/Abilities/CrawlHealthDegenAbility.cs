using UnityEngine;

public class CrawlingHealthDegenAbility : AbilityBase
{
    public override void initialize (float duration)
    {
        abilityName = AbilityName.CRAWLING_HEALTH_DEGEN;
        this.duration = duration;
    }

    public override bool startFunction()
    {
        Debug.Log($"START: {abilityName}");
        return true;
    }

    public override bool endFunction()
    {
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
