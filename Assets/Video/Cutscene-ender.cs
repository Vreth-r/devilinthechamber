using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
public class Cutscene_ender : MonoBehaviour
{
    VideoPlayer video;
    public string scene;
    PlayerControls controls;
    void Start()
    {
        controls = new PlayerControls();
        controls.Player.Enable();
        controls.Player.Jump.performed += SkipTutorial;
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += VidOver;
    }

    private void SkipTutorial(InputAction.CallbackContext context)
    {
        VidOver(video);
    }

    async void VidOver(VideoPlayer vp)
    {
        controls.Player.Jump.performed -= SkipTutorial;
        await SceneFader.Instance.FadeToScene(scene);
    }

}
