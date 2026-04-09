using System.Collections;
using UnityEngine;

public class DoorFloorUp : Door
{
    public float duration = 0.5f;
    public float moveDistance = 4f;

    protected override IEnumerator PlayAnimation(bool forward)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos;

        if (forward)
            endPos = startPos + Vector3.up * moveDistance;
        else
            endPos = startPos + Vector3.down * moveDistance;

        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            yield return null;
        }
    }
}
