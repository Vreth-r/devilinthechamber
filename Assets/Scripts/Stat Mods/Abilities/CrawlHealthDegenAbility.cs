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
        TimerHandler.Instance.CreateTimerHandle(9, DealOneDamage);
        Debug.Log($"START: {abilityName}");
        return true;
    }
    bool DealOneDamage ()
    {
        PlayerManager.Instance.health.TakeDamage(1, Vector3.zero, Vector3.zero);
        return true;
    }

    public override bool endFunction()
    {
        if (PlayerManager.Instance.health.currentHealth > 0)
            TimerHandler.Instance.CreateTimerHandle(9, DealOneDamage);
        Debug.Log($"STOP: {abilityName}");
        return true;
    }
}
