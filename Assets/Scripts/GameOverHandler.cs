using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class GameOverHandler : MonoBehaviour
{
    PlayerControls controls;
    void Start()
    {
        controls = GameManager.Instance.controls;
        controls.Player.Jump.performed += BackToMainMenu;
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
