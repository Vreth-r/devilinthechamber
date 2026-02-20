using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;

public class DealMenu : MonoBehaviour
{  
    public static DealMenu Instance;

    public Texture2D deathCard;

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
    GameManager gameManager;
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

    void OpenMenu()
    {
        if (GameManager.Instance != null) GameManager.Instance.gamePaused = true;
        
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (cameraScript!=null) cameraScript.enabled = false;
        dealMenuOpen = true;

        //hudDoc.enabled = false;
        //hudDoc.gameObject.SetActive(false);
        hudDoc.rootVisualElement.style.display = DisplayStyle.None;
        dealsDoc.sortingOrder = 1;

        deals = DeckManager.Instance.GetRandomDeals();

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
        cameraScript.enabled = true;
        dealMenuOpen = false;
        
        hudDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        dealsDoc.sortingOrder = 1;
        dealPicked = false;
        SetVisible(false);
    }

    void ChooseDeal1 ()
    {
        dealPicked = true;
        deals[0].ApplyDeal();
        CloseMenu();
    }
    void ChooseDeal2 ()
    {
        dealPicked = true;
        deals[1].ApplyDeal(); 
        CloseMenu();
    }
    void ChooseDeal3 ()
    {
        dealPicked = true;
        deals[2].ApplyDeal(); 
        CloseMenu();
    }

    void SetVisible(bool visible)
    {
        root.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    void SetDealsText()
    {

        for (int i = 0; i < dealButtons.Count; i++)
        {
            VisualElement posDealBox = new VisualElement();
            VisualElement negDealBox = new VisualElement();
            posDealBox.AddToClassList("CardTextBox");
            negDealBox.AddToClassList("CardTextBox");

            Deal curDeal = deals[i];

            foreach (StatMod statMod in curDeal.statDeals)
            {
                Label l = new Label();

                if (statMod.dealType == DealType.POSITIVE)
                {
                    string t;
                    if (statMod.statName == StatName.MAGAZINE_SIZE || statMod.statName == StatName.PERMA_HEALTH)
                        t = $"+{statMod.modifier}{DealsLocalization.StatLocale(statMod.statName)}";
                    else
                    {
                        if (statMod.modifier >= 1) 
                            t = $"+{100 * (statMod.modifier - 1)}{DealsLocalization.StatLocale(statMod.statName)}";
                        else
                            t = $"{100 * (statMod.modifier - 1)}{DealsLocalization.StatLocale(statMod.statName)}";
                    }
                    l.AddToClassList("BuffTitle");
                    l.text = t;
                    posDealBox.Add(l);
                }
                else
                {
                    string t;
                    if (statMod.statName == StatName.MAGAZINE_SIZE || statMod.statName == StatName.PERMA_HEALTH)
                        t = $"{statMod.modifier}{DealsLocalization.StatLocale(statMod.statName)}";
                    else
                        t = $"{100 * (statMod.modifier - 1)} {DealsLocalization.StatLocale(statMod.statName)}";
                    l.AddToClassList("DebuffTitle");
                    l.text = t;
                    negDealBox.Add(l);
                }
            }

            foreach (Ability ability in curDeal.abilityDeals)
            {
                if (ability.AbilityName == AbilityName.DEATH)
                {
                    dealButtons[i].style.backgroundImage = new StyleBackground(deathCard);
                }
                else
                {
                    string t;
                    if (ability.duration != -1)
                        t = $"{ability.duration}s{DealsLocalization.AbilityLocale(ability.AbilityName)}";
                    else
                        t = $"{DealsLocalization.AbilityLocale(ability.AbilityName)}";

                    Label l = new Label();

                    if (ability.dealType == DealType.POSITIVE)
                    {
                        l.AddToClassList("BuffTitle");
                        l.text = t;
                        posDealBox.Add(l);
                    }
                    else
                    {
                        l.AddToClassList("DebuffTitle");
                        l.text = t;
                        negDealBox.Add(l);
                    }

                    l.text = t;
                }
            }

            dealButtons[i].Add(posDealBox);
            dealButtons[i].Add(negDealBox);
            
        }
    }
}