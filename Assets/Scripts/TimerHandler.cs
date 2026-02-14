using System.Collections.Generic;
using System;
using UnityEngine;

class TimerHandle
{
    private float currentTime = 0f;
    public float length; // total length of timer
    public Func<bool> timerEndFunction; // function to run on end
    public bool timerDone = false;

    public TimerHandle (float length, Func<bool> timerEndFunction)
    {
        this.length = length;
        this.timerEndFunction = timerEndFunction;
    }

    public void Update ()
    {
        currentTime += Time.deltaTime;
        timerDone = currentTime > length; // is it done yet?
    }

    public void FireEvent()
    {
        if (timerEndFunction != null) // just for safety
            timerEndFunction();
    }
}

public class TimerHandler : MonoBehaviour
{
    public static TimerHandler Instance;

    // all current timer handles
    private List<TimerHandle> timerHandles = new List<TimerHandle>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Update()
    {
        // update and check all timer handles
        for (int i = timerHandles.Count - 1; i >= 0; i--) // decrement because removal of indices
        {
            timerHandles[i].Update(); // update timer

            // if done fire end event and remove from list
            if (timerHandles[i].timerDone)
            {
                timerHandles[i].FireEvent();
                timerHandles.RemoveAt(i);
            }
        }
    }

    // adds a new timer handle to the list
    public void CreateTimerHandle (float length, Func<bool> timerEndFunction)
    {
        timerHandles.Add(new TimerHandle(length, timerEndFunction));
    }
}

