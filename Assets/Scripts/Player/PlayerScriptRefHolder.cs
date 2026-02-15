using UnityEngine;

public class PlayerScriptRefHolder : MonoBehaviour
{
    public static PlayerScriptRefHolder Instance;
    public PlayerMotor playerMotor;
    public PlayerLook playerLook;
    public FovKick fovKick;
    public CameraMovement cameraMovement;
    public GunHitscan gunHitscan;
    public Health health;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SetStatMod(StatName stat)
    {
        Debug.Log("Updating stat");
        switch (stat)
        {
            case StatName.DAMAGE_OUTPUT:
                gunHitscan.damageMod = StatModManager.Instance.GetStatProduct(stat);
                return;

            case StatName.ENEMY_PROJECTILE_SPEED:
                Debug.Log("enemy sat");
                return;

            case StatName.FIRE_SPEED:
                gunHitscan.fireRate = StatModManager.Instance.GetStatProduct(stat);
                return;
                
            case StatName.HEADSHOT_BONUS:
                gunHitscan.headShotDamageBonus = StatModManager.Instance.GetStatProduct(stat);
                return;

            case StatName.JUMP_HEIGHT:
                playerMotor.jumpHeightMod = StatModManager.Instance.GetStatProduct(stat);
                return;

            case StatName.MAGAZINE_SIZE:
                gunHitscan.magazineSize = (int)StatModManager.Instance.GetStatSum(stat);
                return;

            case StatName.MOVEMENT_SPEED:
                playerMotor.movementSpeedMod = StatModManager.Instance.GetStatProduct(stat);
                return;

            case StatName.RELOAD_SPEED:
                gunHitscan.reloadSpeedMod = StatModManager.Instance.GetStatProduct(stat);
                return;
            
            default:
                break;
        }
    }
}
