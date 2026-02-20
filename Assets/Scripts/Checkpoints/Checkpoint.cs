using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool active = true;
    void OnTriggerEnter(Collider other)
    {
        if (!active) return;
        CheckpointData newCheckpointData = new CheckpointData(gameObject.transform.position, gameObject.transform.rotation);
        CheckpointManager.Instance.SetCheckPointData(newCheckpointData);
        active = false;
    }
}
