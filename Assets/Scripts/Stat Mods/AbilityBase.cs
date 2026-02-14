using System;
using UnityEngine;

public enum AbilityName {
    INFINITE_MAG,
    INVINCIBILITY,
    FULL_AUTO,
    AOE_RELOAD,
    KILL_BULLET_RESTORE,
    BLINDNESS,
    EXPLODING_ENEMIES,
    CRAWLING_HEALTH_DEGEN,
    PHANTOM_NOISES
}

public abstract class AbilityBase
{
    public AbilityName abilityName; // prob remove
    public float length = 1f;
    public abstract void initialize(); // prob remove, unless length is variable
    public abstract bool startFunction ();
    public abstract bool endFunction ();
}
