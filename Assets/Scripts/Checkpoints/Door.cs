using System.Collections;
using UnityEngine;

public abstract class Door : MonoBehaviour
{
    bool isOpen;
    public void Close ()
    {
        if (isOpen)
        {
            StartCoroutine(PlayAnimation(false));
            isOpen = false;
        }
    }

    public void Open ()
    {
        if (!isOpen)
        {
            StartCoroutine(PlayAnimation(true));
            isOpen = true;
        }
    }

    protected abstract IEnumerator PlayAnimation(bool forward);

    public bool GetIsOpen () { return isOpen; }
}
