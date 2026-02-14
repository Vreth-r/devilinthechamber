using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraPivot;
    public float standY = 1.6f;
    public float crouchY = 1.1f;
    public float lerpSpeed = 14f;

    public bool isCrouching;

    void LateUpdate()
    {
        if (!cameraPivot) return;
        float target = isCrouching ? crouchY : standY;

        Vector3 p = cameraPivot.localPosition;
        p.y = Mathf.Lerp(p.y, target, 1f - Mathf.Exp(-lerpSpeed * Time.deltaTime));
        cameraPivot.localPosition = p;
    }
}
