using UnityEngine;

using FMODUnity;
using FMOD.Studio;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
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

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "DITC_level1.0")
        {
            cathedralEnemies.AddRange(GameObject.FindGameObjectsWithTag("Cathedral Enemy"));
        }
        controls = new PlayerControls();
        controls.Player.Enable();
        //DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (!musicLoop.IsNull)
        {
            _musicInstance = RuntimeManager.CreateInstance(musicLoop);
            _musicInstance.start();
        }
    }

    public void StopMusic()
    {
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _musicInstance.release();
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

    public async void WinCheck()
    {
        Debug.Log(cathedralEnemies.Count);
        if (cathedralEnemies.Count <= 0)
        {
            //await SceneFader.Instance.FadeToScene("Credits-Animation");
            Debug.Log("Penis Monkey");
        }
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

}