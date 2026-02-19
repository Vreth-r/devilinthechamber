using UnityEngine;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;

public class CheckpointData
{
    public Transform checkpointTransform { get; private set; }
    public Dictionary<StatName, List<float>> currentStatModifiers = new Dictionary<StatName, List<float>>();
    public List<TimerHandle> currentTimerHandles = new List<TimerHandle>();

    public int currentMag;
    public int currentHealth;

    public CheckpointData(Transform checkpointTransform)
    {
        this.checkpointTransform = checkpointTransform;
        var currentTimers = TimerHandler.Instance.GetTimerHandles();

        // Snapshot stats
        currentStatModifiers = new Dictionary<StatName, List<float>>();

        foreach (var entry in StatModManager.StatModifiers)
        {
            currentStatModifiers.Add(entry.Key, new List<float>(entry.Value));
        }


        currentTimerHandles = new List<TimerHandle>();
        foreach (var timer in currentTimers)
        {
            currentTimerHandles.Add(new TimerHandle(timer));
        }

        currentMag = PlayerManager.Instance.gunHitscan.currentMagazine;
        currentHealth = PlayerManager.Instance.health.currentHealth;

    }
}
