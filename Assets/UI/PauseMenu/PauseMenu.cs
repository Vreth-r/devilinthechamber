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

    PlayerControls controls;

    VisualElement root;
    Button resumeButton;
    Button exitButton;

    public bool isPaused;
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

        controls = new PlayerControls();

        SetVisible(false);
    }

    void OnEnable()
    {
        controls.Enable();
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
        //controls.Player.Pause.performed -= OnPause;
        //controls.Disable();
    }

    void OnPause(InputAction.CallbackContext _)
    {
        if (Time.unscaledTime < toggleBlockUntil) return;
        toggleBlockUntil = Time.unscaledTime + 0.15f;

        if (!dealMenu.dealMenuOpen) Toggle();
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
        Time.timeScale = 0f;

        GameManager.Instance.SetPauseBGM(true);

        if (hudDoc != null) hudDoc.rootVisualElement.style.display = DisplayStyle.None;
        SetVisible(true);

        //if (GameManager.Instance != null) GameManager.Instance.gamePaused = true;

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        if (playerLook != null) 
        {
            playerLook.enabled = false;
            playerLook.allowCursorRelockOnClick = false;
        }
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;

        GameManager.Instance.SetPauseBGM(false);

        toggleBlockUntil = Time.unscaledTime + 0.15f;

        SetVisible(false);
        if (hudDoc != null) hudDoc.rootVisualElement.style.display = DisplayStyle.Flex;

        Time.timeScale = 1f;
        if (GameManager.Instance != null) GameManager.Instance.gamePaused = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        PlayUIClick();

        if (playerLook != null) 
        {
            playerLook.enabled = true;
            playerLook.allowCursorRelockOnClick = true;
        }
    }

    void ExitToMainMenu()
    {
        PlayUIClick();
        Time.timeScale = 1f;
        if (GameManager.Instance != null) GameManager.Instance.gamePaused = false;

        Destroy(GameManager.Instance.gameObject);
        SceneFader.Instance.FadeToScene(mainMenuSceneName);
    }

    void SetVisible(bool visible)
    {
        root.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}