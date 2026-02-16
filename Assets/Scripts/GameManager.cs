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
        if (bulletRestoreMod) PlayerScriptRefHolder.Instance.gunHitscan.currentMagazine += 1;
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
                PlayerScriptRefHolder.Instance.playerMotor.movementSpeedMod = 
                    StatModManager.Instance.GetPositiveStatModifier(stat) * StatModManager.Instance.GetNegativeStatModifier(stat);
                return;

            case StatName.DAMAGE_OUTPUT:
                PlayerScriptRefHolder.Instance.gunHitscan.damageMod = 
                    StatModManager.Instance.GetPositiveStatModifier(stat) * StatModManager.Instance.GetNegativeStatModifier(stat);
                return;

            case StatName.FIRE_SPEED:
                PlayerScriptRefHolder.Instance.gunHitscan.fireRate = 
                    StatModManager.Instance.GetPositiveStatModifier(stat) * StatModManager.Instance.GetNegativeStatModifier(stat);
                return;
            
            case StatName.RELOAD_SPEED:
                PlayerScriptRefHolder.Instance.gunHitscan.reloadSpeedMod = 
                    StatModManager.Instance.GetPositiveStatModifier(stat) * StatModManager.Instance.GetNegativeStatModifier(stat);
                return;

            case StatName.MAGAZINE_SIZE:
                PlayerScriptRefHolder.Instance.gunHitscan.magazineSize = (int)
                    (StatModManager.Instance.GetPositiveStatModifier(stat) * StatModManager.Instance.GetNegativeStatModifier(stat));
                return;
                

            case StatName.JUMP_HEIGHT:
                PlayerScriptRefHolder.Instance.playerMotor.jumpHeightMod = 
                    StatModManager.Instance.GetPositiveStatModifier(stat) * StatModManager.Instance.GetNegativeStatModifier(stat);
                return;

            case StatName.SLIDE_DISTANCE:
                PlayerScriptRefHolder.Instance.playerMotor.slideDistMod = 
                    StatModManager.Instance.GetPositiveStatModifier(stat) * StatModManager.Instance.GetNegativeStatModifier(stat);
                return;

            case StatName.HEADSHOT_BONUS:
                PlayerScriptRefHolder.Instance.gunHitscan.headShotDamageBonus =
                    StatModManager.Instance.GetPositiveStatModifier(stat) * StatModManager.Instance.GetNegativeStatModifier(stat);
                return;
                
            case StatName.LADY_PROJECTILE_SPEED:
                Debug.Log("enemy sat");
                return;
            
            case StatName.DOG_RECOVERY_SPEED:
                Debug.Log("enemy sat");
                return;
            
            default:
                break;
        }
    }
}
