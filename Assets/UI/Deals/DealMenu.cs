using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;

public class DealMenu : MonoBehaviour
{  
    public static DealMenu Instance;

    public Texture2D cardFront;
    public Texture2D cardBack;
    public Texture2D deathCard;

    int flippedCards = 0;

    Button deal1;
    Button deal2;
    Button deal3;
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
    public bool dealPicked = false;

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

        dealButtons = new List<Button>{deal1, deal2, deal3};

        deal1.clicked += ChooseDeal1;
        deal2.clicked += ChooseDeal2;
        deal3.clicked += ChooseDeal3;

        controls = new PlayerControls();
        
        SetVisible(false);
    }

    void OnEnable()
    {
        controls.Enable();
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (cameraScript!=null) cameraScript.enabled = false;
        dealMenuOpen = true;

        dealsDoc.sortingOrder = 1;

        deals = DeckManager.Instance.DrawDeals();

        SetDealsText();

        SetVisible(true);
    }

    void CloseMenu()
    {
        deal1.Clear();
        deal2.Clear();
        deal3.Clear();

        if (GameManager.Instance != null) GameManager.Instance.gamePaused = false;
        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (cameraScript!=null) cameraScript.enabled = true;
        dealMenuOpen = false;

        DeckManager.Instance.AddDeathCard(2);
        
        //hudDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        dealsDoc.sortingOrder = 1;
        SetVisible(false);
    }

    void ChooseDeal1 ()
    {
        dealPicked = true;
        deals[0].ApplyDeal();
        DeckManager.Instance.AddToChosenDeals(deals[0]);
        CloseMenu();
    }
    void ChooseDeal2 ()
    {
        dealPicked = true;
        deals[1].ApplyDeal();
        DeckManager.Instance.AddToChosenDeals(deals[1]);
        CloseMenu();
    }
    void ChooseDeal3 ()
    {
        dealPicked = true;
        deals[2].ApplyDeal(); 
        DeckManager.Instance.AddToChosenDeals(deals[2]);
        CloseMenu();
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
            if (Random.value < DeckManager.Instance.cardFlipChance * StatModManager.GetStatModifier(StatName.FLIPPED_CARD_CHANCE))
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
        Debug.Log($"Drew {deals[i].dealName}");
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
}