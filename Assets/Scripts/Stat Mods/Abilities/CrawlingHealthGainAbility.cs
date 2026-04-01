using UnityEngine;

public class CrawlingHealthGainAbility: AbilityBase
{
    public override void startFunction()
    {
        TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), 9, HealOneDamage);
        base.startFunction();
    }
    void HealOneDamage ()
    {
        PlayerManager.Instance.health.Heal(1);
    }
}

