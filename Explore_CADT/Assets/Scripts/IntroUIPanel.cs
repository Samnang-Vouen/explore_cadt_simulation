using UnityEngine;
using UnityEngine.UI;

public class IntroUIPanel : MonoBehaviour
{
    private GameObject creditsPanel;
    private Text creditsText;
    private bool creditsActive = false;

    private GameObject player;
    private Behaviour playerController;
    private Behaviour playerInput;

    private GameObject panel;
    // private Button startButton;
    private Button quitButton;

    void Start()
    {
        // Create Canvas
        GameObject canvasGO = new GameObject("IntroUICanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        canvasGO.AddComponent<GraphicRaycaster>();

        // Create Credits Panel (hidden by default)
        creditsPanel = new GameObject("CreditsPanel");
        creditsPanel.transform.SetParent(canvasGO.transform, false);
        Image creditsImage = creditsPanel.AddComponent<Image>();
        creditsImage.color = new Color(0.08f, 0.08f, 0.12f, 0.92f); // Softer dark background
        creditsImage.raycastTarget = true;
        // Add rounded corners and border if using Unity 2022+ (UI Toolkit), else skip
        // For classic UI, you can use a 9-sliced sprite for rounded corners if available
        RectTransform creditsRect = creditsPanel.GetComponent<RectTransform>();
        creditsRect.anchorMin = new Vector2(0.15f, 0.12f);
        creditsRect.anchorMax = new Vector2(0.85f, 0.88f);
        creditsRect.offsetMin = Vector2.zero;
        creditsRect.offsetMax = Vector2.zero;

        // Credits Text (with padding and line spacing)
        GameObject creditsTextGO = new GameObject("CreditsText");
        creditsTextGO.transform.SetParent(creditsPanel.transform, false);
        creditsText = creditsTextGO.AddComponent<Text>();
        creditsText.supportRichText = true;
        creditsText.text = "<b><size=32>Lecturer</size></b>\nDr. Va Hongly\n\n<b><size=28>Team Members</size></b>\nVouen Samnang\nVorn NaraTheany\nKhen Chandarapisey\n\n<size=22>Press <b>Y</b> to Confirm Quit\nPress <b>N</b> to Continue</size>";
        creditsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        creditsText.resizeTextForBestFit = true;
        creditsText.resizeTextMinSize = 18;
        creditsText.resizeTextMaxSize = 40;
        creditsText.color = new Color(1f, 1f, 1f, 0.97f);
        creditsText.alignment = TextAnchor.MiddleCenter;
        creditsText.lineSpacing = 1.25f;
        RectTransform creditsTextRect = creditsText.GetComponent<RectTransform>();
        creditsTextRect.anchorMin = new Vector2(0.08f, 0.08f);
        creditsTextRect.anchorMax = new Vector2(0.92f, 0.92f);
        creditsTextRect.offsetMin = new Vector2(16, 16);
        creditsTextRect.offsetMax = new Vector2(-16, -16);
        creditsPanel.SetActive(false);

        // Try to find the player GameObject by common names or tag
        player = GameObject.Find("Player");
        if (player == null)
            player = GameObject.Find("FirstPersonController");
        if (player == null)
        {
            GameObject[] playersByTag = GameObject.FindGameObjectsWithTag("Player");
            if (playersByTag.Length > 0)
                player = playersByTag[0];
        }
        if (player != null)
        {
            // Try to get the First Person Controller script (Starter Assets)
            playerController = player.GetComponent("StarterAssets.FirstPersonController") as Behaviour;
            // Try to get the PlayerInput script (Starter Assets)
            playerInput = player.GetComponent("UnityEngine.InputSystem.PlayerInput") as Behaviour;
            if (playerController != null) playerController.enabled = false;
            if (playerInput != null) playerInput.enabled = false;
        }

        // Create Panel
        panel = new GameObject("Panel");
        panel.transform.SetParent(canvasGO.transform, false);
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.25f, 0.3f);
        panelRect.anchorMax = new Vector2(0.75f, 0.7f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        // Create Message Text
        GameObject textGO = new GameObject("MessageText");
        textGO.transform.SetParent(panel.transform, false);
        Text messageText = textGO.AddComponent<Text>();
        messageText.text = "This is the IDT Building. It has 24 rooms and 12 bathrooms for both men and women.\n\nPress Space to Start Exploring.";
        messageText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        messageText.resizeTextForBestFit = true;
        messageText.resizeTextMinSize = 16;
        messageText.resizeTextMaxSize = 36;
        messageText.color = Color.white;
        messageText.alignment = TextAnchor.MiddleCenter;
        RectTransform textRect = messageText.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.1f, 0.4f);
        textRect.anchorMax = new Vector2(0.9f, 0.9f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // Add 'Press Q to Quit' text always visible on screen (on canvas root)
        GameObject quitTextGO = new GameObject("QuitText");
        quitTextGO.transform.SetParent(canvasGO.transform, false);
        Text quitText = quitTextGO.AddComponent<Text>();
        quitText.text = "Press Q to Quit";
        quitText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        quitText.resizeTextForBestFit = true;
        quitText.resizeTextMinSize = 12;
        quitText.resizeTextMaxSize = 28;
        quitText.color = Color.white;
        quitText.alignment = TextAnchor.LowerRight;
        RectTransform quitTextRect = quitText.GetComponent<RectTransform>();
        quitTextRect.anchorMin = new Vector2(0.8f, 0.01f);
        quitTextRect.anchorMax = new Vector2(0.99f, 0.08f);
        quitTextRect.offsetMin = Vector2.zero;
        quitTextRect.offsetMax = Vector2.zero;
    }

    void Update()
    {
        // Only allow starting if the panel is active
        if (panel != null && panel.activeSelf && !creditsActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartExploring();
            }
        }

        // Open credits/quit panel with Q (if not already open)
        if (!creditsActive && Input.GetKeyDown(KeyCode.Q))
        {
            ShowCreditsPanel();
        }

        // Handle credits panel controls
        if (creditsActive)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            if (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Escape))
            {
                HideCreditsPanel();
            }
        }
        void ShowCreditsPanel()
        {
            creditsPanel.SetActive(true);
            creditsActive = true;
            // Optionally, lock player controls while credits are open
            if (playerController != null) playerController.enabled = false;
            if (playerInput != null) playerInput.enabled = false;
        }

        void HideCreditsPanel()
        {
            creditsPanel.SetActive(false);
            creditsActive = false;
            // Optionally, re-enable player controls
            if (panel != null && !panel.activeSelf)
            {
                if (playerController != null) playerController.enabled = true;
                if (playerInput != null) playerInput.enabled = true;
            }
        }
    }

    GameObject CreateButton(string name, Transform parent, string buttonText, Vector2 anchorCenter)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);

        Button button = btnGO.AddComponent<Button>();
        Image image = btnGO.AddComponent<Image>();
        image.color = new Color(1, 1, 1, 0.9f);

        RectTransform rect = btnGO.GetComponent<RectTransform>();
        rect.anchorMin = anchorCenter - new Vector2(0.1f, 0.08f);
        rect.anchorMax = anchorCenter + new Vector2(0.1f, 0.08f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        GameObject txtGO = new GameObject("Text");
        txtGO.transform.SetParent(btnGO.transform, false);
        Text txt = txtGO.AddComponent<Text>();
        txt.text = buttonText;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontSize = 22;
        txt.color = Color.black;
        txt.alignment = TextAnchor.MiddleCenter;
        RectTransform txtRect = txt.GetComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;

        return btnGO;
    }


    void StartExploring()
    {
        panel.SetActive(false);
        // Enable player controls
        if (playerController != null) playerController.enabled = true;
        if (playerInput != null) playerInput.enabled = true;
    }

    void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
