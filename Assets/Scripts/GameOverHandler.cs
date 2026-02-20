using UnityEngine;
using Yarn.Unity;

public class GameOverHandler : MonoBehaviour
{
    [YarnCommand("mm")]
    public void transition_scene()
    {
        SceneFader.Instance.FadeToScene("MainMenu");
    }
}
