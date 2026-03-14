using System.Collections;
using UnityEngine;

public class DoorSwingLeft : Door
{
    public float duration = 0.5f;

    protected override IEnumerator PlayAnimation(bool forward)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot;

        if (forward)
            endRot = startRot * Quaternion.Euler(0, -90, 0);
        else
            endRot = startRot * Quaternion.Euler(0, 90, 0);

        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRot, endRot, time / duration);
            yield return null;
        }
    }
}
