using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;
using UnityEditor.MPE;
using FMODUnity;

public class DealMenu : MonoBehaviour
{  
    public static DealMenu Instance;

    public Texture2D cardFront;
    public Texture2D cardBack;
    public Texture2D deathCard;
    public EventReference StartDealSFX;
    public EventReference SelectDealSFX;
    public EventReference ChooseDealSFX;

    int flippedCards = 0;

    Button deal1;
    Button deal2;
    Button deal3;
    Button chooseButton;
    List<Button> dealButtons;
    List<Deal> deals;

    [Header("UIDocuments")]
    [SerializeField]UIDocument dealsDoc;
    [SerializeField]UIDocument hudDoc;
    [SerializeField]PlayerLook cameraScript;
    [SerializeField]PauseMenu pauseMenu;

    VisualElement root;
    PlayerControls controls;
    
    public bool dealMenuOpen;
    int pickedIndex = -1;
    public bool dealPicked = false;

    int col = 0;
    int row = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (dealsDoc == null)
            dealsDoc = GetComponent<UIDocument>();

        dealsDoc.enabled = true;
        dealMenuOpen = false;
        root = dealsDoc.rootVisualElement;

        deal1 = root.Q<Button>("Deal1");
        deal2 = root.Q<Button>("Deal2");
        deal3 = root.Q<Button>("Deal3");
        chooseButton = root.Q<Button>("ChooseButton");

        dealButtons = new List<Button>{deal1, deal2, deal3};

        deal1.clicked += SelectDeal1;
        deal2.clicked += SelectDeal2;
        deal3.clicked += SelectDeal3;
        chooseButton.clicked += ChooseDeal;
        
        SetVisible(false);
    }

    void OnEnable()
    {
        if(controls != null)
        {
            controls.Player.OpenDeals.performed += MenuCheck;
        }
    }

    void OnDisable()
    {
        if(controls != null)
        {
            controls.Player.OpenDeals.performed -= MenuCheck;
        }
    }

    void Start()
    {
        controls = GameManager.Instance.controls;
        controls.Player.OpenDeals.performed += MenuCheck;
    }

    void MenuCheck(InputAction.CallbackContext _)
    {
        if(!dealMenuOpen && !pauseMenu.isPaused) OpenMenu();
    }

    public void OpenMenu()
    {
        if (GameManager.Instance != null) GameManager.Instance.gamePaused = true;
        
        Time.timeScale = 0f;

        GameManager.Instance.SetInputMap(false);

        if (GameManager.Instance.lastInputDevice is not Gamepad)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (cameraScript!=null) cameraScript.enabled = false;
        dealMenuOpen = true;

        dealsDoc.sortingOrder = 1;

        for (int i = 0; i < 3; i++)
            dealButtons[i].visible = true;

        dealPicked = false;
        deals = DeckManager.Instance.DrawDeals();

        DeselectAll();

        controls.UI.UIMove.performed += NavMenuController;
        controls.UI.UISelect.performed += SelectButtonMenuController;
        SetDealsText();
        RuntimeManager.PlayOneShot(StartDealSFX);

        SetVisible(true);
    }

    private void NavMenuController(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();

        if (col == 0 && move.y < 0 && dealPicked)
        {
            row = -1;
            col = 1;
            chooseButton.AddToClassList("hovered");
        }
        else if (col == 1 && move.y > 0)
        {
            row = 1;
            col = 0;
            chooseButton.RemoveFromClassList("hovered");
        }
        else if (row < dealButtons.Count - 1 && col == 0 && move.x > 0)
        {
            row += 1;
            HoverButton();
        }
        else if (row > 0 && col == 0 && move.x < 0)
        {
            row -= 1;
            for (int i = 0; i < dealButtons.Count; i++)
            HoverButton();
        }
        
    }

    private void SelectButtonMenuController(InputAction.CallbackContext context)
    {
        if (col == 0)
        {
            switch (row)
            {
                case 0:
                    SelectDeal1();
                    return;
                case 1:
                    SelectDeal2();
                    return;
                case 2:
                    SelectDeal3();
                    return;
            }
            RuntimeManager.PlayOneShot(SelectDealSFX);
        }
        else 
            ChooseDeal();
    }


    void CloseMenu()
    {
        deal1.Clear();
        deal2.Clear();
        deal3.Clear();

        if (GameManager.Instance != null) GameManager.Instance.gamePaused = false;
        
        Time.timeScale = 1f;
        if (GameManager.Instance.lastInputDevice is not Gamepad)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (cameraScript!=null) cameraScript.enabled = true;
        dealMenuOpen = false;

        DeckManager.Instance.AddDeathCard(2);
        controls.UI.UIMove.performed -= NavMenuController;
        controls.UI.UISelect.performed -= SelectButtonMenuController;
        GameManager.Instance.SetInputMap(true);
        DeselectAll();
        
        //hudDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        dealsDoc.sortingOrder = 0;
        SetVisible(false);
    }

    void SelectDeal1 ()
    {
        pickedIndex = 0;
        dealPicked = true;
        deal1.RemoveFromClassList("DealNotSelected");
        deal1.AddToClassList("DealSelected");

        deal2.RemoveFromClassList("DealSelected");
        deal2.AddToClassList("DealNotSelected");

        deal3.RemoveFromClassList("DealSelected");
        deal3.AddToClassList("DealNotSelected");
        chooseButton.SetEnabled(true);
    }
    void SelectDeal2 ()
    {
        pickedIndex = 1;
        dealPicked = true;
        deal1.RemoveFromClassList("DealSelected");
        deal1.AddToClassList("DealNotSelected");

        deal2.RemoveFromClassList("DealNotSelected");
        deal2.AddToClassList("DealSelected");

        deal3.RemoveFromClassList("DealSelected");
        deal3.AddToClassList("DealNotSelected");
        chooseButton.SetEnabled(true);
    }
    void SelectDeal3 ()
    {
        pickedIndex = 2;
        dealPicked = true;
        deal1.RemoveFromClassList("DealSelected");
        deal1.AddToClassList("DealNotSelected");

        deal2.RemoveFromClassList("DealSelected");
        deal2.AddToClassList("DealNotSelected");

        deal3.RemoveFromClassList("DealNotSelected");
        deal3.AddToClassList("DealSelected");
        chooseButton.SetEnabled(true);
    }

    void ChooseDeal ()
    {
        if (!dealPicked) return;
        deals[pickedIndex].ApplyDeal();
        HighlightDeal(pickedIndex);
        RuntimeManager.PlayOneShot(ChooseDealSFX);
    }

    void SetVisible(bool visible)
    {
        root.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    void SetDealsText()
    {
        flippedCards = 0;
        if (AbilityModManager.abilityFlags[AbilityName.ALL_CARDS_FLIPPED])
        {
            for (int i = 0; i < deals.Count; i++)
            {
                dealButtons[i].style.backgroundImage = new StyleBackground(cardBack);
            }
            AbilityModManager.abilityFlags[AbilityName.ALL_CARDS_FLIPPED] = false;
            return;
        }
        for (int i = 0; i < deals.Count; i++)
        {
            if (Random.value < DeckManager.Instance.cardFlipChance + StatModManager.GetStatModifier(StatName.FLIPPED_CARD_CHANCE))
            {
                flippedCards++;
                dealButtons[i].style.backgroundImage = new StyleBackground(cardBack);
                continue;
            }
            populateDealData(i);
        }
        if (flippedCards == 0 && AbilityModManager.abilityFlags[AbilityName.ONE_CARD_ALWAYS_FLIPPED])
        {
            int i = Random.Range(0, 2);
            dealButtons[i].Clear();
            dealButtons[i].style.backgroundImage = new StyleBackground(cardBack);
        }
        if (flippedCards == 3 && AbilityModManager.abilityFlags[AbilityName.ONE_CARD_NEVER_FLIPPED])
        {
            int i = Random.Range(0, 2);
            populateDealData(i);
        }
    }

    void populateDealData(int i)
    {
        if (deals[i].dealName == "Death")
        {
            dealButtons[i].style.backgroundImage = new StyleBackground(deathCard);
            return;
        }
        dealButtons[i].style.backgroundImage = new StyleBackground(cardFront);
        Debug.Log($"Drew: {deals[i].dealName}");
        VisualElement dealCardInfo = new VisualElement();
        dealCardInfo.AddToClassList("CardTextBox");

        Label dealName = new Label();
        dealName.AddToClassList("DealTitle");
        dealName.text = deals[i].dealName;

        Label dealDesc = new Label();
        dealDesc.AddToClassList("DealDescription");
        dealDesc.text = deals[i].dealDescription;

        dealCardInfo.Add(dealName);
        dealCardInfo.Add(dealDesc);

        dealButtons[i].Add(dealCardInfo);
    }

    void HighlightDeal (int i)
    {

        for (int j = 0; j < 3; j++)
            if (i != j) dealButtons[j].visible = false;

        dealButtons[i].Clear();
        populateDealData(i);

        root.schedule.Execute(() =>
        {
            CloseMenu();
        }).StartingIn(3000);
    }
    void HoverButton ()
    {
        for (int i = 0; i < dealButtons.Count; i++)
        {
            if (i == row)
                dealButtons[i].AddToClassList("hovered");
            else
                dealButtons[i].RemoveFromClassList("hovered");
        }
    }

    void DeselectAll ()
    {
        foreach (Button b in dealButtons)
        {
            b.RemoveFromClassList("hovered");
            b.RemoveFromClassList("DealSelected");
            b.AddToClassList("DealNotSelected");
        }
        chooseButton.SetEnabled(false);
        chooseButton.RemoveFromClassList("hovered");
        dealPicked = false;
        row = 0;
        col = 0;
    }
}