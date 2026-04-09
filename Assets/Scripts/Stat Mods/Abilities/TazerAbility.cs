
using System;

public class TazerAbility : AbilityBase
{
    public Action TazerEvent;
    public override void startFunction()
    {
        base.startFunction();
        TazerEvent += DoTazer;
    }

    public override void endFunction()
    {
        base.endFunction();
    }

    void DoTazer ()
    {
        ApplyTazerStatMods();
        TimerHandler.Instance.CreateTimerHandle("paranoia", 1, RemoveTazerStatMods);   
    }

    void ApplyTazerStatMods ()
    {
        StatModManager.AddStatModifier(StatName.DOG_MOVEMENT_SPEED, 0.75f);
        StatModManager.AddStatModifier(StatName.LADY_MOVEMENT_SPEED, 0.75f);
    }
    void RemoveTazerStatMods ()
    {
        StatModManager.RemoveStatModifierExact(StatName.DOG_MOVEMENT_SPEED, 0.75f);
        StatModManager.RemoveStatModifierExact(StatName.LADY_MOVEMENT_SPEED, 0.75f);
    }
}
