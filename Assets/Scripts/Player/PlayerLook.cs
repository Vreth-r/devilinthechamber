using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public Transform cameraPivot;

    public float sensitivity = 0.08f;
    public float pitchMin = -85f;
    public float pitchMax = 85f;

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

        Vector3 e = cameraPivot ? cameraPivot.eulerAngles : transform.eulerAngles;
        yaw = e.y;
        pitch = e.x;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        if (GameManager.Instance.gamePaused) return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            LockCursor(false);

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked)
            LockCursor(true);

        Vector2 look = controls.Player.Look.ReadValue<Vector2>();

        yaw += look.x * sensitivity;
        pitch -= look.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        if (cameraPivot)
            cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
