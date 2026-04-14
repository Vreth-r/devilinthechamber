using UnityEngine;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{
    public List<GameObject> loadEnemiesContainer;
    public List<GameObject> unloadEnemiesContainer;
    public int WillPowerBump = 10;
    bool active = true;
    void OnTriggerEnter(Collider other)
    {
        if (!active) return;
        CheckpointData newCheckpointData = new CheckpointData(gameObject.transform.position, gameObject.transform.rotation);
        CheckpointManager.Instance.SetCheckPointData(newCheckpointData);
        active = false;

        if (loadEnemiesContainer != null)
        {
            foreach (GameObject go in loadEnemiesContainer)
            {
                foreach (Transform child in go.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        
        if (unloadEnemiesContainer != null)
        {
            foreach (GameObject go in unloadEnemiesContainer)
            {
                foreach (Transform child in go.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        PlayerManager.Instance.willpower.AddWillpower(WillPowerBump);
    }
}
