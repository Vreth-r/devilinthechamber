//using GLTF.Schema;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using FMODUnity;
using FMOD.Studio;
using System;

public class MainMenu : MonoBehaviour {
    UIDocument doc;
    Button playButton;
    Button exitButton;

    [Header("Events")]
    public EventReference uiClick;
    public EventReference musicLoop;

    private EventInstance _musicInstance;
    public PlayerControls controls;
    public InputDevice lastInputDevice;

    void Awake()
    {
        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;
        playButton = root.Q<Button>("Play");
        exitButton = root.Q<Button>("Exit");

        playButton.clicked += startGame;
        exitButton.clicked += exitGame;
    }

    void Start()
    {
        controls = new PlayerControls();
        controls.Player.Enable();
        controls.UI.Enable();

        controls.Player.Fire.performed += SetMouseShow;

        controls.UI.UIMove.performed += NavMenuController;
        controls.UI.UISelect.performed += SelectButtonMenuController;
        if (!musicLoop.IsNull)
        {
            _musicInstance = RuntimeManager.CreateInstance(musicLoop);
            _musicInstance.start();
        }
    }

    void OnDestroy()
    {
        if (_musicInstance.isValid())
        {
            _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _musicInstance.release();
        }
    }

    public void PlayUIClick()
    {
        if (!uiClick.IsNull)
        {
            RuntimeManager.PlayOneShot(uiClick);
        }
    }

    async void startGame ()
    {
        PlayUIClick();
        await SceneFader.Instance.FadeToScene("Intro-Animation");
    }

    void exitGame()
    {
        PlayUIClick();
        Application.Quit();
    }

    int buttonIndex = 0;
    private void NavMenuController(InputAction.CallbackContext context)
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        Vector2 move = context.ReadValue<Vector2>();

        if (move.y > 0 && buttonIndex == 1) 
        {
            exitButton.RemoveFromClassList("hover");
            playButton.AddToClassList("hover");
            buttonIndex = 0;
        }
        else if (move.y < 0 && buttonIndex == 0)
        {
            playButton.RemoveFromClassList("hover");
            exitButton.AddToClassList("hover");
            buttonIndex = 1;
        }
    }

    private void SelectButtonMenuController(InputAction.CallbackContext context)
    {
        if (buttonIndex == 0)
        {
            startGame();
            DeselectAll();
        }
        else if (buttonIndex == 1)
        {
            exitGame();
            DeselectAll();
        }
    }

    void DeselectAll()
    {
        playButton.RemoveFromClassList("hover");
        exitButton.RemoveFromClassList("hover");
        buttonIndex = 0;
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

    void SetMouseShow(InputAction.CallbackContext context)
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }
}
