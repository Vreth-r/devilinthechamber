using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    Door door;
    public bool isOpen = true;
    public bool oneTime = true;
    int triggers = 0;
    void Start()
    {
        if (door == null)
        {
            door = GetComponentInChildren<Door>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        triggers++;
        Debug.Log("JE");
        if (oneTime && triggers > 1) return;
        if (door != null)
        {
            if (isOpen)
                door.Close();
            else
                door.Open();

            isOpen = !isOpen;
        }
    }
}
