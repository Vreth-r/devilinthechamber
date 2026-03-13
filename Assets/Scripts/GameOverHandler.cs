using UnityEngine;
using Yarn.Unity;

public class GameOverHandler : MonoBehaviour
{
    [YarnCommand("mm")]
    public async void transition_scene()
    {
        await SceneFader.Instance.FadeToScene("MainMenu");
    }
}
