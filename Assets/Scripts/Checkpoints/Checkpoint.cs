using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        CheckpointData newCheckpointData = new CheckpointData(gameObject.transform);
        CheckpointManager.Instance.SetCheckPointData(newCheckpointData);
    }
}
