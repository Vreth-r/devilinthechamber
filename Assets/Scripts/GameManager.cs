using UnityEngine;

using FMODUnity;
using FMOD.Studio;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int enemiesKilled = 0;
    public int numBullets = 0;
    public bool gamePaused;
    public PlayerControls controls;
    public InputDevice lastInputDevice;
    List<GameObject> cathedralEnemies = new List<GameObject>();

    public EventReference musicLoop;
    public bool checkForWin = true;
    private EventInstance _musicInstance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "DITC_level2.0")
        {
            cathedralEnemies.AddRange(GameObject.FindGameObjectsWithTag("Cathedral Enemy"));
        }
        controls = new PlayerControls();
        controls.Player.Enable();
        controls.UI.Disable();
        SetControlContextUpdate();
    }

    void Start()
    {
        if (!musicLoop.IsNull)
        {
            _musicInstance = RuntimeManager.CreateInstance(musicLoop);
            _musicInstance.start();
        }
        StatModManager.ResetStatMods();
        AbilityModManager.ResetAbilities();
        Time.timeScale = 1f;
        SetInputMap(true);
    }
    public void StopMusic()
    {
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _musicInstance.release();
    }

    public void enemyKilled (GameObject enemy)
    {
        enemiesKilled += 1;
        
        UnlistEnemy(enemy); 
        if (checkForWin) WinCheck();
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
            StopMusic();
            await SceneFader.Instance.FadeToScene("Credits-Animation");
        }
    }

    public void SetInputMap(bool gameplay)
    {
        if (controls == null) return;

        if (gameplay)
        {
            controls.UI.Disable();
            controls.Player.Enable();
        }
        else
        {
            controls.Player.Disable();
            controls.UI.Enable();
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


    void SetControlContextUpdate ()
    {
        controls.Player.Crouch.performed += ctx => lastInputDevice = ctx.control.device;
        controls.Player.DrainWP.performed += ctx => lastInputDevice = ctx.control.device;
        controls.Player.Fire.performed += ctx => lastInputDevice = ctx.control.device;
        controls.Player.Jump.performed += ctx => lastInputDevice = ctx.control.device;
        controls.Player.Look.performed += ctx => lastInputDevice = ctx.control.device;
        controls.Player.Reload.performed += ctx => lastInputDevice = ctx.control.device;
        controls.Player.Pause.performed += ctx => lastInputDevice = ctx.control.device;
        controls.Player.Move.performed += ctx => lastInputDevice = ctx.control.device;
        controls.UI.UIMove.performed += ctx => lastInputDevice = ctx.control.device;
        controls.UI.UISelect.performed += ctx => lastInputDevice = ctx.control.device;
    }
}