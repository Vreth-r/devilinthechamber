using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using DG.Tweening;

public class TutorialHandler : MonoBehaviour
{
    float elapsedTime;
    public PlayerControls controls;
    //bool playerMoved = false;
    public DialogueRunner dialogueRunner;
    public LinePresenter linePresenter;

    public Tutorial_DealMenu dealmenu;

    bool dialogueStarted = false;
    bool dealTakenDialogueStarted = false;



    public Image whiteImage;

    bool devilIgnored = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = GameManager.Instance.controls;
    }

    // Update is called once per frame
    void Update()
     {
        elapsedTime += Time.deltaTime;
        // Vector2 moveInput = Vector2.ClampMagnitude(controls.Player.Move.ReadValue<Vector2>(), 1f);
        // if (moveInput != Vector2.zero)
        // {
        //     playerMoved = true; 
        // }

        // if (playerMoved)
        // {
        //     elapsedTime += Time.deltaTime;
        // }

        if (elapsedTime >= 9f && dialogueStarted == false)
        {
            dialogueRunner.StartDialogue("Start");
            dialogueStarted = true;
        }

        if (elapsedTime >= 40f && devilIgnored == false)
        {
            dealmenu.controls.Disable();
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("Ignored");
            devilIgnored = true;
        }

        if (dealmenu.dealPicked && dealTakenDialogueStarted == false)
        {

            dealmenu.controls.Disable();
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("dealTaken");
            dealTakenDialogueStarted = true;
        }

    }

    [YarnCommand("transition_scene")]
    public async void transition_scene()
    {
        //whiteImage.DOFade(1f, 2f);
        controls.Player.Disable();
        GameManager.Instance.controls.Player.Disable();
        await SceneFader.Instance.FadeToScene("DITC_level1.0");
    }
    async void SkipTutorial()
    {
        await SceneFader.Instance.FadeToScene("DITC_level1.0");
    }
}
