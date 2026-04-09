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
    DEPRECIATED_NO_AMMO_COUNTER,
    ONLY_HEADSHOTS,
    NO_CROSSHAIR,
    NO_HEALTH_BAR,
    NO_PLAYER_HIT_INDICATOR,
    NO_JUMPING,
    WHERES_ME,
    NO_SLIDING,
    NO_SLIDE_OR_JUMP_LOW_HP,
    ONE_EYED,
    PARANOIA,
    RELOAD,
    REMOVE_DEATH_CARD,
    SLOW_ENEMY_ON_HIT,
    DEATH,
    BULLET_RANGED,
    HOP,
    BACKWARDS_LOGIC,
    ALL_CARDS_FLIPPED,
    NO_DAMAGE_CHANCE,
    ONE_CARD_ALWAYS_FLIPPED,
    ONE_CARD_NEVER_FLIPPED,
    DEPRECIATED_NO_GUN_MODEL,
    DAMAGE_BONUS_OUTSIDE_SIGHT_RANGE,
    MIND_WIPE,
    DAMAGE_BONUS_HIGH_HEALTH,
    INSATIABLE_GREED,
    TWO_CARDS_NEVER_FLIPPED,
    SURVIVOR,
    NO_ENEMY_HIT_INDICATOR,
    DAMAGE_BONUS_LOW_HEALTH,
    WHERES_GUN,
    THREE_GUNS_IN_ONE,
    FROG_LEGS,
    IN_A_JAM
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
        { AbilityName.NO_PLAYER_HIT_INDICATOR, null },
        { AbilityName.NO_SLIDING, new NoSlideAbility() },
        { AbilityName.NO_JUMPING, new NoJumpAbility() },
        { AbilityName.HALF_HEALTH, new HalfHealthAbility() },
        { AbilityName.DEAFNESS, new DeafnessAbility() },
        { AbilityName.ONE_EYED, new OneEyedAbility() },
        { AbilityName.CRAWLING_HEALTH_DEGEN, new CrawlingHealthDegenAbility()},
        { AbilityName.KNOCKBACK_ABILITY, new KnockbackAbilty()},
        { AbilityName.DEATH, new DeathAbility() },
        { AbilityName.CRAWLING_HEALTH_GAIN, new CrawlingHealthGainAbility() },
        { AbilityName.RELOAD, new ReloadAbility() },
        { AbilityName.NO_CROSSHAIR, new HideCrosshairAbility() },
        { AbilityName.HOP, new HopAbility() },
        { AbilityName.BACKWARDS_LOGIC, new BackwardsLogicAbility() },
        { AbilityName.MIND_WIPE, new MindWipeAbility() },
        { AbilityName.PARANOIA, new ParanoiaAbility() },
        { AbilityName.WHERES_GUN, new WheresMyGunAbility() },
        { AbilityName.WHERES_ME, new WheresMeAbility() },
        { AbilityName.SLOW_ENEMY_ON_HIT, new TazerAbility() },
        { AbilityName.NEAR_SIGHTED, new NearSigtedAbility() },
        { AbilityName.REMOVE_DEATH_CARD, new RemoveDeathCardAbility() },
    };

    public static Dictionary<AbilityName, bool> abilityFlags = new Dictionary<AbilityName, bool> ()
    {
        { AbilityName.INFINITE_MAG, false },
        { AbilityName.INVINCIBILITY, false },
        { AbilityName.FULL_AUTO, false },
        { AbilityName.KILL_BULLET_RESTORE, false },
        { AbilityName.BLINDNESS, false },
        { AbilityName.BLINKING, false },
        { AbilityName.NO_PLAYER_HIT_INDICATOR, false },
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
        { AbilityName.DEPRECIATED_NO_AMMO_COUNTER, false },
        { AbilityName.ONLY_HEADSHOTS, false },
        { AbilityName.NO_CROSSHAIR, false },
        { AbilityName.NO_HEALTH_BAR, false },
        { AbilityName.WHERES_ME, false },
        { AbilityName.NO_SLIDE_OR_JUMP_LOW_HP, false },
        { AbilityName.PARANOIA, false },
        { AbilityName.REMOVE_DEATH_CARD, false },
        { AbilityName.SLOW_ENEMY_ON_HIT, false },
        { AbilityName.DOUBLE_DAMAGE_OUTPUT_LOW_HP, false },
        { AbilityName.RELOAD, false },
        { AbilityName.KNOCKBACK_ABILITY, false },
        { AbilityName.BULLET_RANGED, false },
        { AbilityName.ALL_CARDS_FLIPPED, false },
        { AbilityName.NO_DAMAGE_CHANCE, false },
        { AbilityName.ONE_CARD_ALWAYS_FLIPPED, false },
        { AbilityName.ONE_CARD_NEVER_FLIPPED, false },
        { AbilityName.DAMAGE_BONUS_OUTSIDE_SIGHT_RANGE, false },
        { AbilityName.MIND_WIPE, false },
        { AbilityName.DAMAGE_BONUS_HIGH_HEALTH, false },
        { AbilityName.INSATIABLE_GREED, false },
        { AbilityName.TWO_CARDS_NEVER_FLIPPED, false },
        { AbilityName.SURVIVOR, false },
        { AbilityName.NO_ENEMY_HIT_INDICATOR, false },
        { AbilityName.DAMAGE_BONUS_LOW_HEALTH, false },
        { AbilityName.WHERES_GUN, false },
        { AbilityName.THREE_GUNS_IN_ONE, false },
        { AbilityName.FROG_LEGS, false },
        { AbilityName.IN_A_JAM, false },
    };

    public static void StartAbility (AbilityName abilityName, float duration)
    {   
        if (abilities.ContainsKey(abilityName) && abilities[abilityName] != null)
        {
            abilities[abilityName].initialize(abilityName, duration);
            abilities[abilityName].startFunction(); // run the start function (the effect)
            if (abilities[abilityName].duration != 0) 
                TimerHandler.Instance.CreateTimerHandle(abilityName.ToString(), abilities[abilityName].duration, abilities[abilityName].endFunction); // set timer to remove effect
        }
    }

    public static void ResetAbilities()
    {
        var keys = new List<AbilityName>(abilityFlags.Keys);

        foreach (var key in keys)
        {
            bool isActive = abilityFlags[key];

            if (isActive && abilities.ContainsKey(key))
            {
                abilities[key].endFunction();
            }

            abilityFlags[key] = false;
        }
    }
}
