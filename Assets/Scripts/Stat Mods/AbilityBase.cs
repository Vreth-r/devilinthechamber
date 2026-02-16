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
    PHANTOM_NOISES,
    NO_SLIDING,
    NO_JUMPING
}

public abstract class AbilityBase
{
    public AbilityName abilityName; // prob remove
    public float duration = 1f;
    public abstract void initialize(float duration); // prob remove, unless length is variable
    public abstract bool startFunction ();
    public abstract bool endFunction ();
}
