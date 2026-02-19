using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(Collider))]
public class FmodSnapshotZone : MonoBehaviour
{
    public EventReference snapshotEvent;
    public int priority = 0;

    private EventInstance _snapshotInstance;

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void StartSnapshot()
    {
        if (snapshotEvent.IsNull) return;
        if (!_snapshotInstance.isValid())
            _snapshotInstance = RuntimeManager.CreateInstance(snapshotEvent);

        _snapshotInstance.start();
    }

    public void StopSnapshot()
    {
        if (_snapshotInstance.isValid())
        {
            _snapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _snapshotInstance.release();
            _snapshotInstance.clearHandle();
        }
    }
}
