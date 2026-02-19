using UnityEngine;

public class DealsLocalization
{

    public string StatLocale(string stat)
    {
        switch(stat)
        {
            case "MOVEMENT_SPEED":
                return "% Movement Speed";
                
            case "DAMAGE_OUTPUT":
                return "% Damage Output";
                
            case "FIRE_SPEED":
                return "% Fire Rate";
                
            case "RELOAD_SPEED":
                return "% Reload Speed";
                
            case "MAGAZINE_SIZE":
                return " Magazine Size";
                
            case "PERMA_HEALTH":
                return " Permanent Health Bar Increase";
                
            case "JUMP_HEIGHT":
                return "% Jump Height";
                
            case "SLIDE_DISTANCE":
                return "% Slide Distance";
                
            case "SLIDE_SPEED":
                return "% Slide Speed";
                
            case "SLIDE_COOLDOWN":
                return "% Slide Cooldown";
                
            case "HEADSHOT_BONUS":
                return "% Headshot Damage";
                
            case "LADY_PROJECTILE_SPEED":
                return "% Lady Projectile Speed";
                
            case "DOG_RECOVERY_SPEED":
                return "% Dog Attack Recovery Speed";
                
            case "LADY_FIRE_RATE":
                return "% Lady Fire Rate";
                
            case "LADY_MOVEMENT_SPEED":
                return "% Lady Movement Speed";
                
            case "DOG_MOVEMENT_SPEED":
                return "% Dog Movement Speed";
                
            default:
                return "something wrong";
                
        }
    }

    public string AbilityLocale(string ability)
    {
        switch(ability)
        {
            case "INFINITE_MAG":
                return " Second Infinite Magazine";
                
            case "INVINCIBILITY":
                return " Second Invincibility";
                
            case "FULL_AUTO":
                return " Second Fully Automatic Gun";
                
            case "AOE_RELOAD":
                return "AOE On Reload";
                
            case "AOE_ON_DAMAGE":
                return "AOE On Health Bar Depletion";
                
            case "KILL_BULLET_RESTORE":
                return "Kills Restore A Bullet To Your Magazine";
                
            case "BLINDNESS":
                return "Blindness";
                
            case "BLINKING":
                return "Blinking";
                
            case "EXPLODING_ENEMIES":
                return "Exploding Enemies";
                
            case "CRAWLING_HEALTH_DEGEN":
                return "Crawling Health Degeneration";
                
            case "NO_HIT_INDICATOR":
                return "No Hit Indicator";
                
            case "FAKE_HIT_INDICATOR":
                return "Phantom Hit Indicator";
                
            case "PHANTOM_NOISES":
                return "Phantom Noises";
                
            case "NO_SLIDING":
                return "No Sliding";
                
            case "NO_JUMPING":
                return "No Jumping";
                
            case "HALF_HEALTH":
                return "Half Health";
                
            case "DEAFNESS":
                return "Deafness";
                
            case "DOUBLE_DAMAGE_LOW_HP":
                return "Dobule Damage When Below 3 Hit Points";
                
            case "KNOCKBACK_ABILITY":
                return "Enemies Knock You Back On Hits";
                
            case "DOUBLE_LIFEGAIN_ABILITY":
                return "Double Amount Of Restored Hit Points";
                
            case "ONE_EYED":
                return "One-Eyed";
                
            case "DEATH":
                return "Death";
                
            default:
                return "something wrong";
        }
    }
}