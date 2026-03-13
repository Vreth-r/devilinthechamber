using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Audio/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    public List<SoundEntry> sounds;
    private Dictionary<string, SoundEntry> lookup;

    public SoundEntry Get(string soundId)
    {
        if(lookup == null)
        {
            lookup = new Dictionary<string, SoundEntry>();
            foreach (var s in sounds)
            {
                lookup[s.soundId] = s;
            }
        }

        lookup.TryGetValue(soundId, out var entry);
        return entry;
    }

    void OnEnable()
    {
        if (sounds == null) return;
        var seen = new HashSet<string>();
        foreach (var s in sounds)
        {
            if (string.IsNullOrWhiteSpace(s.soundId)) continue;
            if (!seen.Add(s.soundId))
                Debug.LogWarning($"SoundLibrary: Duplicate soundId '{s.soundId}'", this);
        }
    }
}