using System.Collections.Generic;
using UnityEngine;

public class FlickerTaggedPointLights : MonoBehaviour
{
    [Header("Tag")]
    [SerializeField] private string flickerTag = "candle";

    [Header("Intensity Flicker")]
    [Tooltip("Base intensity multiplier. 1 = use the light's original intensity.")]
    [Range(0.1f, 3f)]
    public float baseIntensityMultiplier = 1f;

    [Tooltip("How much the intensity varies (relative). Example: 0.25 = +/-25% around base.")]
    [Range(0f, 1f)]
    public float intensityVariation = 0.25f;

    [Tooltip("How fast the candle flickers.")]
    [Range(0.1f, 10f)]
    public float flickerSpeed = 2.5f;

    [Tooltip("Extra tiny random 'spark' jitter on top of noise.")]
    [Range(0f, 1f)]
    public float jitterAmount = 0.05f;

    [Header("Color Warmth Flicker")]
    public bool flickerColorSlightly = true;
    [Tooltip("How much to shift between warm and slightly less warm colors.")]
    [Range(0f, 1f)]
    public float colorVariation = 0.08f;

    [Header("Discovery")]
    [Tooltip("If > 0, re-scan scene every X seconds to include lights spawned later.")]
    public float refreshEverySeconds = 0f;

    private class FlickerLight
    {
        public Light light;
        public float baseIntensity;
        public Color baseColor;
        public float noiseSeed;
        public float jitterSeed;
    }

    private readonly List<FlickerLight> _lights = new();
    private float _refreshTimer;

    void Start()
    {
        RefreshLights();
    }

    void Update()
    {
        if (refreshEverySeconds > 0f)
        {
            _refreshTimer += Time.deltaTime;
            if (_refreshTimer >= refreshEverySeconds)
            {
                _refreshTimer = 0f;
                RefreshLights();
            }
        }

        float t = Time.time;

        for (int i = 0; i < _lights.Count; i++)
        {
            var fl = _lights[i];
            if (!fl.light) continue;

            float n = Mathf.PerlinNoise(fl.noiseSeed, t * flickerSpeed);

            float centered = 1f + (n - 0.5f) * 2f * intensityVariation;

            float j = (Mathf.PerlinNoise(fl.jitterSeed, t * (flickerSpeed * 3.7f)) - 0.5f) * 2f;
            float jitter = 1f + j * jitterAmount;

            float targetIntensity = fl.baseIntensity * baseIntensityMultiplier * centered * jitter;
            fl.light.intensity = Mathf.Max(0f, targetIntensity);

            if (flickerColorSlightly)
            {

                Color warmer = fl.baseColor;
                Color slightlyWhiter = Color.Lerp(fl.baseColor, Color.white, 0.15f);

                float cN = Mathf.PerlinNoise(fl.noiseSeed + 13.37f, t * (flickerSpeed * 0.8f));
                float cT = (cN - 0.5f) * 2f * colorVariation;
                float lerpT = Mathf.Clamp01(0.5f + cT);

                fl.light.color = Color.Lerp(warmer, slightlyWhiter, lerpT);
            }
        }
    }

    [ContextMenu("Refresh Lights Now")]
    public void RefreshLights()
    {
        _lights.Clear();

        var tagged = GameObject.FindGameObjectsWithTag(flickerTag);
        for (int i = 0; i < tagged.Length; i++)
        {
            var go = tagged[i];
            if (!go) continue;

            var l = go.GetComponent<Light>();
            if (!l) continue;
            if (l.type != LightType.Point) continue;

            _lights.Add(new FlickerLight
            {
                light = l,
                baseIntensity = l.intensity,
                baseColor = l.color,
                noiseSeed = Random.Range(0f, 9999f),
                jitterSeed = Random.Range(0f, 9999f)
            });
        }
    }
}
