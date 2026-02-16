using UnityEngine;

public class PlayerScriptRefHolder : MonoBehaviour
{
    public static PlayerScriptRefHolder Instance;
    public PlayerMotor playerMotor;
    public PlayerLook playerLook;
    public FovKick fovKick;
    public CameraMovement cameraMovement;
    public GunHitscan gunHitscan;
    public PlayerHealth health;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
