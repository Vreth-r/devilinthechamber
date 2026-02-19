using UnityEngine;

public static class DealsLocalization
{

    public static string StatLocale(StatName stat)
    {
        switch(stat)
        {
            case StatName.MOVEMENT_SPEED:
                return "% Movement Speed";
                
            case StatName.DAMAGE_OUTPUT:
                return "% Damage Output";
                
            case StatName.FIRE_SPEED:
                return "% Fire Rate";
                
            case StatName.RELOAD_SPEED:
                return "% Reload Speed";
                
            case StatName.MAGAZINE_SIZE:
                return " Magazine Size";
                
            case StatName.PERMA_HEALTH:
                return " Permanent Health Bar Increase";
                
            case StatName.JUMP_HEIGHT:
                return "% Jump Height";
                
            case StatName.SLIDE_DISTANCE:
                return "% Slide Distance";
                
            case StatName.SLIDE_SPEED:
                return "% Slide Speed";

            case StatName.BULLET_RANGE:
                return "% Bullet Range";
                
            case StatName.SLIDE_COOLDOWN:
                return "% Slide Cooldown";
                
            case StatName.HEADSHOT_BONUS:
                return "% Headshot Damage";
                
            case StatName.LADY_PROJECTILE_SPEED:
                return "% Lady Projectile Speed";
                
            case StatName.DOG_RECOVERY_SPEED:
                return "% Dog Attack Recovery Speed";
                
            case StatName.LADY_FIRE_RATE:
                return "% Lady Fire Rate";
                
            case StatName.LADY_MOVEMENT_SPEED:
                return "% Lady Movement Speed";
                
            case StatName.DOG_MOVEMENT_SPEED:
                return "% Dog Movement Speed";
                
            default:
                return stat.ToString();
                
        }
    }

    public static string AbilityLocale(AbilityName ability)
    {
        switch(ability)
        {
            case AbilityName.INFINITE_MAG:
                return " Second Infinite Magazine";
                
            case AbilityName.INVINCIBILITY:
                return " Second Invincibility";
                
            case AbilityName.FULL_AUTO:
                return " Second Fully Automatic Gun";
                
            case AbilityName.AOE_RELOAD:
                return "AOE On Reload";
                
            case AbilityName.AOE_ON_DAMAGE:
                return "AOE On Health Bar Depletion";
                
            case AbilityName.KILL_BULLET_RESTORE:
                return "Kills Restore A Bullet To Your Magazine";
                
            case AbilityName.BLINDNESS:
                return "Sunglasses Indoors";
                
            case AbilityName.BLINKING:
                return "Blinking";
                
            case AbilityName.EXPLODING_ENEMIES:
                return "Exploding Enemies";
                
            case AbilityName.CRAWLING_HEALTH_DEGEN:
                return "Crawling Health Degeneration";
                
            case AbilityName.NO_HIT_INDICATOR:
                return "No Hit Indicator";
                
            case AbilityName.FAKE_HIT_INDICATOR:
                return "Phantom Hit Indicator";
                
            case AbilityName.PHANTOM_NOISES:
                return "Phantom Noises";
                
            case AbilityName.NO_SLIDING:
                return "No Sliding";
                
            case AbilityName.NO_JUMPING:
                return "No Jumping";
                
            case AbilityName.HALF_HEALTH:
                return "Half Health";
                
            case AbilityName.DEAFNESS:
                return "Deafness";
                
            case AbilityName.DOUBLE_DAMAGE_LOW_HP:
                return "Dobule Damage When Below 3 Hit Points";
                
            case AbilityName.KNOCKBACK_ABILITY:
                return "Enemies Knock You Back On Hits";
                
            case AbilityName.DOUBLE_LIFEGAIN_ABILITY:
                return "Double Amount Of Restored Hit Points";
                
            case AbilityName.ONE_EYED:
                return "One-Eyed";
                
            case AbilityName.DEATH:
                return "Death";
                
            default:
                return "something wrong";
        }
    }
}