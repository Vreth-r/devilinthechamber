using UnityEngine;

public class GunLag : MonoBehaviour
{
    [Header("References")]
    public Transform cameraPivot;
    public PlayerMotor motor;
    [Header("Base Pose")]
    public Vector3 baseLocalPos;
    public Vector3 baseLocalEuler;

    [Header("Movement Drag")]
    public float maxPosOffset = 0.10f;
    public float posLag = 0.06f;
    public float maxRotOffset = 8f;
    public float rotLag = 0.07f;

    [Tooltip("How much the weapon shifts opposite to movement in camera space (X=strafe, Y=up, Z=forward).")]
    public Vector3 posResponse = new Vector3(1.0f, 0.0f, 0.8f);

    [Tooltip("How much the weapon rotates due to movement (pitch,yaw,roll).")]
    public Vector3 rotResponse = new Vector3(0.0f, 0.0f, 1.0f);

    [Header("Bob")]
    public bool enableBob = false;
    public float bobAmount = 0.02f;
    public float bobSpeed = 10f;

    Vector3 posVel;
    Vector3 rotVel;

    void Reset()
    {
        baseLocalPos = transform.localPosition;
        baseLocalEuler = transform.localEulerAngles;
    }

    void Awake()
    {
        if (baseLocalPos == Vector3.zero && baseLocalEuler == Vector3.zero)
        {
            baseLocalPos = transform.localPosition;
            baseLocalEuler = transform.localEulerAngles;
        }
    }

    void LateUpdate()
    {
        if (!motor)
            return;

        Vector3 v = motor.PlanarVelocity;

        Transform refT = cameraPivot ? cameraPivot : Camera.main.transform;
        Vector3 localV = refT.InverseTransformDirection(v);

        float speedRef = Mathf.Max(1f, motor.maxGroundSpeed);
        Vector3 norm = localV / speedRef;

        Vector3 targetOffset =
            new Vector3(-norm.x * posResponse.x, -norm.y * posResponse.y, -norm.z * posResponse.z);

        if (targetOffset.magnitude > 1f) targetOffset.Normalize();
        targetOffset *= maxPosOffset;

        Vector3 targetPos = baseLocalPos + targetOffset;

        Vector3 targetRot =
            new Vector3(
                baseLocalEuler.x + (-norm.z * rotResponse.x) * maxRotOffset,
                baseLocalEuler.y + (-norm.x * rotResponse.y) * maxRotOffset,
                baseLocalEuler.z + ( norm.x * rotResponse.z) * maxRotOffset
            );

        if (enableBob)
        {
            float t = Time.time * bobSpeed;
            targetPos += new Vector3(0f, Mathf.Sin(t) * bobAmount, 0f);
        }

        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetPos,
            ref posVel,
            posLag
        );

        Vector3 current = transform.localEulerAngles;
        Vector3 smoothed = new Vector3(
            SmoothDampAngle(current.x, targetRot.x, ref rotVel.x, rotLag),
            SmoothDampAngle(current.y, targetRot.y, ref rotVel.y, rotLag),
            SmoothDampAngle(current.z, targetRot.z, ref rotVel.z, rotLag)
        );
        transform.localEulerAngles = smoothed;
    }

    static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime)
    {
        return Mathf.SmoothDampAngle(current, target, ref currentVelocity, smoothTime);
    }
}
