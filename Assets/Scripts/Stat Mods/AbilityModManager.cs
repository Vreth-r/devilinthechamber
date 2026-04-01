using System.Collections.Generic;
using UnityEngine;


public enum AbilityName {
    NONE,
    BALLOON_SHOES,
    BLINDNESS,
    BLINKING,
    BULLET_PIERCE,
    CRAWLING_HEALTH_DEGEN,
    CRAWLING_HEALTH_GAIN,
    DEAFNESS,
    DAMAGE_ON_RELOAD,
    DOUBLE_DAMAGE_OUTPUT_LOW_HP,
    DOUBLE_DAMAGE_TAKEN_LOW_HP,
    FULL_AUTO,
    HALF_HEALTH,
    HALF_DAMAGE_TAKEN_LOW_HP,
    JUMP_SPEED_BOOST,
    KILL_BULLET_RESTORE,
    KNOCKBACK_ABILITY,
    INFINITE_MAG,
    INVINCIBILITY,
    NEAR_SIGHTED,
    NO_AMMO_COUNTER,
    NO_BODY_DAMAGE,
    NO_CROSSHAIR,
    NO_HEALTH_BAR,
    NO_HIT_INDICATOR,
    NO_JUMPING,
    NO_PLAYER_MODEL,
    NO_SLIDING,
    NO_SLIDE_OR_JUMP_LOW_HP,
    ONE_EYED,
    PARANOIA,
    RELOAD,
    REMOVE_DEATH_CARD,
    SLOW_ENEMY_ON_HIT,
    DEATH,
    BULLET_RANGED,
}
public class AbilityModManager {

    // all abilities
    public static Dictionary<AbilityName, AbilityBase> abilities = new Dictionary<AbilityName, AbilityBase> ()
    {
        { AbilityName.INFINITE_MAG, new InfiniteMagAbility() },
        { AbilityName.INVINCIBILITY, new InvincibilityAbility() },
        { AbilityName.FULL_AUTO, new FullAutoAbility() },
        { AbilityName.KILL_BULLET_RESTORE, new BulletRestoreAbility() },
        { AbilityName.BLINDNESS, new BlindnessAbility() },
        { AbilityName.BLINKING, new BlinkingAbility() },
        { AbilityName.NO_HIT_INDICATOR, new NoHitIndicatorAbility() },
        { AbilityName.NO_SLIDING, new NoSlideAbility() },
        { AbilityName.NO_JUMPING, new NoJumpAbility() },
        { AbilityName.HALF_HEALTH, new HalfHealthAbility() },
        { AbilityName.DEAFNESS, new DeafnessAbility() },
        { AbilityName.ONE_EYED, new OneEyedAbility() },
        { AbilityName.CRAWLING_HEALTH_DEGEN, new CrawlingHealthDegenAbility()},
        { AbilityName.KNOCKBACK_ABILITY, new KnockbackAbilty()},
        { AbilityName.DEATH, new DeathAbility() },
        { AbilityName.BALLOON_SHOES, null },
        { AbilityName.BULLET_PIERCE, null },
        { AbilityName.CRAWLING_HEALTH_GAIN, null },
        { AbilityName.DAMAGE_ON_RELOAD, null },
        { AbilityName.HALF_DAMAGE_TAKEN_LOW_HP, null },
        { AbilityName.DOUBLE_DAMAGE_TAKEN_LOW_HP, null },
        { AbilityName.JUMP_SPEED_BOOST, null },
        { AbilityName.NEAR_SIGHTED, null },
        { AbilityName.NO_AMMO_COUNTER, null },
        { AbilityName.NO_BODY_DAMAGE, null },
        { AbilityName.NO_CROSSHAIR, null },
        { AbilityName.NO_HEALTH_BAR, null },
        { AbilityName.NO_PLAYER_MODEL, null },
        { AbilityName.NO_SLIDE_OR_JUMP_LOW_HP, null },
        { AbilityName.PARANOIA, null },
        { AbilityName.REMOVE_DEATH_CARD, null },
        { AbilityName.SLOW_ENEMY_ON_HIT, null },
        { AbilityName.DOUBLE_DAMAGE_OUTPUT_LOW_HP, null },
    };

    public static Dictionary<AbilityName, bool> abilityFlags = new Dictionary<AbilityName, bool> ()
    {
        { AbilityName.INFINITE_MAG, false },
        { AbilityName.INVINCIBILITY, false },
        { AbilityName.FULL_AUTO, false },
        { AbilityName.KILL_BULLET_RESTORE, false },
        { AbilityName.BLINDNESS, false },
        { AbilityName.BLINKING, false },
        { AbilityName.NO_HIT_INDICATOR, false },
        { AbilityName.NO_SLIDING, false },
        { AbilityName.NO_JUMPING, false },
        { AbilityName.HALF_HEALTH, false },
        { AbilityName.DEAFNESS, false },
        { AbilityName.ONE_EYED, false },
        { AbilityName.CRAWLING_HEALTH_DEGEN, false},
        { AbilityName.DEATH, false },
        { AbilityName.BALLOON_SHOES, false },
        { AbilityName.BULLET_PIERCE, false },
        { AbilityName.CRAWLING_HEALTH_GAIN, false },
        { AbilityName.DAMAGE_ON_RELOAD, false },
        { AbilityName.HALF_DAMAGE_TAKEN_LOW_HP, false },
        { AbilityName.DOUBLE_DAMAGE_TAKEN_LOW_HP, false },
        { AbilityName.JUMP_SPEED_BOOST, false },
        { AbilityName.NEAR_SIGHTED, false },
        { AbilityName.NO_AMMO_COUNTER, false },
        { AbilityName.NO_BODY_DAMAGE, false },
        { AbilityName.NO_CROSSHAIR, false },
        { AbilityName.NO_HEALTH_BAR, false },
        { AbilityName.NO_PLAYER_MODEL, false },
        { AbilityName.NO_SLIDE_OR_JUMP_LOW_HP, false },
        { AbilityName.PARANOIA, false },
        { AbilityName.REMOVE_DEATH_CARD, false },
        { AbilityName.SLOW_ENEMY_ON_HIT, false },
        { AbilityName.DOUBLE_DAMAGE_OUTPUT_LOW_HP, false },
        { AbilityName.RELOAD, false },
        { AbilityName.KNOCKBACK_ABILITY, false },
        { AbilityName.BULLET_RANGED, false },
    };

    public static void StartAbility (AbilityName abilityName, float duration)
    {   
        abilities[abilityName].initialize(abilityName, duration);
        abilities[abilityName].startFunction(); // run the start function (the effect)
        if (abilities[abilityName].duration != 0) 
            TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), abilities[abilityName].duration, abilities[abilityName].endFunction); // set timer to remove effect
    }
}
