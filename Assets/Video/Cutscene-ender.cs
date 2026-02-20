using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Cutscene_ender : MonoBehaviour
{
    VideoPlayer video;
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += vidOver;
    }

    void vidOver(UnityEngine.Video.VideoPlayer vp)
    {
        print ("Video Over");
        // SceneManager.LoadScene("Tutorial"); // uncomment when merged
    }

}
