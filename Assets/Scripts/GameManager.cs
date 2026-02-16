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

    // not good! idk where to put this lol
    public void SetStatMod(StatName stat)
    {
        switch (stat)
        {
            case StatName.DAMAGE_OUTPUT:
                enemyProjectileSpeedMod = StatModManager.Instance.GetStatProduct(stat);
                return;

            case StatName.ENEMY_PROJECTILE_SPEED:
                Debug.Log("enemy sat");
                return;

            case StatName.FIRE_SPEED:
                PlayerScriptRefHolder.Instance.gunHitscan.fireRate = StatModManager.Instance.GetStatProduct(stat);
                return;
                
            case StatName.HEADSHOT_BONUS:
                PlayerScriptRefHolder.Instance.gunHitscan.headShotDamageBonus = StatModManager.Instance.GetStatProduct(stat);
                return;

            case StatName.JUMP_HEIGHT:
                PlayerScriptRefHolder.Instance.playerMotor.jumpHeightMod = StatModManager.Instance.GetStatProduct(stat);
                return;

            case StatName.MAGAZINE_SIZE:
                PlayerScriptRefHolder.Instance.gunHitscan.magazineSize = (int)StatModManager.Instance.GetStatSum(stat);
                return;

            case StatName.MOVEMENT_SPEED:
                PlayerScriptRefHolder.Instance.playerMotor.movementSpeedMod = StatModManager.Instance.GetStatProduct(stat);
                return;

            case StatName.RELOAD_SPEED:
                PlayerScriptRefHolder.Instance.gunHitscan.reloadSpeedMod = StatModManager.Instance.GetStatProduct(stat);
                return;
            
            default:
                break;
        }
    }
}
