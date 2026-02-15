using UnityEngine;

public class CrawlingHealthDegenAbility : AbilityBase
{
    public override void initialize ()
    {
        abilityName = AbilityName.CRAWLING_HEALTH_DEGEN;
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
