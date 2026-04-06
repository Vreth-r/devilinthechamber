using UnityEngine;


public class DoorHandler : MonoBehaviour
{
    public enum DoorType { OPEN_LEFT, OPEN_RIGHT, SLIDE_DOWN, SLIDE_UP };
    
    public DoorType doorType = DoorType.OPEN_LEFT;
    Door door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        switch (doorType)
        {
            case DoorType.OPEN_LEFT:
                gameObject.AddComponent<DoorSwingLeft>();
                break;
            case DoorType.OPEN_RIGHT:
                gameObject.AddComponent<DoorSwingRight>();
                break;
            case DoorType.SLIDE_DOWN:
                gameObject.AddComponent<DoorCeilingDown>();
                break;
            case DoorType.SLIDE_UP:
                gameObject.AddComponent<DoorFloorUp>();
                break;
        }
    }
}
