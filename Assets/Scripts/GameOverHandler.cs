using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class GameOverHandler : MonoBehaviour
{
    PlayerControls controls;
    public GameObject ys;
    void Start()
    {
        controls = new PlayerControls();
        controls.Player.Jump.performed += BackToMainMenu;
        ys.SetActive(true);
    }

    [YarnCommand("mm")]
    public async void transition_scene()
    {
        await SceneFader.Instance.FadeToScene("MainMenu");
    }

    private void BackToMainMenu(InputAction.CallbackContext context)
    {
        transition_scene();
    }

}
