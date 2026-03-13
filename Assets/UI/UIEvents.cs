using System;
using UnityEngine;
using System.Collections.Generic;

public static class UIEvents
{
    public static Action UpdateHealth;
    public static Action UpdateAmmo;
    public static Action<bool> SetBlind;
    public static Action IndicateHit;
    public static Action<bool> UpdateShowHitIndicator;
    public static Action<int, Sprite> UpdatePerks;
    public static Action blink;
    public static Action OneEye;
    public static Action Die;
    public static Action DeathAnimFinished;
    public static Action ForceRefresh;

    public static void SetHealth()
    {
        UpdateHealth?.Invoke();
    }
    public static void SetAmmo()
    {
        UpdateAmmo?.Invoke();
    }

    public static void Hit ()
    {
        IndicateHit?.Invoke();
    }

    public static void SetShowHitIndicator (bool showHitIndicator)
    {
        UpdateShowHitIndicator?.Invoke(showHitIndicator);
    }

    public static void SetPerk(int index, Sprite sprite)
    {
        UpdatePerks?.Invoke(index, sprite);
    }

    public static void SetIsBlind(bool isBlind)
    {
        SetBlind?.Invoke(isBlind);
    }

    public static void DoBlink()
    {
        blink?.Invoke();
    }

    public static void SetOneEyed ()
    {
        OneEye?.Invoke();
    }

    public static void ForceHUDRefresh ()
    {
        ForceRefresh?.Invoke();
    }

    public static void DoDeathAnim ()
    {
        Die?.Invoke();
    }
    public static void NotifyDeathAnimFinished ()
    {
        DeathAnimFinished?.Invoke();
    }
}