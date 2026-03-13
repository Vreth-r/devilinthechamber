using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class CutsceneManager : MonoBehaviour
{
    public EventReference musicLoop;
    private EventInstance _musicInstance;

    void Start()
    {
        if (!musicLoop.IsNull)
        {
            _musicInstance = RuntimeManager.CreateInstance(musicLoop);
            _musicInstance.start();
        }
    }
}