using System;
using UnityEngine;



public abstract class AbilityBase
{
    public AbilityName abilityName; // prob remove
    public float duration = 1f;
    public DealType dealType;
    public abstract void initialize(float duration); // prob remove, unless length is variable
    public abstract bool startFunction ();
    public abstract bool endFunction ();
}
