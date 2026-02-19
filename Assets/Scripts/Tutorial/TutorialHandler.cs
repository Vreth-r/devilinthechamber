using UnityEngine;
using Yarn.Unity;

public class TutorialHandler : MonoBehaviour
{
    float elapsedTime;
    public PlayerControls controls;
    public GameManager manager;
    bool playerMoved = false;
    public DialogueRunner dialogueRunner;
    bool dialogueStarted = false;

    bool devilIgnored = false;

    public
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = manager.controls;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = Vector2.ClampMagnitude(controls.Player.Move.ReadValue<Vector2>(), 1f);
        if (moveInput != Vector2.zero)
        {
            playerMoved = true; 
        }

        if (playerMoved)
        {
            elapsedTime += Time.deltaTime;
        }

        if (elapsedTime >= 4f && dialogueStarted == false)
        {
            dialogueRunner.StartDialogue("Start");
            dialogueStarted = true;
        }

        if (elapsedTime >= 25f && devilIgnored == false)
        {
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("Ignored");
            devilIgnored = true;
        }

    }
}
