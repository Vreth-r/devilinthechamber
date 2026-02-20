using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using Unity.VisualScripting;

public class HUD : MonoBehaviour
{
    public int hp = 10;
    public int hpMax = 10;
    public int ammoInMag = 10;
    public int ammoReserve = 10;

    public bool showHitIndicator = true;

    public Texture2D oneEyedImage;

    Color baseBackgroundTint = new Color(0, 0, 0, 0.3f);
    Color hitBackgroundTint = new Color(1, 0, 0, 0.45f);

    UIDocument doc;

    VisualElement background;
    VisualElement healthContainer;
    VisualElement bulletContainer;
    VisualElement ammoBullet;
    Label healthText;
    Label ammoText;

    [Header("Health bar sprites")]
    [SerializeField]Sprite filledHP;
    [SerializeField]Sprite emptyHP;
    [SerializeField]Sprite bullet;

    void Awake()
    {
        // event subs
        UIEvents.UpdateHealth += SetHealth;
        UIEvents.UpdateAmmo += SetAmmo;
        UIEvents.SetBlind += SetBlind;
        UIEvents.IndicateHit += IndicateHit;
        UIEvents.UpdateShowHitIndicator += SetShowHitIndicator;
        UIEvents.blink += BlinkWrapper;
        UIEvents.OneEye += SetOneEyed;

        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        background = root.Q<VisualElement>("vignette-panel");
        healthContainer =root.Q<VisualElement>("health-container");
        healthText = root.Q<Label>("health-text");
        ammoText = root.Q<Label>("ammo-text");

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
        StartCoroutine(FadeToBlack(isBlind));

        IEnumerator FadeToBlack (bool forward)
        {
            Color regBgCol = new Color (0, 0, 0, 0);
            Color blindBgCol = new Color (0, 0, 0, 0.99f);
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
                background.style.backgroundColor = Color.Lerp(normalColor, newColor, t);
                yield return null;
            }
            background.style.backgroundColor = new StyleColor(newColor);
        }
    }

    public void BlinkWrapper()
    {
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
                    background.style.backgroundColor = Color.Lerp(normalColor, newColor, t);
                    yield return null;
                }
                background.style.backgroundColor = new StyleColor(newColor);
            }
        }
    }

    void SetOneEyed ()
    {
        background.style.backgroundImage = oneEyedImage;
        StartCoroutine(FadeFromTo(baseBackgroundTint, new Color(0, 0, 0, 1), 0.2f));
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
        healthContainer.Clear();
        for (int i = 0; i < hpMax; i++)
        {
            Image hpImage = new Image();

            if (i < hp) hpImage.sprite = filledHP;
            else hpImage.sprite = emptyHP;

            hpImage.style.width = 75;
            hpImage.style.height = 25;
            //hpImage.style.marginRight = 0;

            healthContainer.Add(hpImage);
        }
        healthText.text = $"{hp} / {hpMax}";

        if (ammoReserve == int.MaxValue)
            ammoText.text = "inf / inf";
        else
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
