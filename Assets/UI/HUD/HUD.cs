using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class HUD : MonoBehaviour
{
    public int hp = 10;
    public int hpMax = 10;
    public int ammoInMag = 10;
    public int ammoReserve = 10;

    bool uiReady;

    public bool showHitIndicator = true;

    public Texture2D oneEyedImage;

    Color baseBackgroundTint = new Color(0, 0, 0, 0.3f);
    Color hitBackgroundTint = new Color(1, 0, 0, 0.45f);

    UIDocument doc;

    VisualElement backgroundVignette;
    VisualElement oneEyePanel;
    VisualElement healthContainer;
    VisualElement bulletContainer;
    VisualElement ammoBullet;
    Label livesText;
    Label ammoText;

    [Header("Health bar sprites")]
    [SerializeField]Sprite filledHP;
    [SerializeField]Sprite emptyHP;
    [SerializeField]Sprite bullet;

    void Awake()
    {
        doc = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        UIEvents.UpdateHealth += SetHealth;
        UIEvents.UpdateAmmo += SetAmmo;
        UIEvents.SetBlind += SetBlind;
        UIEvents.IndicateHit += IndicateHit;
        UIEvents.UpdateShowHitIndicator += SetShowHitIndicator;
        UIEvents.blink += BlinkWrapper;
        UIEvents.OneEye += SetOneEyed;

        uiReady = false;
        StartCoroutine(InitUIWhenReady());
    }

    void OnDisable()
    {
        UIEvents.UpdateHealth -= SetHealth;
        UIEvents.UpdateAmmo -= SetAmmo;
        UIEvents.SetBlind -= SetBlind;
        UIEvents.IndicateHit -= IndicateHit;
        UIEvents.UpdateShowHitIndicator -= SetShowHitIndicator;
        UIEvents.blink -= BlinkWrapper;
        UIEvents.OneEye -= SetOneEyed;
    }

    IEnumerator InitUIWhenReady()
    {
        if (doc == null) doc = GetComponent<UIDocument>();

        while (doc == null || doc.rootVisualElement == null)
            yield return null;

        var root = doc.rootVisualElement;

        backgroundVignette = root.Q<VisualElement>("vignette-panel");
        oneEyePanel        = root.Q<VisualElement>("one-eye-panel");
        healthContainer    = root.Q<VisualElement>("health-container");
        livesText          = root.Q<Label>("lives-text");
        ammoText           = root.Q<Label>("ammo-text");

        // Minimum needed for Refresh to run safely
        uiReady = healthContainer != null && ammoText != null;

        if (!uiReady)
        {
            Debug.LogError(
                $"[HUD] Missing required UI elements. " +
                $"healthContainer={healthContainer != null} ammoText={ammoText != null} " +
                $"vignette={backgroundVignette != null} livesText={livesText != null} " +
                $"(Check UXML names for this scene.)",
                this
            );
            yield break;
        }

        Refresh();
    }

    public void SetHealth(int current, int max)
    {
        hp = current;
        hpMax = Mathf.Max(1, max);
        Refresh();
    }


    public void SetAmmo(int mag, int reserve)
    {
        ammoInMag = Mathf.Max(0, mag);
        ammoReserve = Mathf.Max(0, reserve);
        Refresh();
    }

    public void SetBlind (bool isBlind)
    {
        if (!uiReady) return;
        StartCoroutine(FadeToBlack(isBlind));

        IEnumerator FadeToBlack (bool forward)
        {
            Color regBgCol = new Color (0, 0, 0, 0);
            Color blindBgCol = new Color (0, 0, 0, 0.85f);
            Color normalColor;
            Color newColor;
            if (forward)
            {
                normalColor = regBgCol;
                newColor = blindBgCol;
            }
            else
            {
                normalColor = blindBgCol;
                newColor = regBgCol;
            }
            float timer = 0f;
            while (timer < 0.3)
            {
                timer += Time.deltaTime;
                float t = timer / 0.5f;
                backgroundVignette.style.backgroundColor = Color.Lerp(normalColor, newColor, t);
                yield return null;
            }
            backgroundVignette.style.backgroundColor = new StyleColor(newColor);
        }
    }

    public void BlinkWrapper()
    {
        if (!uiReady) return;
        StartCoroutine(Blink());
        IEnumerator Blink ()
        {
            StartCoroutine(FadeToBlack(true));
            yield return new WaitForSeconds(0.15f);
            StartCoroutine(FadeToBlack(false));
            IEnumerator FadeToBlack (bool forward)
            {
                Color regBgCol = new Color (0, 0, 0, 0);
                Color blindBgCol = new Color (0, 0, 0, 1f);
                Color normalColor;
                Color newColor;
                if (forward)
                {
                    normalColor = regBgCol;
                    newColor = blindBgCol;
                }
                else
                {
                    normalColor = blindBgCol;
                    newColor = regBgCol;
                }
                float timer = 0f;
                while (timer < 0.075)
                {
                    timer += Time.deltaTime;
                    float t = timer / 0.5f;
                    backgroundVignette.style.backgroundColor = Color.Lerp(normalColor, newColor, t);
                    yield return null;
                }
                backgroundVignette.style.backgroundColor = new StyleColor(newColor);
            }
        }
    }

    void SetOneEyed()
    {
        if (!uiReady) return;
        oneEyePanel.style.backgroundImage = oneEyedImage;
        StartCoroutine(FadeFromTo(baseBackgroundTint, new Color(0, 0, 0, 1), 0.2f));
    }


    public void IndicateHit()
    {
        if (!showHitIndicator) return;
        if (!uiReady) return;
        if (backgroundVignette == null) return;

        StartCoroutine(HitAnim());
        IEnumerator HitAnim()
        {
            yield return FadeFromTo(baseBackgroundTint, hitBackgroundTint, 0.05f);
            yield return new WaitForSeconds(0.075f);
            yield return FadeFromTo(hitBackgroundTint, baseBackgroundTint, 0.05f);
        }
    }

    public void SetShowHitIndicator (bool showHitIndicator)
    {
        this.showHitIndicator = showHitIndicator;
    }

    void Refresh()
    {
        if (!uiReady) return;

        // health
        healthContainer.Clear();
        for (int i = 0; i < hpMax; i++)
        {
            Image hpImage = new Image();
            hpImage.sprite = (i < hp) ? filledHP : emptyHP;
            hpImage.style.width = 75;
            hpImage.style.height = 25;
            healthContainer.Add(hpImage);
        }

        // lives text is OPTIONAL: only set it if it exists + player manager exists
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "DITC_level1.0" && livesText != null)
        {
            var pm = PlayerManager.Instance;
            if (pm != null && pm.health != null)
                livesText.text = $"Time of Death: {NumToRoman(pm.health.lives)}";
            else
                livesText.text = "Time of Death: ?";
        }

        // ammo (ammoText required, so safe)
        if (ammoReserve == int.MaxValue)
            ammoText.text = "inf / inf";
        else
            ammoText.text = $"{ammoInMag} / {ammoReserve}";
    }

    // tried to be smart, ended up with more work lol
    IEnumerator FadeFromTo(Color normalColor, Color newColor, float duration)
    {
        if (!uiReady) yield break;
        if (backgroundVignette == null) yield break;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            backgroundVignette.style.unityBackgroundImageTintColor =
                new StyleColor(Color.Lerp(normalColor, newColor, t));
            yield return null;
        }
        backgroundVignette.style.unityBackgroundImageTintColor = new StyleColor(newColor);
    }

    string NumToRoman (int num)
    {
        switch (num)
        {
            case 0:
                return "0";
            case 1:
                return "I";
            case 2:
                return "II";
            case 3:
                return "III";
            case 4:
                return "IV";
            case 5:
                return "V";
            case 6:
                return "VI";
            case 7:
                return "VII";
            case 8:
                return "VIII";
            case 9:
                return "IX";
            case 10:
                return "X";
            case 11:
                return "XI";
            default:
                return "";
            
        }
    }
}
