using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;

public class Tutorial_DealMenu : MonoBehaviour
{  
    public static Tutorial_DealMenu Instance;

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
    public PlayerControls controls;
    
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
        SetVisible(false);
    }

    void ChooseDeal1 ()
    {
        dealPicked = true;
        // deals[0].ApplyDeal();
        CloseMenu();
    }
    void ChooseDeal2 ()
    {
        dealPicked = true;
        //deals[1].ApplyDeal(); 
        CloseMenu();
    }
    void ChooseDeal3 ()
    {
        dealPicked = true;
        //deals[2].ApplyDeal(); 
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

            Label l1 = new Label();
            Label l2 = new Label();
            

            string t1 = "\nEscape this place by \nmaking deals with me.";
            l1.AddToClassList("BuffTitle");
            l1.text = t1;
            posDealBox.Add(l1);

            string t2 = "A cost, of course.";
            l2.AddToClassList("DebuffTitle");
            l2.text = t2;
            negDealBox.Add(l2);
       

            dealButtons[i].Add(posDealBox);
            dealButtons[i].Add(negDealBox);
            
        }
    }
}