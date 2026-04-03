using UnityEngine;

public class ParanoiaAbility : AbilityBase
{
    public override void startFunction()
    {
        base.startFunction();
        UIEvents.IndicateHit += DoParanoia;
    }
    public override void endFunction()
    {
        base.endFunction();
        UIEvents.IndicateHit -= DoParanoia;
    }

    void DoParanoia ()
    {
        ApplyParanoiaStatMods();
        TimerHandler.Instance.CreateTimerHandle("paranoia", 1, RemoveParanoiaStatMods);   
    }

    void ApplyParanoiaStatMods ()
    {
        StatModManager.AddStatModifier(StatName.SLIDE_DISTANCE, 1.5f);
        StatModManager.AddStatModifier(StatName.JUMP_HEIGHT, 1.15f);
    }
    void RemoveParanoiaStatMods ()
    {
        StatModManager.RemoveStatModifierExact(StatName.SLIDE_DISTANCE, 1.5f);
        StatModManager.RemoveStatModifierExact(StatName.JUMP_HEIGHT, 1.15f);
    }
}
