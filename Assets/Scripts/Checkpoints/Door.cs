using System.Collections;
using UnityEngine;

public abstract class Door : MonoBehaviour
{
    public void Close ()
    {
            StartCoroutine(PlayAnimation(false));
    }

    public void Open ()
    {
        StartCoroutine(PlayAnimation(true));
    }

    protected abstract IEnumerator PlayAnimation(bool forward);

}
