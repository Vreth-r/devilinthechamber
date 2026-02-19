using GLTF.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using FMODUnity;
using FMOD.Studio;

public class MainMenu : MonoBehaviour {
    UIDocument doc;
    Button playButton;
    Button exitButton;

    [Header("Events")]
    public EventReference uiClick;
    public EventReference musicLoop;

    private EventInstance _musicInstance;

    void Awake()
    {
        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;
        playButton = root.Q<Button>("Play");
        exitButton = root.Q<Button>("Exit");

        playButton.clicked += testFunc;
        exitButton.clicked += exitGame;
    }

    void Start()
    {
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

    void testFunc ()
    {
        PlayUIClick();
        SceneManager.LoadScene("DITC_level1.0", LoadSceneMode.Single);
    }

    void exitGame()
    {
        PlayUIClick();
        Application.Quit();
    }


}
