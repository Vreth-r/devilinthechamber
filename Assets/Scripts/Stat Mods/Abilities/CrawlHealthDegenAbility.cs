using UnityEngine;

public class CrawlingHealthDegenAbility : AbilityBase
{
    public override void startFunction()
    {
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), 9, DealOneDamage);
        base.startFunction();
    }
    void DealOneDamage ()
    {
        PlayerManager.Instance.health.TakeDamage(1, Vector3.zero, Vector3.zero);
        if (PlayerManager.Instance.health.currentHealth > 0)
            TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), 9, DealOneDamage);
    }
}
