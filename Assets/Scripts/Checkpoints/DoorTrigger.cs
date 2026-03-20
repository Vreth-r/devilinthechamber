using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door door;
    public bool closeDoor = true;
    void OnTriggerEnter(Collider other)
    {
        if (door != null)
        {
            if (closeDoor)
                door.Close();
            else
                door.Open();
        }
    }
}
