using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Cutscene_ender : MonoBehaviour
{
    VideoPlayer video;
    public string scene;
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += vidOver;
    }

    async void vidOver(UnityEngine.Video.VideoPlayer vp)
    {
        await SceneFader.Instance.FadeToScene(scene);
    }

}
