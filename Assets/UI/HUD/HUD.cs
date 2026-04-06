using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class HUD : MonoBehaviour
{
    bool uiReady;

    public Texture2D oneEyedImage;

    Color baseBackgroundTint = new Color(0, 0, 0, 0.3f);
    Color hitBackgroundTint = new Color(1, 0, 0, 0.45f);

    UIDocument doc;

    VisualElement backgroundVignette;
    VisualElement oneEyePanel;
    VisualElement healthBar;
    VisualElement willpowerBar;
    VisualElement ammoPanel;
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
        UIEvents.UpdateWillpower += SetWillpower;
        UIEvents.UpdateAmmo += SetAmmo;
        UIEvents.SetBlind += SetBlind;
        UIEvents.IndicateHit += IndicateHit;
        UIEvents.blink += BlinkWrapper;
        UIEvents.OneEye += SetOneEyed;
        UIEvents.ForceRefresh += ForceRefreshAll;

        uiReady = false;
        StartCoroutine(InitUIWhenReady());
    }

    void OnDisable()
    {
        UIEvents.UpdateHealth -= SetHealth;
        UIEvents.UpdateAmmo -= SetAmmo;
        UIEvents.SetBlind -= SetBlind;
        UIEvents.IndicateHit -= IndicateHit;
        UIEvents.blink -= BlinkWrapper;
        UIEvents.OneEye -= SetOneEyed;
        UIEvents.ForceRefresh -= ForceRefreshAll;
    }

    IEnumerator InitUIWhenReady()
    {
        if (doc == null) doc = GetComponent<UIDocument>();

        while (doc == null || doc.rootVisualElement == null)
            yield return null;

        var root = doc.rootVisualElement;

        backgroundVignette = root.Q<VisualElement>("vignette-panel");
        oneEyePanel        = root.Q<VisualElement>("one-eye-panel");
        healthBar          = root.Q<VisualElement>("health-bar");
        willpowerBar       = root.Q<VisualElement>("willpower-bar");
        ammoPanel          = root.Q<VisualElement>("ammo-panel");
        livesText          = root.Q<Label>("lives-text");

        // Minimum needed for Refresh to run safely
        uiReady = healthBar != null && ammoPanel != null && willpowerBar != null;

        if (!uiReady)
        {
            Debug.LogError(
                $"[HUD] Missing required UI elements. " +
                $"healthContainer={healthBar != null} ammoText={ammoText != null} " +
                $"vignette={backgroundVignette != null} livesText={livesText != null} " +
                $"(Check UXML names for this scene.)",
                this
            );
            yield break;
        }

        RefreshAll();
    }

    public void SetHealth()
    {
        RefreshHealth();
    }

    public void SetWillpower ()
    {
        RefreshWillpower();
    }

    public void SetAmmo()
    {
        RefreshAmmo();
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


    void ForceRefreshAll ()
    {
        RefreshAll();
    }

    void RefreshHealth ()
    {
        if (!uiReady) return;
        if (AbilityModManager.abilityFlags[AbilityName.WHERES_ME])
        {
            healthBar.parent.visible = false;
            return;
        }
        healthBar.style.width = Length.Percent(100 * (PlayerManager.Instance.health.currentHealth / (float)(PlayerManager.Instance.health.maxHealth + StatModManager.GetStatModifier(StatName.PERMA_HEALTH))));
    }

    void RefreshWillpower ()
    {
        if (!uiReady) return;
        if (AbilityModManager.abilityFlags[AbilityName.WHERES_ME])
        {
            willpowerBar.parent.visible = false;
            return;
        }
        willpowerBar.style.width = Length.Percent(100 * (PlayerManager.Instance.willpower.currentWillpower / (float)PlayerManager.Instance.willpower.maxWillpower));
    }

    void RefreshAmmo ()
    {
        if (!uiReady) return;
        ammoPanel.Clear();
        if (AbilityModManager.abilityFlags[AbilityName.WHERES_GUN] || !AbilityModManager.abilityFlags[AbilityName.RELOAD] || AbilityModManager.abilityFlags[AbilityName.INFINITE_MAG]) return;

        for (int i = 0; i < PlayerManager.Instance.gunHitscan.currentMagazine; i++)
        {
            VisualElement bullet = new VisualElement();
            bullet.AddToClassList("bullet-img");
            ammoPanel.Add(bullet);
        }
            
    }

    void RefreshAll()
    {
        RefreshHealth();
        RefreshWillpower();
        RefreshAmmo();
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

}
