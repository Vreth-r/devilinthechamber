using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

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
                checkpointData.checkpointPosition,
                checkpointData.checkpointRotation
            );
            //PlayerManager.Instance.playerLook.forceSetYaw(checkpointData.checkpointRotation.y);
            //PlayerManager.Instance.playerLook.g();

            controller.enabled = true;
        }

        // Restore timers

        foreach (TimerHandle timer in checkpointData.currentTimerHandles)
        {
            if (Enum.TryParse(timer.timerName, out AbilityName ability))
                AbilityModManager.StartAbility(ability, -1);
        }

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

        PlayerManager.Instance.gunHitscan.currentMagazine = checkpointData.currentMag;
        PlayerManager.Instance.health.currentHealth = PlayerManager.Instance.health.maxHealth;
        PlayerManager.Instance.willpower.currentWillpower = PlayerManager.Instance.willpower.maxWillpower;
        UIEvents.UpdateAmmo();
        PlayerManager.Instance.health.ForceUpdateHealth();
        UIEvents.ForceHUDRefresh();
    }

    public IEnumerator PauseLook ()
    {
        PlayerManager.Instance.playerLook.enabled = false;
        yield return new WaitForSeconds(0.2f);
        PlayerManager.Instance.playerLook.enabled = true;
    }

    
}
