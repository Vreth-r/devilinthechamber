using System;
using UnityEngine;

public static class AudioEvents
{
    // Play event
    public static event Action<string> OnPlaySound;

    // stop event
    public static event Action<string> OnStopSound;

    // public static event Action<string> OnPlayMusic;

    // play 3d 
    public static event Action<string, Vector3> OnPlaySoundAt;

    public static void Play(string soundId)
    {
        OnPlaySound?.Invoke(soundId);
    }

    public static void Stop(string soundId)
    {
        OnStopSound?.Invoke(soundId);
    }

    public static void PlayAt(string soundId, Vector3 pos)
    {
        OnPlaySoundAt?.Invoke(soundId, pos);
    }

    // public static void PlayMusic(string soundId)
    // {
    //     OnPlaySoundAt?.Invoke(soundId);
    // }
}