using System;
using UnityEngine;
using System.Collections.Generic;

public static class UIEvents
{
    public static Action<int, int> UpdateHealth;
    public static Action<int, int> UpdateAmmo;
    public static Action<bool> SetBlind;
    public static Action IndicateHit;
    public static Action<bool> UpdateShowHitIndicator;

    public static void SetHealth(int current, int max)
    {
        UpdateHealth?.Invoke(current, max);
    }
    public static void SetAmmo(int current, int max)
    {
        UpdateAmmo?.Invoke(current, max);
    }

    public static void Hit ()
    {
        IndicateHit?.Invoke();
    }

    public static void SetShowHitIndicator (bool showHitIndicator)
    {
        UpdateShowHitIndicator?.Invoke(showHitIndicator);
    }

    

    public static void SetIsBlind(bool isBlind)
    {
        SetBlind?.Invoke(isBlind);
    }
}