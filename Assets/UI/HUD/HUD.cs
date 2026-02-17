using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using Unity.VisualScripting;

public class HUD : MonoBehaviour
{
    [Header("Demo Values")]
    public int hp = 100;
    public int hpMax = 100;
    public int ammoInMag = 5;
    public int ammoReserve = 10;

    public bool showHitIndicator = true;

    Color baseBackgroundTint = new Color(0, 0, 0, 0.3f);
    Color hitBackgroundTint = new Color(1, 0, 0, 0.45f);

    UIDocument doc;

    VisualElement background;
    VisualElement healthFill;
    Label healthText;
    Label ammoText;
    VisualElement perksRow;

    void Awake()
    {
        // event subs
        UIEvents.UpdateHealth += SetHealth;
        UIEvents.UpdateAmmo += SetAmmo;
        UIEvents.SetBlind += SetBlind;
        UIEvents.IndicateHit += IndicateHit;
        UIEvents.UpdateShowHitIndicator += SetShowHitIndicator;

        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        background = root.Q<VisualElement>("vignette-panel");
        healthFill = root.Q<VisualElement>("health-bar-fill");
        healthText = root.Q<Label>("health-text");
        ammoText = root.Q<Label>("ammo-text");
        perksRow = root.Q<VisualElement>("perks-row");

        // visual slots
        SetPerkCount(3);
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
        Debug.Log($"{mag}, {reserve}");
        ammoInMag = Mathf.Max(0, mag);
        ammoReserve = Mathf.Max(0, reserve);
        Refresh();
    }

    public void SetPerkCount(int count)
    {
        perksRow.Clear();
        for (int i = 0; i < count; i++)
        {
            var icon = new VisualElement();
            icon.AddToClassList("perk-icon");
            perksRow.Add(icon);
        }
    }

    public void SetPerkIcon(int index, Sprite sprite)
    {
        if (index < 0 || index >= perksRow.childCount) return;
        var icon = perksRow[index];

        icon.style.backgroundImage = sprite
            ? new StyleBackground(sprite)
            : StyleKeyword.None;
    }

    public void SetBlind (bool isBlind)
    {
        StartCoroutine(FadeToBlack(isBlind));

        IEnumerator FadeToBlack (bool forward)
        {
            Color regBgCol = new Color (0, 0, 0, 0);
            Color blindBgCol = new Color (0, 0, 0, 1);
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
            while (timer < 0.5)
            {
                timer += Time.deltaTime;
                float t = timer / 0.5f;
                background.style.backgroundColor =
                    new StyleColor(Color.Lerp(normalColor, newColor, t));
                yield return null;
            }
            background.style.unityBackgroundImageTintColor = new StyleColor(newColor);
        }
    }

    public void IndicateHit()
    {
        if (!showHitIndicator) return;
        StartCoroutine(HitAnim());
        IEnumerator HitAnim()
        {
            StartCoroutine(FadeFromTo(baseBackgroundTint, hitBackgroundTint, 0.05f));
            yield return new WaitForSeconds(0.075f);
            StartCoroutine(FadeFromTo(hitBackgroundTint, baseBackgroundTint, 0.05f));
        }
    }

    public void SetShowHitIndicator (bool showHitIndicator)
    {
        this.showHitIndicator = showHitIndicator;
    }

    void Refresh()
    {
        float t = Mathf.Clamp01(hp / (float)Mathf.Max(1, hpMax));
        healthFill.style.width = Length.Percent(t * 100f);
        healthText.text = $"{hp} / {hpMax}";

        ammoText.text = $"{ammoInMag} / {ammoReserve}";
    }

    // tried to be smart, ended up with more work lol
    IEnumerator FadeFromTo (Color normalColor, Color newColor, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            background.style.unityBackgroundImageTintColor =
                new StyleColor(Color.Lerp(normalColor, newColor, t));
            yield return null;
        }
        background.style.unityBackgroundImageTintColor = new StyleColor(newColor);
    }
}
