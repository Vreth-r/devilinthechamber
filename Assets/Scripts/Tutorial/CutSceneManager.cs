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

    void OnDestroy()
    {
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _musicInstance.release();
    }
}