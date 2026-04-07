using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    public EventReference uiClick;

    [Header("UIDocuments")]
    [SerializeField] UIDocument pauseDoc;
    [SerializeField] UIDocument hudDoc;

    [Header("Optional")]
    [SerializeField] PlayerLook playerLook;
    [SerializeField] string mainMenuSceneName = "MainMenu";

    [Header("DealMenu (for correct pausing behaviour)")]
    [SerializeField] DealMenu dealMenu;

    [Header("Pause BGM")]
    public EventReference musicLoop;
    private EventInstance _musicInstance;

    PlayerControls controls;

    VisualElement root;
    Button resumeButton;
    Button exitButton;

    public bool isPaused;
    float oldTimeScale;
    float toggleBlockUntil;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (pauseDoc == null)
            pauseDoc = GetComponent<UIDocument>();

        pauseDoc.enabled = true;

        root = pauseDoc.rootVisualElement;
        resumeButton = root.Q<Button>("Resume");
        exitButton   = root.Q<Button>("Exit");

        if (resumeButton == null) Debug.LogError("PauseMenu: Button name='Resume' not found.");
        if (exitButton == null)   Debug.LogError("PauseMenu: Button name='Exit' not found.");

        if (resumeButton != null) resumeButton.clicked += Resume;
        if (exitButton != null)   exitButton.clicked += ExitToMainMenu;

        SetVisible(false);
    }

    void OnEnable()
    {
        if(controls != null)
        {
            controls.Player.Pause.performed += OnPause;
        }
    }

    void Start()
    {
        controls = GameManager.Instance.controls;
        controls.Player.Pause.performed += OnPause;
    }

    public void PlayUIClick()
    {
        if (!uiClick.IsNull)
        {
            RuntimeManager.PlayOneShot(uiClick);
        }
    }

    void OnDisable()
    {
        if(controls != null)
        {
            controls.Player.Pause.performed -= OnPause;
        }
    }

    void OnPause(InputAction.CallbackContext _)
    {
        if (Time.unscaledTime < toggleBlockUntil) return;
        toggleBlockUntil = Time.unscaledTime + 0.15f;

        if (!dealMenu.dealMenuOpen) Toggle();
        if (!musicLoop.IsNull)
        {
            _musicInstance = RuntimeManager.CreateInstance(musicLoop);
            _musicInstance.start();
        }
    }

    public void Toggle()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        pauseDoc.sortingOrder = 3;
        GameManager.Instance.SetPauseBGM(true);

        if (hudDoc != null) hudDoc.rootVisualElement.style.display = DisplayStyle.None;
        SetVisible(true);

        GameManager.Instance.SetInputMap(false);

        //if (GameManager.Instance != null) GameManager.Instance.gamePaused = true;

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        if (playerLook != null) 
        {
            playerLook.allowCursorRelockOnClick = false;
        }
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;

        GameManager.Instance.SetPauseBGM(false);

        toggleBlockUntil = Time.unscaledTime + 0.15f;
        pauseDoc.sortingOrder = 0;
        SetVisible(false);
        if (hudDoc != null) hudDoc.rootVisualElement.style.display = DisplayStyle.Flex;

        Time.timeScale = oldTimeScale;
        if (GameManager.Instance != null) GameManager.Instance.gamePaused = false;
        GameManager.Instance.SetInputMap(true);

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        PlayUIClick();

        if (playerLook != null) 
        {
            playerLook.allowCursorRelockOnClick = true;
        }
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _musicInstance.release();
    }

    async void ExitToMainMenu()
    {
        PlayUIClick();
        Time.timeScale = 1f;
        if (GameManager.Instance != null) GameManager.Instance.gamePaused = false;

        GameManager.Instance.StopMusic();
        Destroy(GameManager.Instance.gameObject);
        await SceneFader.Instance.FadeToScene(mainMenuSceneName);
    }

    void SetVisible(bool visible)
    {
        root.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}