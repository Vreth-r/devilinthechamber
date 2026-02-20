using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;

public class DealMenu : MonoBehaviour
{  
    public static DealMenu Instance;

    Button deal1;
    Button deal2;
    Button deal3;
    List<Deal> deals;

    [Header("UIDocuments")]
    [SerializeField]UIDocument dealsDoc;
    [SerializeField]UIDocument hudDoc;
    [SerializeField]PlayerLook cameraScript;
    
    VisualElement root;
    GameManager gameManager;
    PlayerControls controls;

    public bool dealPicked = false; // for the tutorial to trigger a thing

    bool isPaused; 


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

        root = dealsDoc.rootVisualElement;

        //deals = DeckManager.Instance.GetRandomDeals(); (do not uncomment until deals work)

        deal1 = root.Q<Button>("Deal1");
        deal2 = root.Q<Button>("Deal2");
        deal3 = root.Q<Button>("Deal3");

        deal1.clicked += chooseDeal1;
        deal2.clicked += chooseDeal2;
        deal3.clicked += chooseDeal3;

        controls = new PlayerControls();

        SetVisible(false);
        onEnable();
    }

    void Start()
    {
    }

    void onEnable()
    {
        controls.Enable();
        controls.Player.OpenDeals.performed += openMenu;
    }

    void onDisable()
    {
        controls.Disable();
    }

    void openMenu(InputAction.CallbackContext _)
    {
        dealPicked = false;
        if (GameManager.Instance != null) GameManager.Instance.gamePaused = true;
        
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (cameraScript!=null) cameraScript.enabled = false;

        hudDoc.enabled = false;

        SetVisible(true);
        Debug.Log("Successfully opened deals menu");
    }

    void closeMenu()
    {
        if (GameManager.Instance != null) GameManager.Instance.gamePaused = false;
        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraScript.enabled = true;
        
        hudDoc.enabled = true;

        SetVisible(false);
        dealPicked = true;
        Debug.Log("Successfully closed deals menu");
    }

    void chooseDeal1 ()
    {
        //deals[0].ApplyDeal(); (do not uncomment until deals work)
        closeMenu();
    }
    void chooseDeal2 ()
    {
        //deals[1].ApplyDeal(); (do not uncomment until deals work)
        closeMenu();
    }
    void chooseDeal3 ()
    {
        //deals[2].ApplyDeal(); (do not uncomment until deals work)
        closeMenu();
    }

    void SetVisible(bool visible)
    {
        root.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}