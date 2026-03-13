using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DeathScreen : MonoBehaviour
{
    UIDocument doc;

    VisualElement background;
    Label livesRemaining;
    float fadeDuration = 1.2f;
    Vector3 textRGB = new Vector3 (0.7294118f, 0.1215686f, 0.1215686f);

    void OnEnable()
    {
        if (doc == null) doc = GetComponent<UIDocument>();

        var root = doc.rootVisualElement;

        background = root.Q<VisualElement>("Background");
        livesRemaining = root.Q<Label>("Text");

        livesRemaining.visible = false;

        UIEvents.Die += PlayDeathAnimation;
    }

    void OnDisable()
    {
        UIEvents.Die -= PlayDeathAnimation;        
    }

    void PlayDeathAnimation()
    {
        float fadeMs = fadeDuration * 1000f;

        // Fade to black
        background.style.backgroundColor = new Color(0,0,0,1);

        // set timers for transitions
        livesRemaining.schedule.Execute(() =>
        {
            livesRemaining.text = NumToRoman(PlayerManager.Instance.health.lives + 1);
        }).StartingIn((long)fadeMs);

        livesRemaining.schedule.Execute(() =>
        {
            livesRemaining.visible = true;
        }).StartingIn((long)(fadeMs + 750));

        livesRemaining.schedule.Execute(() =>
        {
            livesRemaining.visible = false;
            livesRemaining.text = NumToRoman(PlayerManager.Instance.health.lives);
        }).StartingIn((long)(fadeMs + 2250));

        livesRemaining.schedule.Execute(() =>
        {
            livesRemaining.visible = true;
        }).StartingIn((long)(fadeMs + 2750));

        livesRemaining.schedule.Execute(() =>
        {
            background.style.backgroundColor = new Color(0,0,0,0);
            livesRemaining.style.color = new Color(textRGB.x, textRGB.y, textRGB.z, 0);
            UIEvents.NotifyDeathAnimFinished();
        }).StartingIn((long)(fadeMs + 5500));

        livesRemaining.schedule.Execute(() =>
        {
            livesRemaining.visible = false;
            livesRemaining.style.color = new Color(textRGB.x, textRGB.y, textRGB.z, 1);
        }).StartingIn((long)(fadeMs + 6000));

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
