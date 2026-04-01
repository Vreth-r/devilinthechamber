using System;
using UnityEngine;



public abstract class AbilityBase
{
    public AbilityName abilityName; // prob remove
    public float duration = 1f;
    public DealType dealType;
    public virtual void initialize(AbilityName abilityName, float duration)
    {
        this.abilityName = abilityName;
        this.duration = duration;
        AbilityModManager.abilityFlags[abilityName] = true;
    }
    public virtual void startFunction ()
    {
        Debug.Log($"START: {abilityName}");
    }
    public virtual void endFunction ()
    {
        Debug.Log($"STOP: {abilityName}");
    }
}
