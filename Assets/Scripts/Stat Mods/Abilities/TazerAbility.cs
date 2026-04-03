
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

    }
    void RemoveTazerStatMods ()
    {

    }
}
