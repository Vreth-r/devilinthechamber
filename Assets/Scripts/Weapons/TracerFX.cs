using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TracerFX : MonoBehaviour
{
    [Header("Timing")]
    public float extendTime = 0.02f;
    public float holdTime = 0.05f;
    public float fadeTime = 0.03f;

    [Header("Optional: Fade Width")]
    public bool fadeWidth = true;
    public float startWidth = 0.02f;

    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    public void Init(Vector3 start, Vector3 end)
    {
        StopAllCoroutines();
        StartCoroutine(Play(start, end));
    }

    System.Collections.IEnumerator Play(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, start);

        if (fadeWidth)
        {
            lr.startWidth = startWidth;
            lr.endWidth = startWidth;
        }

        float t = 0f;
        float dur = Mathf.Max(0.0001f, extendTime);

        while (t < 1f)
        {
            t += Time.deltaTime / dur;

            float e = EaseOutCubic(Mathf.Clamp01(t));
            lr.SetPosition(1, Vector3.LerpUnclamped(start, end, e));

            yield return null;
        }

        lr.SetPosition(1, end);

        if (holdTime > 0f)
            yield return new WaitForSeconds(holdTime);

        if (fadeTime > 0f)
        {
            float f = 0f;
            float w0 = lr.startWidth;

            while (f < 1f)
            {
                f += Time.deltaTime / fadeTime;
                float k = 1f - Mathf.Clamp01(f);

                if (fadeWidth)
                {
                    float w = w0 * k;
                    lr.startWidth = w;
                    lr.endWidth = w;
                }

                yield return null;
            }
        }

        Destroy(gameObject);
    }

    static float EaseOutCubic(float x) => 1f - Mathf.Pow(1f - x, 3f);
}