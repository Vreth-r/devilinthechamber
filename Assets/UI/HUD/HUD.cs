using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    [Header("Demo Values")]
    public int hp = 100;
    public int hpMax = 100;
    public int ammoInMag = 5;
    public int ammoReserve = 10;

    UIDocument doc;

    VisualElement healthFill;
    Label healthText;
    Label ammoText;
    VisualElement perksRow;

    void Awake()
    {
        // event subs
        UIEvents.UpdateHealth += SetHealth;
        UIEvents.UpdateAmmo += SetAmmo;

        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

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

    void Refresh()
    {
        float t = Mathf.Clamp01(hp / (float)Mathf.Max(1, hpMax));
        healthFill.style.width = Length.Percent(t * 100f);
        healthText.text = $"{hp} / {hpMax}";

        ammoText.text = $"{ammoInMag} / {ammoReserve}";
    }
}
