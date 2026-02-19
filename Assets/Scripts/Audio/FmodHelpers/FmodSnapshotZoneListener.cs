using UnityEngine;
using System.Collections.Generic;

public class FmodSnapshotZoneListener : MonoBehaviour
{
    private readonly HashSet<FmodSnapshotZone> _zones = new();
    private FmodSnapshotZone _current;

    void OnTriggerEnter(Collider other)
    {
        var z = other.GetComponent<FmodSnapshotZone>();
        if (z == null) return;
        _zones.Add(z);
        Refresh();
    }

    void OnTriggerExit(Collider other)
    {
        var z = other.GetComponent<FmodSnapshotZone>();
        if (z == null) return;
        _zones.Remove(z);
        Refresh();
    }

    void Refresh()
    {
        FmodSnapshotZone best = null;
        foreach (var z in _zones)
        {
            if (z == null) continue;
            if (best == null || z.priority > best.priority) best = z;
        }

        if (best == _current) return;

        if (_current != null) _current.StopSnapshot();
        _current = best;
        if (_current != null) _current.StartSnapshot();
    }
}
