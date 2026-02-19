using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Library")]
    public SoundLibrary soundLibrary;

    [Header("Mixer")]
    public AudioMixer mixer;
    public AudioMixerGroup masterGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup uiGroup;
    public AudioMixerGroup gameplayGroup;

    [Header("Volume")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float uiVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float gameplayVolume = 1f;

    private readonly List<AudioSource> sfxSources = new();
    private AudioSource musicSource;

    const string PREF_MASTER = "vol_master";
    const string PREF_UI = "vol_ui";
    const string PREF_MUSIC = "vol_music";
    const string PREF_GAMEPLAY = "vol_gameplay";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved volumes
        masterVolume   = PlayerPrefs.GetFloat(PREF_MASTER, masterVolume);
        uiVolume       = PlayerPrefs.GetFloat(PREF_UI, uiVolume);
        musicVolume    = PlayerPrefs.GetFloat(PREF_MUSIC, musicVolume);
        gameplayVolume = PlayerPrefs.GetFloat(PREF_GAMEPLAY, gameplayVolume);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.outputAudioMixerGroup = musicGroup != null ? musicGroup : masterGroup;

        ApplyMixerVolumes();
    }

    void OnEnable()
    {
        AudioEvents.OnPlaySound += HandlePlaySound;
        AudioEvents.OnStopSound += HandleStopSound;
        AudioEvents.OnPlaySoundAt += HandlePlaySoundAt;
        //AudioEvents.OnPlayMusic += HandlePlayMusic;
    }

    void OnDisable()
    {
        AudioEvents.OnPlaySound -= HandlePlaySound;
        AudioEvents.OnStopSound -= HandleStopSound;
        AudioEvents.OnPlaySoundAt -= HandlePlaySoundAt;
        //AudioEvents.OnPlayMusic -= HandlePlayMusic;
    }

    // Public API for UI sliders
    public void SetMasterVolume(float v)   { masterVolume = Mathf.Clamp01(v);   SaveAndApply(); }
    public void SetMusicVolume(float v)    { musicVolume = Mathf.Clamp01(v);    SaveAndApply(); }
    public void SetUIVolume(float v)       { uiVolume = Mathf.Clamp01(v);       SaveAndApply(); }
    public void SetGameplayVolume(float v) { gameplayVolume = Mathf.Clamp01(v); SaveAndApply(); }

    void SaveAndApply()
    {
        PlayerPrefs.SetFloat(PREF_MASTER, masterVolume);
        PlayerPrefs.SetFloat(PREF_UI, uiVolume);
        PlayerPrefs.SetFloat(PREF_MUSIC, musicVolume);
        PlayerPrefs.SetFloat(PREF_GAMEPLAY, gameplayVolume);
        PlayerPrefs.Save();
        ApplyMixerVolumes();
    }

    void ApplyMixerVolumes()
    {
        if (mixer == null) return;

        mixer.SetFloat("MasterVol",   LinearToDb(masterVolume));
        mixer.SetFloat("MusicVol",    LinearToDb(musicVolume));
        mixer.SetFloat("UIVol",       LinearToDb(uiVolume));
        mixer.SetFloat("GameplayVol", LinearToDb(gameplayVolume));
    }

    void OnValidate()
    {
        // Clamp just in case, and push values to mixer
        masterVolume = Mathf.Clamp01(masterVolume);
        musicVolume = Mathf.Clamp01(musicVolume);
        uiVolume = Mathf.Clamp01(uiVolume);
        gameplayVolume = Mathf.Clamp01(gameplayVolume);

        ApplyMixerVolumes();
    }

    void Update()
    {
        ApplyMixerVolumes();
    }

    static float LinearToDb(float v)
    {
        // -80 dB is effectively silent
        if (v <= 0.0001f) return -80f;
        return Mathf.Log10(v) * 20f;
    }

    // Event Handlers
    void HandlePlaySound(string id)
    {
        var src = GetSFXSource();
        var sound = soundLibrary.Get(id);

        src.outputAudioMixerGroup =
            sound.category == SoundCategory.UI
                ? (uiGroup != null ? uiGroup : masterGroup)
                : (gameplayGroup != null ? gameplayGroup : masterGroup);

        if (sound == null)
        {
            Debug.LogWarning($"AudioManager: Sound '{id}' not found.");
            return;
        }

        switch (sound.category)
        {
            case SoundCategory.Music: PlayMusic(sound); break;
            case SoundCategory.UI:
            case SoundCategory.Gameplay: PlaySFX(sound); break;
        }
    }

    void HandleStopSound(string id)
    {
        var sound = soundLibrary.Get(id);
        if (sound == null) return;

        if (sound.category == SoundCategory.Music)
        {
            if (musicSource.clip == sound.clip) musicSource.Stop();
        }
        else
        {
            foreach (var src in sfxSources)
            {
                if (src.isPlaying && src.clip == sound.clip) { src.Stop(); return; }
            }
        }
    }

    void HandlePlaySoundAt(string id, Vector3 pos)
    {
        var sound = soundLibrary.Get(id);
        if (sound == null) return;

        var src = GetSFXSource();   
        src.transform.position = pos;
        src.spatialBlend = 1f;     // 3D
        src.rolloffMode = AudioRolloffMode.Logarithmic;
        src.minDistance = 1.5f;
        src.maxDistance = 30f;

        src.outputAudioMixerGroup =
            sound.category == SoundCategory.UI
                ? (uiGroup != null ? uiGroup : masterGroup) // usually won’t use UI here
                : (gameplayGroup != null ? gameplayGroup : masterGroup);

        src.clip = sound.clip;
        src.volume = sound.volume;
        src.pitch = sound.pitch;
        src.loop = sound.loop;
        src.Play();
    }

    // Playback
    void PlayMusic(SoundEntry sound)
    {
        var src = GetSFXSource();

        src.outputAudioMixerGroup =
            sound.category == SoundCategory.UI
                ? (uiGroup != null ? uiGroup : masterGroup)
                : (gameplayGroup != null ? gameplayGroup : masterGroup);

        if (musicSource.clip == sound.clip && musicSource.isPlaying) return;

        musicSource.outputAudioMixerGroup = musicGroup != null ? musicGroup : masterGroup;
        musicSource.clip = sound.clip;
        musicSource.volume = sound.volume; // mixer handles master/music volume
        musicSource.pitch = sound.pitch;
        musicSource.loop = sound.loop;
        musicSource.Play();
    }

    void PlaySFX(SoundEntry sound)
    {
        var src = GetSFXSource();
        src.outputAudioMixerGroup =
            sound.category == SoundCategory.UI
                ? (uiGroup != null ? uiGroup : masterGroup)
                : (gameplayGroup != null ? gameplayGroup : masterGroup);

        src.clip = sound.clip;
        src.volume = sound.volume; // mixer handles master/category volume now
        src.pitch = sound.pitch;
        src.loop = sound.loop;
        src.spatialBlend = 0f;
        src.Play();
    }

    public void PlayOneShot(string id)
    {
        var sound = soundLibrary.Get(id);
        if (sound == null) return;

        var src = GetSFXSource();
        src.outputAudioMixerGroup =
            sound.category == SoundCategory.UI
                ? (uiGroup != null ? uiGroup : masterGroup)
                : (gameplayGroup != null ? gameplayGroup : masterGroup);

        src.pitch = sound.pitch;
        src.loop = false;
        src.spatialBlend = 0f;
        src.PlayOneShot(sound.clip, sound.volume);
    }

    // Pooling
    AudioSource GetSFXSource()
    {
        foreach (var src in sfxSources)
            if (!src.isPlaying) return src;

        var newSrc = gameObject.AddComponent<AudioSource>();
        newSrc.playOnAwake = false;
        newSrc.outputAudioMixerGroup = masterGroup;
        sfxSources.Add(newSrc);
        return newSrc;
    }

}
