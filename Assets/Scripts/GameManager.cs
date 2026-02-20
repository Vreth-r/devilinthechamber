using UnityEngine;

using FMODUnity;
using FMOD.Studio;
using Unity.Mathematics;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int enemiesKilled = 0;

    public bool bulletRestoreMod = false;

    public float enemyProjectileSpeedMod = 1f;
    public GameObject pauseMenu;
    public bool gamePaused;
    public PlayerControls controls;
    List<GameObject> cathedralEnemies = new List<GameObject>();

    public EventReference musicLoop;
    private EventInstance _musicInstance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        cathedralEnemies.AddRange(GameObject.FindGameObjectsWithTag("Cathedral Enemy"));
        controls = new PlayerControls();
        controls.Player.Enable();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (!musicLoop.IsNull)
        {
            _musicInstance = RuntimeManager.CreateInstance(musicLoop);
            _musicInstance.start();
        }
    }

    void Update()
    {
        if (controls.Player.Pause.WasPressedThisFrame())
        {
            if (pauseMenu != null)
                Instantiate(pauseMenu);
        }
    }

    public void enemyKilled (GameObject enemy)
    {
        if (bulletRestoreMod) PlayerManager.Instance.gunHitscan.currentMagazine += 1;
        enemiesKilled += 1;
        
        UnlistEnemy(enemy); 
        WinCheck();
    }

    public void UnlistEnemy(GameObject enemy)
    {
        if (cathedralEnemies.Contains(enemy)) cathedralEnemies.Remove(enemy);
    }

    public void WinCheck()
    {
        Debug.Log(cathedralEnemies.Count);
        if (cathedralEnemies.Count <= 0) Debug.Log("all cathedral enemies killed");//SceneFader.Instance.FadeToScene("MainMenu");
    }

    public void SetPauseBGM(bool paused)
    {
        if (!_musicInstance.isValid()) return;

        _musicInstance.setPaused(paused);
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
                PlayerManager.Instance.playerMotor.movementSpeedMod = StatModManager.GetStatModifier(stat);
                return;

            case StatName.DAMAGE_OUTPUT:
                PlayerManager.Instance.gunHitscan.damageMod = StatModManager.GetStatModifier(stat);

                return;

            case StatName.FIRE_SPEED:
                PlayerManager.Instance.gunHitscan.fireRate = StatModManager.GetStatModifier(stat);
                return;
            
            case StatName.RELOAD_SPEED:
                PlayerManager.Instance.gunHitscan.reloadSpeedMod = StatModManager.GetStatModifier(stat);
                return;

            case StatName.MAGAZINE_SIZE:
                PlayerManager.Instance.gunHitscan.magazineSizeMod = (int)StatModManager.GetStatModifier(stat);
                PlayerManager.Instance.gunHitscan.currentMagazine = math.min(PlayerManager.Instance.gunHitscan.currentMagazine + PlayerManager.Instance.gunHitscan.magazineSizeMod, PlayerManager.Instance.gunHitscan.magazineSize + PlayerManager.Instance.gunHitscan.magazineSizeMod);
                PlayerManager.Instance.gunHitscan.ForceUpdateMagazine();
                return;

            case StatName.PERMA_HEALTH:
                PlayerManager.Instance.health.maxHealthMod = (int)StatModManager.GetStatModifier(stat);
                PlayerManager.Instance.health.currentHealth += (int)StatModManager.GetStatModifier(stat);
                PlayerManager.Instance.health.ForceUpdateHealth();
                return;
                
            case StatName.JUMP_HEIGHT:
                PlayerManager.Instance.playerMotor.jumpHeightMod = StatModManager.GetStatModifier(stat);
                return;

            case StatName.SLIDE_DISTANCE:
                PlayerManager.Instance.playerMotor.slideDistMod = StatModManager.GetStatModifier(stat);
                return;

            case StatName.SLIDE_SPEED:
                PlayerManager.Instance.playerMotor.slideSpeedMod = StatModManager.GetStatModifier(stat);
                return;

            case StatName.SLIDE_COOLDOWN:
                PlayerManager.Instance.playerMotor.slideCooldownMod = StatModManager.GetStatModifier(stat);
                return;

            case StatName.HEADSHOT_BONUS:
                PlayerManager.Instance.gunHitscan.headShotDamageMod = StatModManager.GetStatModifier(stat);
                return;

            case StatName.BULLET_RANGE:
                PlayerManager.Instance.gunHitscan.rangeMod = StatModManager.GetStatModifier(stat);
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