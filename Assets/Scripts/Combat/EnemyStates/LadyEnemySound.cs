using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class LadyEnemySound : MonoBehaviour
{
[Header("FMOD")]
    public EventReference nunShootEvent;

    [Header("When to play")]
    public float minMoveSpeed = 0.15f;
    public float minInterval = 0.18f;
    public float maxInterval = 0.50f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayShootSound()
    {
        if (nunShootEvent.IsNull) return;
        RuntimeManager.PlayOneShotAttached(nunShootEvent, gameObject);
    }
}
