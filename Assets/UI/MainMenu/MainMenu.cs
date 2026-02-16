using GLTF.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour {
    UIDocument doc;
    Button playButton;

    void Awake()
    {
        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;
        playButton = root.Q<Button>("Play");

        playButton.clicked += testFunc;
    }

    void testFunc ()
    {
        SceneManager.LoadScene("AbilitiesTestScene");
    }


}
