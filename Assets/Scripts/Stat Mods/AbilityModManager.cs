using System.Collections.Generic;
using UnityEngine;


public enum AbilityName {
    INFINITE_MAG,
    INVINCIBILITY,
    FULL_AUTO,
    AOE_RELOAD,
    AOE_ON_DAMAGE,
    KILL_BULLET_RESTORE,
    BLINDNESS,
    BLINKING,
    EXPLODING_ENEMIES,
    CRAWLING_HEALTH_DEGEN,
    NO_HIT_INDICATOR,
    FAKE_HIT_INDICATOR,
    PHANTOM_NOISES,
    NO_SLIDING,
    NO_JUMPING,
    HALF_HEALTH,
    DEAFNESS 
    
}
public class AbilityModManager {

    // all abilities
    public static Dictionary<AbilityName, AbilityBase> abilities = new Dictionary<AbilityName, AbilityBase> ()
    {
        { AbilityName.INFINITE_MAG, new InfiniteMagAbility() },
        { AbilityName.INVINCIBILITY, new InvincibilityAbility() },
        { AbilityName.FULL_AUTO, new FullAutoAbility() },
        { AbilityName.AOE_RELOAD, new ReloadAOEAbility() },
        { AbilityName.KILL_BULLET_RESTORE, new BulletRestoreAbility() },
        { AbilityName.BLINDNESS, new BlindnessAbility() },
        { AbilityName.BLINKING, new BlinkingAbility() },
        { AbilityName.EXPLODING_ENEMIES, new ExplodingEnemiesAbility() },
        { AbilityName.NO_HIT_INDICATOR, new NoHitIndicatorAbility() },
        { AbilityName.FAKE_HIT_INDICATOR, new FakeHitIndicatorAbility() },
        { AbilityName.PHANTOM_NOISES, new PhantomNoisesAbility() },
        { AbilityName.NO_SLIDING, new NoSlideAbility() },
        { AbilityName.NO_JUMPING, new NoJumpAbility() },
        { AbilityName.HALF_HEALTH, new HalfHealthAbility() },
        { AbilityName.DEAFNESS, new DeafnessAbility() }
    };

    public static void StartAbility (AbilityName abilityName, float duration)
    {   
        abilities[abilityName].initialize(duration);
        abilities[abilityName].startFunction(); // run the start function (the effect)
        TimerHandler.Instance.CreateTimerHandle(abilities[abilityName].duration, abilities[abilityName].endFunction); // set timer to remove effect
    }
}
