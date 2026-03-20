using System.Collections;
using UnityEngine;

public class DoorCeilingDown : Door
{
    public float duration = 0.5f;
    public float moveDistance = 3f;

    protected override IEnumerator PlayAnimation(bool forward)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos;

        if (forward)
            endPos = startPos + Vector3.down * moveDistance;
        else
            endPos = startPos + Vector3.up * moveDistance;

        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            yield return null;
        }
    }
}
    