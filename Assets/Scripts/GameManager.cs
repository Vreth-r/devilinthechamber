using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int enemiesKilled = 0;

    public bool bulletRestoreMod = false;

    public float enemyProjectileSpeedMod = 1f;
    public GameObject pauseMenu;
    public bool gamePaused;
    public PlayerControls controls;
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        controls = new PlayerControls();
        controls.Player.Enable();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (controls.Player.Pause.WasPressedThisFrame())
        {
            if (pauseMenu != null)
                Instantiate(pauseMenu);
        }
    }

    public void enemyKilled ()
    {
        if (bulletRestoreMod) PlayerManager.Instance.gunHitscan.currentMagazine += 1;
        enemiesKilled += 1;
    }

    public void FourNegativeDeals (StatName statName)
    {
        Debug.Log($"Too many negative deals {statName}");
    }

    // not good! idk where to put this lol
    // MOVEMENT_SPEED,
    // DAMAGE_OUTPUT,
    // FIRE_SPEED,
    // RELOAD_SPEED,
    // MAGAZINE_SIZE,
    // JUMP_HEIGHT,
    // SLIDE_DISTANCE,
    // HEADSHOT_BONUS,
    // LADY_PROJECTILE_SPEED,
    // DOG_RECOVERY_SPEED
    public void SetStatMod(StatName stat)
    {
        switch (stat)
        {
            case StatName.MOVEMENT_SPEED:
                PlayerManager.Instance.playerMotor.movementSpeedMod = 
                    StatModManager.GetPositiveStatModifier(stat) * StatModManager.GetNegativeStatModifier(stat);
                return;

            case StatName.DAMAGE_OUTPUT:
                PlayerManager.Instance.gunHitscan.damageMod = 
                    StatModManager.GetPositiveStatModifier(stat) * StatModManager.GetNegativeStatModifier(stat);
                return;

            case StatName.FIRE_SPEED:
                PlayerManager.Instance.gunHitscan.fireRate = 
                    StatModManager.GetPositiveStatModifier(stat) * StatModManager.GetNegativeStatModifier(stat);
                return;
            
            case StatName.RELOAD_SPEED:
                PlayerManager.Instance.gunHitscan.reloadSpeedMod = 
                    StatModManager.GetPositiveStatModifier(stat) * StatModManager.GetNegativeStatModifier(stat);
                return;

            case StatName.MAGAZINE_SIZE:
                PlayerManager.Instance.gunHitscan.magazineSize = (int)
                    (StatModManager.GetPositiveStatModifier(stat) * StatModManager.GetNegativeStatModifier(stat));
                return;
                

            case StatName.JUMP_HEIGHT:
                PlayerManager.Instance.playerMotor.jumpHeightMod = 
                    StatModManager.GetPositiveStatModifier(stat) * StatModManager.GetNegativeStatModifier(stat);
                return;

            case StatName.SLIDE_DISTANCE:
                PlayerManager.Instance.playerMotor.slideDistMod = 
                    StatModManager.GetPositiveStatModifier(stat) * StatModManager.GetNegativeStatModifier(stat);
                return;

            case StatName.HEADSHOT_BONUS:
                PlayerManager.Instance.gunHitscan.headShotDamageBonus =
                    StatModManager.GetPositiveStatModifier(stat) * StatModManager.GetNegativeStatModifier(stat);
                return;

            case StatName.BULLET_RANGE:
                PlayerManager.Instance.gunHitscan.rangeMod = 
                    StatModManager.GetPositiveStatModifier(stat) * StatModManager.GetNegativeStatModifier(stat);
                return;

            case StatName.LADY_MOVEMENT_SPEED:
                Debug.Log("enemy sat");
                return;

            case StatName.DOG_MOVEMENT_SPEED:
                Debug.Log("enemy sat");
                return;
            
            default:
                break;
        }
    }
}