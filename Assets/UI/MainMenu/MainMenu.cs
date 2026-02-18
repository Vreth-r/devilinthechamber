using GLTF.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour {
    UIDocument doc;
    Button playButton;
    Button exitButton;

    void Awake()
    {
        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;
        playButton = root.Q<Button>("Play");
        exitButton = root.Q<Button>("Exit");

        playButton.clicked += testFunc;
        exitButton.clicked += exitGame;
    }

    void testFunc ()
    {
        SceneManager.LoadScene("DITC_level1.0", LoadSceneMode.Single);
    }

    void exitGame()
    {
        Application.Quit();
    }


}
