using UnityEngine;

public class DealsLocalization : MonoBehaviour
{
    public string StatLocale(string stat)
    {
        switch(stat)
        {
            case MOVEMENT_SPEED:
                return "% Movement Speed";
                break;
            case DAMAGE_OUTPUT:
                return "% Damage Output";
                break;
            case FIRE_SPEED:
                return "% Fire Rate";
                break;
            case RELOAD_SPEED:
                return "% Reload Speed";
                break;
            case MAGAZINE_SIZE:
                return " Magazine Size";
                break;
            case PERMA_HEALTH:
                return " Permanent Health Bar Increase";
                break;
            case JUMP_HEIGHT:
                return "% Jump Height";
                break;
            case SLIDE_DISTANCE:
                return "% Slide Distance";
                break;
            case SLIDE_SPEED:
                return "% Slide Speed";
                break;
            case SLIDE_COOLDOWN:
                return "% Slide Cooldown";
                break;
            case HEADSHOT_BONUS:
                return "% Headshot Damage";
                break;
            case LADY_PROJECTILE_SPEED:
                return "% Lady Projectile Speed";
                break;
            case DOG_RECOVERY_SPEED:
                return "% Dog Attack Recovery Speed";
                break;
            case LADY_FIRE_RATE:
                return "% Lady Fire Rate";
                break;
            case LADY_MOVEMENT_SPEED:
                return "% Lady Movement Speed";
                break;
            case DOG_MOVEMENT_SPEED:
                return "% Dog Movement Speed";
                break;
            case default:
                return "Something is wrong";
                break;
        }
    }

    public string AbilityLocale(string ability)
    {
        switch(ability)
        {
            case INFINITE_MAG:
                return " Second Infinite Magazine";
                break;
            case INVINCIBILITY:
                return " Second Invincibility";
                break;
            case FULL_AUTO:
                return " Second Fully Automatic Gun";
                break;
            case AOE_RELOAD:
                return "AOE On Reload";
                break;
            case AOE_ON_DAMAGE:
                return "AOE On Health Bar Depletion";
                break;
            case KILL_BULLET_RESTORE:
                return "Kills Restore A Bullet To Your Magazine";
                break;
            case BLINDNESS:
                return "Blindness";
                break;
            case BLINKING:
                return "Blinking";
                break;
            case EXPLODING_ENEMIES:
                return "Exploding Enemies";
                break;
            case CRAWLING_HEALTH_DEGEN:
                return "Crawling Health Degeneration";
                break;
            case NO_HIT_INDICATOR:
                return "No Hit Indicator";
                break;
            case FAKE_HIT_INDICATOR:
                return "Phantom Hit Indicator";
                break;
            case PHANTOM_NOISES:
                return "Phantom Noises";
                break;
            case NO_SLIDING:
                return "No Sliding";
                break;
            case NO_JUMPING:
                return "No Jumping";
                break;
            case HALF_HEALTH:
                return "Half Health";
                break;
            case DEAFNESS:
                return "Deafness";
                break;
            case DOUBLE_DAMAGE_LOW_HP:
                return "Dobule Damage When Below 3 Hit Points";
                break;
            case KNOCKBACK_ABILITY:
                return "Enemies Knock You Back On Hits";
                break;
            case DOUBLE_LIFEGAIN_ABILITY:
                return "Double Amount Of Restored Hit Points";
                break;
            case ONE_EYED:
                return "One-Eyed";
                break;
            case DEATH:
                return "Death";
                break;
            case default:
                return "Something is wrong";
                break;
        }

    }
}