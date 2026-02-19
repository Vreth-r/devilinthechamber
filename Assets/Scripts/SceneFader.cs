using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 0.8f;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Start fully transparent
        if (fadeImage != null)
        {
            var c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    /// <summary>
    /// Fades out, loads the scene, fades back in.
    /// </summary>
    public async Task FadeToScene(string sceneName)
    {
        await FadeOut();
        await SceneManager.LoadSceneAsync(sceneName);
        await FadeIn();
    }

    public async Task FadeOut()
    {
        if (fadeImage == null) return;

        float time = 0f;
        Color c = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadeImage.color = c;
            await Task.Yield();
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    public async Task FadeIn()
    {
        if (fadeImage == null) return;

        float time = 0f;
        Color c = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadeImage.color = c;
            await Task.Yield();
        }

        c.a = 0f;
        fadeImage.color = c;
    }
}

