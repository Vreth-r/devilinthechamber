using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    CheckpointData checkpointData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetCheckPointData(CheckpointData newCheckpointData)
    {
        checkpointData = newCheckpointData;
    }

    public void RespawnPlayer(GameObject player)
    {
        if (checkpointData == null) return;

        // Teleport
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            player.transform.SetPositionAndRotation(
                checkpointData.checkpointTransform.position,
                checkpointData.checkpointTransform.rotation
            );
            controller.enabled = true;
        }

        // Restore timers
        TimerHandler.Instance.SetTimerHandles(
            new List<TimerHandle>(checkpointData.currentTimerHandles)
        );

        // Restore stat modifiers
        StatModManager.StatModifiers =
            new Dictionary<StatName, List<float>>();

        foreach (var entry in checkpointData.currentStatModifiers)
        {
            StatModManager.StatModifiers.Add(
                entry.Key,
                new List<float>(entry.Value)
            );
        }

    }
}
