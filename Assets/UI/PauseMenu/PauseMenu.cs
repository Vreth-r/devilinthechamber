using UnityEngine;
using UnityEngine.UIElements;
public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    UIDocument doc;

    Button resume;

    void Awake()
    {
        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        resume = root.Q<Button>();

        resume.clicked += Resume;
        Time.timeScale = 0;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        GameManager.Instance.gamePaused = true;
    }

    void Resume ()
    {
        Time.timeScale = 1;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        GameManager.Instance.gamePaused = false;
        Destroy(gameObject);
    }
}
