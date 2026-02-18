using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public PlayerMotor playerMotor;
    public PlayerLook playerLook;
    public FovKick fovKick;
    public CameraMovement cameraMovement;
    public GunHitscan gunHitscan;
    public PlayerHealth health;
    public CharacterController controller;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        controller = gameObject.GetComponent<CharacterController>();
    }
}
