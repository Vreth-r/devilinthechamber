using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeathScreen : MonoBehaviour
{
    UIDocument doc;

    VisualElement background;
    VisualElement deathMarker;
    Label livesRemaining;
    float fadeDuration = 1.2f;
    Vector3 textRGB = new Vector3 (0.7294118f, 0.1215686f, 0.1215686f);

    void OnEnable()
    {
        if (doc == null) doc = GetComponent<UIDocument>();

        var root = doc.rootVisualElement;

        background = root.Q<VisualElement>("Background");
        deathMarker = root.Q<VisualElement>("DeathMarker");
        livesRemaining = root.Q<Label>("Text");

        deathMarker.visible = false;

        UIEvents.Die += PlayDeathAnimation;
    }

    void OnDisable()
    {
        UIEvents.Die -= PlayDeathAnimation;        
    }

    void PlayDeathAnimation()
    {
        doc.sortingOrder = 2;
        float fadeMs = fadeDuration * 1000f;

        // Fade to black
        background.style.backgroundColor = new Color(0,0,0,1);

        // set timers for transitions
        livesRemaining.schedule.Execute(() =>
        {
            livesRemaining.text = NumToRoman(PlayerManager.Instance.health.deaths - 1);
        }).StartingIn((long)fadeMs);

        livesRemaining.schedule.Execute(() =>
        {
            deathMarker.visible = true;
        }).StartingIn((long)(fadeMs + 750));

        livesRemaining.schedule.Execute(() =>
        {
            deathMarker.visible = false;
            livesRemaining.text = NumToRoman(PlayerManager.Instance.health.deaths);
        }).StartingIn((long)(fadeMs + 2250));

        livesRemaining.schedule.Execute(() =>
        {
            deathMarker.visible = true;
        }).StartingIn((long)(fadeMs + 2750));

        livesRemaining.schedule.Execute(() =>
        {
            background.style.backgroundColor = new Color(0,0,0,0);
            livesRemaining.style.color = new Color(textRGB.x, textRGB.y, textRGB.z, 0);
            UIEvents.NotifyDeathAnimFinished();
        }).StartingIn((long)(fadeMs + 5500));

        livesRemaining.schedule.Execute(() =>
        {
            deathMarker.visible = false;
            livesRemaining.style.color = new Color(textRGB.x, textRGB.y, textRGB.z, 1);
            doc.sortingOrder = 0;
            UIEvents.ForceHUDRefresh();
        }).StartingIn((long)(fadeMs + 6000));
    }
    Dictionary<int, string> baseRomanNums = new Dictionary<int, string>
    {
        {1, "I"},
        {4, "IV"},
        {5, "V"},
        {9, "IX"},
        {10, "X"},
        {40, "XL"},
        {50, "L"},
    };
    string NumToRoman (int num)
    { 
        if (num == 0) return "O";
        string romanNum = "";
        int d = 1;
        while (num >= d)
            d *= 10;
        
        d /= 10;

        while (num > 0)
        {
            int last = num / d;
            if (last <= 3)
            {
                for (int i = 0; i < last; i++) romanNum += baseRomanNums[d];
            }
            else if (last == 4)
                romanNum += baseRomanNums[d] + baseRomanNums[d * 5];
            else if (5 <= last && last <= 8)
            {
                romanNum += baseRomanNums[d * 5];
                for (int i = 0; i < last - 5; i++) romanNum += baseRomanNums[d];
            }
            else if (last == 9)
                romanNum += baseRomanNums[d] + baseRomanNums[d * 10];
            num = num % d;
            d /= 10;
        }

        return romanNum;
    }
}