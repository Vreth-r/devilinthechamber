using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FovKick : MonoBehaviour
{
    [Header("Base")]
    public float baseFov = 75f;

    [Header("Slide Kick")]
    public float slideKickAmount = 18f;
    public float slideSpeedExponent = 2.2f;
    public float slideKickSmooth = 18f;

    [Header("Speed FOV")]
    public bool useSpeedFov = true;
    public float speedFovAmount = 8f;
    public float speedForMaxFov = 18f;
    public float speedFovSmooth = 10f;

    public Camera cam;

    bool sliding;
    float slideStartSpeed = 1f;

    float slideKick;
    float slideKickVel;
    float slideKickTarget;

    float speedFov;
    float speedFovTarget;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (baseFov <= 1f) baseFov = cam.fieldOfView;
    }

    void LateUpdate()
    {
        slideKick = Mathf.Lerp(slideKick, slideKickTarget, 1f - Mathf.Exp(-slideKickSmooth * Time.deltaTime));

        speedFov = Mathf.Lerp(speedFov, speedFovTarget, 1f - Mathf.Exp(-speedFovSmooth * Time.deltaTime));

        cam.fieldOfView = baseFov + slideKick + speedFov;
    }

    public void BeginSlide(float startPlanarSpeed)
    {
        sliding = true;
        slideStartSpeed = Mathf.Max(0.01f, startPlanarSpeed);
    }

    public void UpdateSlideSpeed(float currentPlanarSpeed)
    {
        if (!sliding)
        {
            slideKickTarget = 0f;
            return;
        }
        float t = Mathf.Clamp01(currentPlanarSpeed / slideStartSpeed);

        float shaped = Mathf.Pow(t, slideSpeedExponent);

        slideKickTarget = shaped * slideKickAmount;
    }

    public void EndSlide()
    {
        sliding = false;
        slideKickTarget = 0f;
    }

    public void SetSpeed(float planarSpeed)
    {
        if (!useSpeedFov)
        {
            speedFovTarget = 0f;
            return;
        }

        float t = Mathf.Clamp01(planarSpeed / Mathf.Max(0.01f, speedForMaxFov));
        speedFovTarget = t * speedFovAmount;
    }  
}