using System.Collections;
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
    [SerializeField] UIDocument dealsDoc;
    [SerializeField] UIDocument hudDoc;
    [SerializeField] PlayerLook cameraScript;
    [SerializeField] PauseMenu pauseMenu;

    VisualElement root;

    public PlayerControls controls;

    public bool dealMenuOpen;
    public bool dealPicked = false;

    bool uiReady;
    bool inputHooked;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (dealsDoc == null)
            dealsDoc = GetComponent<UIDocument>();

        if (dealsDoc == null)
        {
            Debug.LogError("[Tutorial_DealMenu] Missing dealsDoc UIDocument.", this);
            enabled = false;
            return;
        }

        // Create controls once
        controls = new PlayerControls();

        // Ensure doc is enabled (so it can build the panel)
        dealsDoc.enabled = true;

        dealMenuOpen = false;
        uiReady = false;

        // IMPORTANT: don't touch rootVisualElement in Awake in builds
        StartCoroutine(InitUIWhenReady());
    }

    IEnumerator InitUIWhenReady()
    {
        // Wait until the UIDocument has a root
        while (dealsDoc == null || dealsDoc.rootVisualElement == null)
            yield return null;

        root = dealsDoc.rootVisualElement;

        // Query buttons (these names MUST match your UXML)
        deal1 = root.Q<Button>("Deal1");
        deal2 = root.Q<Button>("Deal2");
        deal3 = root.Q<Button>("Deal3");

        if (deal1 == null || deal2 == null || deal3 == null)
        {
            Debug.LogError("[Tutorial_DealMenu] Could not find Deal1/Deal2/Deal3 in deals UXML. Check element names.", this);
            yield break;
        }

        dealButtons = new List<Button> { deal1, deal2, deal3 };

        // Hook click handlers once
        deal1.clicked += ChooseDeal1;
        deal2.clicked += ChooseDeal2;
        deal3.clicked += ChooseDeal3;

        // Start hidden
        SetVisible(false);

        uiReady = true;
    }

    void OnEnable()
    {
        // Controls can enable immediately
        controls?.Enable();

        // But don't hook callbacks multiple times
        if (!inputHooked && controls != null)
        {
            controls.Player.OpenDeals.performed += MenuCheck;
            inputHooked = true;
        }
    }

    void OnDisable()
    {
        if (inputHooked && controls != null)
        {
            controls.Player.OpenDeals.performed -= MenuCheck;
            inputHooked = false;
        }

        controls?.Disable();
    }

    void MenuCheck(InputAction.CallbackContext _)
    {
        if (!uiReady) return;
        if (dealMenuOpen) return;
        if (pauseMenu != null && pauseMenu.isPaused) return;

        OpenMenu();
    }

    public void OpenMenu()
    {
        if (!uiReady)
        {
            Debug.LogWarning("[Tutorial_DealMenu] OpenMenu called before UI ready.", this);
            return;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.gamePaused = true;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (cameraScript != null)
            cameraScript.enabled = false;

        dealMenuOpen = true;

        // Hide HUD safely (hudDoc might be null, or root might not be ready yet)
        if (hudDoc != null && hudDoc.rootVisualElement != null)
            hudDoc.rootVisualElement.style.display = DisplayStyle.None;

        dealsDoc.sortingOrder = 1;

        // Deals source might not exist in tutorial scenes — guard it
        if (DeckManager.Instance != null)
        {
            deals = DeckManager.Instance.GetRandomDeals();
        }
        else
        {
            deals = null;
            Debug.LogWarning("[Tutorial_DealMenu] DeckManager.Instance is null; showing menu without deals.", this);
        }

        SetDealsText();
        SetVisible(true);
    }

    void CloseMenu()
    {
        if (!uiReady) return;

        // Clear safely
        deal1?.Clear();
        deal2?.Clear();
        deal3?.Clear();

        if (GameManager.Instance != null)
            GameManager.Instance.gamePaused = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraScript != null)
            cameraScript.enabled = true;

        dealMenuOpen = false;

        if (hudDoc != null && hudDoc.rootVisualElement != null)
            hudDoc.rootVisualElement.style.display = DisplayStyle.Flex;

        dealsDoc.sortingOrder = 1;
        SetVisible(false);
    }

    void ChooseDeal1()
    {
        dealPicked = true;
        // if (deals != null && deals.Count > 0) deals[0].ApplyDeal();
        CloseMenu();
    }

    void ChooseDeal2()
    {
        dealPicked = true;
        // if (deals != null && deals.Count > 1) deals[1].ApplyDeal();
        CloseMenu();
    }

    void ChooseDeal3()
    {
        dealPicked = true;
        // if (deals != null && deals.Count > 2) deals[2].ApplyDeal();
        CloseMenu();
    }

    void SetVisible(bool visible)
    {
        if (root == null) return;
        root.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    void SetDealsText()
    {
        if (!uiReady) return;
        if (dealButtons == null) return;

        // If deals is null or too short, still show placeholder text without crashing
        for (int i = 0; i < dealButtons.Count; i++)
        {
            var btn = dealButtons[i];
            if (btn == null) continue;

            // ensure clean slate
            btn.Clear();

            VisualElement posDealBox = new VisualElement();
            VisualElement negDealBox = new VisualElement();
            posDealBox.AddToClassList("CardTextBox");
            negDealBox.AddToClassList("CardTextBox");

            // Deal curDeal = (deals != null && i < deals.Count) ? deals[i] : null;

            Label l1 = new Label();
            Label l2 = new Label();

            l1.AddToClassList("BuffTitle");
            l1.text = "\nEscape this place by \nmaking deals with me.";
            posDealBox.Add(l1);

            l2.AddToClassList("DebuffTitle");
            l2.text = "A cost, of course.";
            negDealBox.Add(l2);

            btn.Add(posDealBox);
            btn.Add(negDealBox);
        }
    }
}