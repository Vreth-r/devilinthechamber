using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Pivots")]
    [Tooltip("Yaw only (left/right). Usually a child of the player root at eye height.")]
    public Transform yawPivot;

    [Tooltip("Pitch only (up/down). Child of yawPivot. Camera should be a child of this.")]
    public Transform pitchPivot;

    [Header("Settings")]
    public float sensitivity = 0.08f;
    public float pitchMin = -85f;
    public float pitchMax = 85f;

    public bool allowCursorRelockOnClick = true;

    PlayerControls controls;

    float yaw;
    float pitch;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void Start()
    {
        LockCursor(true);

        if (yawPivot && pitchPivot)
        {
            yaw = yawPivot.eulerAngles.y;

            float x = pitchPivot.localEulerAngles.x;
            if (x > 180f) x -= 360f;
            pitch = Mathf.Clamp(x, pitchMin, pitchMax);
        }
        else
        {
            Vector3 e = transform.eulerAngles;
            yaw = e.y;
            pitch = e.x;
        }

        ApplyRotation();
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        if (GameManager.Instance.gamePaused) return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            LockCursor(false);

        if (allowCursorRelockOnClick &&
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            UnityEngine.Cursor.lockState != CursorLockMode.Locked)
        {
            LockCursor(true);
        }

        Vector2 look = controls.Player.Look.ReadValue<Vector2>();

        yaw += look.x * sensitivity;
        pitch -= look.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        ApplyRotation();
    }

    void ApplyRotation()
    {
        if (!yawPivot || !pitchPivot) return;

        yawPivot.rotation = Quaternion.Euler(0f, yaw, 0f);

        pitchPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
