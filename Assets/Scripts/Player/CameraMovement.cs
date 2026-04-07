using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraPivot;
    public float standY = 1.6f;
    public float crouchY = 1.1f;
    public float lerpSpeed = 14f;

    public bool isCrouching;

    [Header("Shake")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float shakeDecay = 10f;

    float shakeTime;
    Vector3 shakeOffset;

    void LateUpdate()
    {
        if (!cameraPivot) return;

        float target = isCrouching ? crouchY : standY;
        Vector3 p = cameraPivot.localPosition;
        p.y = Mathf.Lerp(p.y, target, 1f - Mathf.Exp(-lerpSpeed * Time.deltaTime));

        if (shakeTime > 0f)
        {
            shakeOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-0.7f, 0.7f),
                0f
            ) * shakeMagnitude;

            cameraPivot.localRotation *= Quaternion.Euler(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            );

            shakeTime -= Time.deltaTime * shakeDecay;
        }
        else
        {
            shakeOffset = Vector3.zero;

            Vector3 resetPos = cameraPivot.localPosition;
            resetPos.x = 0f;
            resetPos.z = 0f;
            cameraPivot.localPosition = resetPos;

            cameraPivot.localRotation = Quaternion.identity;
        }

        cameraPivot.localPosition = p + shakeOffset;
    }

    public void Shake(float intensity = 1f)
    {
        shakeTime = shakeDuration * intensity;
    }
}
