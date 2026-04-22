using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 
using System.Collections;

public class GameManager : MonoBehaviour
{
    private float survivalTime;
    private bool isGameOver;
    public bool IsGameOver => isGameOver;

    [Header("Estado de Juego")]
    public GameMode currentMode = GameMode.Solo;
    public TurnState currentTurn = TurnState.Player1;
    public GameState currentState = GameState.Menu;

    [Header("Resultados")]
    public float p1Time;
    public float p2Time;
    public int currentKills;

    [Header("Referencias UI")]
    public GameObject resultsPanel;
    public CanvasGroup resultsCanvasGroup; 
    public GameObject deathFlashOverlay; 
    
    public TextMeshProUGUI p1TimeText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI timerHUDText; 
    public CanvasGroup hudGroup;         
    public TextMeshProUGUI killsHUDText; 
    public Button retryButton;          
    public Button menuButton;            

    public static GameManager Instance; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void CleanupScene()
    {
        try {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies) Destroy(enemy);
        } catch { }

        try {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets) Destroy(bullet);
        } catch { }
    }

    private IEnumerator DeathSequenceCoroutine()
    {
        currentState = GameState.DeathTransition;
        Time.timeScale = 0f; 
        
        Debug.Log("<color=red><b>[GameManager]</b> Secuencia de muerte iniciada.</color>");

        // OCULTAR HUD
        if (hudGroup != null && UIAnimationManager.Instance != null)
        {
            StartCoroutine(UIAnimationManager.Instance.FadeCanvasGroup(hudGroup, 1f, 0f, 0.2f));
        }

        if (deathFlashOverlay != null)
        {
            deathFlashOverlay.SetActive(true);
            CanvasGroup flashGroup = deathFlashOverlay.GetComponent<CanvasGroup>();
            if (flashGroup != null && UIAnimationManager.Instance != null)
            {
                yield return UIAnimationManager.Instance.FadeCanvasGroup(flashGroup, 1f, 0f, 0.2f);
            }
            deathFlashOverlay.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(0.5f);
        
        CleanupScene();
        
        currentState = GameState.GameOver;
        ShowResults();
    }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "SampleScene" && currentState == GameState.Menu)
        {
            currentState = GameState.Playing;
            currentMode = GameMode.Solo;
        }

        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnMatchStarted += HandleOnlineStart;
            NetworkManager.Instance.OnGameOver += HandleOnlineGameOver;
        }
    }

    void Update()
    {
        if (currentState == GameState.Playing && !isGameOver)
        {
            survivalTime += Time.unscaledDeltaTime; 
            
            if (timerHUDText != null) 
            {
                timerHUDText.text = survivalTime.ToString("F2") + "s";
                timerHUDText.color = Color.white;
            }

            if (killsHUDText != null)
            {
                killsHUDText.text = "BAJAS: " + currentKills;
            }
        }
    }

    public void StartGame(GameMode mode)
    {
        currentMode = mode;
        if (mode == GameMode.Online) return;

        currentTurn = TurnState.Player1;
        currentState = GameState.Playing; 
        p1Time = p2Time = survivalTime = 0;
        isGameOver = false;
        
        ResetSession();
        SceneManager.LoadScene("SampleScene");
    }

    private void HandleOnlineStart()
    {
        currentMode = GameMode.Online;
        currentState = GameState.Playing;
        p1Time = p2Time = survivalTime = 0;
        isGameOver = false;
        SceneManager.LoadScene("SampleScene");
    }

    public void ProcessDeath()
    {
        if (currentState != GameState.Playing || isGameOver) return;
        isGameOver = true;
        
        if (currentMode == GameMode.Online)
        {
            // Requisito: Enviar resultados vía HTTP (UnityWebRequest)
            NetworkManager.Instance.ReportResults(survivalTime);
            
            // Sincronizar fin de partida vía Socket para otros jugadores
            NetworkManager.Instance.Emit("player_death", new DeathData { roomId = NetworkManager.Instance.currentRoomId, survivalTime = survivalTime });
        }
        else 
        {
            p1Time = survivalTime;
            StartCoroutine(DeathSequenceCoroutine());
        }
    }

    private void HandleOnlineGameOver(string winnerId, float time)
    {
        isGameOver = true;
        p1Time = time;
        StartCoroutine(DeathSequenceCoroutine());
    }

    public void RegisterResultsUI(ResultsUIRegisterer ui)
    {
        if (ui == null || ui.resultsPanel == null) return;

        resultsPanel = ui.resultsPanel;
        if (ui.resultsPanel != null) resultsCanvasGroup = ui.resultsPanel.GetComponent<CanvasGroup>();
        deathFlashOverlay = ui.deathFlashOverlay;
        
        p1TimeText = ui.p1TimeText;
        titleText = ui.titleText;
        winnerText = ui.winnerText;
        killsText = ui.killsText;
        timerHUDText = ui.timerHUDText;
        hudGroup = ui.hudGroup;
        killsHUDText = ui.killsHUDText;
        retryButton = ui.retryButton;
        menuButton = ui.menuButton;

        if (retryButton != null) 
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(RetryGame);
            AddHoverEffect(retryButton.gameObject);
        }
        if (menuButton != null) 
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(ReturnToMenu);
            AddHoverEffect(menuButton.gameObject);
        }
        Debug.Log("<b>[GameManager]</b> UI de resultados registrada correctamente.");
    }

    private void AddHoverEffect(GameObject go)
    {
        if (go.GetComponent<ButtonHoverEffect>() == null) go.AddComponent<ButtonHoverEffect>();
    }

    private void ShowResults()
    {
        Debug.Log("<b>[GameManager]</b> Intentando mostrar resultados...");
        
        if (resultsPanel == null)
        {
            Debug.LogWarning("<b>[GameManager]</b> Panel ausente. Iniciando búsqueda de emergencia...");
            FindResultsPanelExhaustive();
        }

        if (resultsPanel != null)
        {
            // ASEGURAR QUE EL CANVAS PADRE ESTÉ ACTIVO Y VISIBLE
            Canvas parentCanvas = resultsPanel.GetComponentInParent<Canvas>(true);
            if (parentCanvas != null) 
            {
                parentCanvas.gameObject.SetActive(true);
                parentCanvas.gameObject.transform.localScale = Vector3.one;
            }
            resultsPanel.transform.localScale = Vector3.one;

            StartCoroutine(ShowResultsSequence());
        }
        else
        {
            Debug.LogError("<b>[GameManager]</b> ERROR CRÍTICO: No existe el objeto 'PanelResultados' en la escena.");
        }
    }

    private void FindResultsPanelExhaustive()
    {
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in allObjects)
        {
            if (go.name == "PanelResultados")
            {
                resultsPanel = go;
                resultsPanel.transform.localScale = Vector3.one; 
                resultsCanvasGroup = go.GetComponent<CanvasGroup>() ?? go.AddComponent<CanvasGroup>();
                
                // Re-vincular componentes hijos por nombre
                p1TimeText = go.transform.Find("P1TimeText")?.GetComponent<TextMeshProUGUI>();
                titleText = go.transform.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
                winnerText = go.transform.Find("WinnerText")?.GetComponent<TextMeshProUGUI>();
                killsText = go.transform.Find("KillsText")?.GetComponent<TextMeshProUGUI>();
                
                retryButton = go.transform.Find("RetryButton")?.GetComponent<Button>();
                menuButton = go.transform.Find("MenuButton")?.GetComponent<Button>();

                if (retryButton != null) {
                    retryButton.onClick.RemoveAllListeners();
                    retryButton.onClick.AddListener(RetryGame);
                    AddHoverEffect(retryButton.gameObject);
                }
                if (menuButton != null) {
                    menuButton.onClick.RemoveAllListeners();
                    menuButton.onClick.AddListener(ReturnToMenu);
                    AddHoverEffect(menuButton.gameObject);
                }
                Debug.Log("<b>[GameManager]</b> UI auto-sanada con éxito.");
                return;
            }
        }
    }

    private IEnumerator ShowResultsSequence()
    {
        resultsPanel.SetActive(true);
        
        // 3.1 Asignación inmediata para evitar valores en cero visibles
        if (titleText != null) titleText.text = "PARTIDA FINALIZADA";
        if (p1TimeText != null) p1TimeText.text = $"Resultado: {p1Time:F2}s";
        if (killsText != null) killsText.text = $"Bajas: {currentKills}";
        
        if (winnerText != null) winnerText.gameObject.SetActive(false);

        Debug.Log("<b>[GameManager]</b> PanelResultados activado.");

        if (UIAnimationManager.Instance == null)
        {
            if (resultsCanvasGroup != null) resultsCanvasGroup.alpha = 1f;
        }
        else
        {
            if (resultsCanvasGroup != null) 
                yield return UIAnimationManager.Instance.FadeCanvasGroup(resultsCanvasGroup, 0f, 1f, 0.5f);
        }

        // 3.2 Animaciones en paralelo
        if (UIAnimationManager.Instance != null)
        {
            if (killsText != null)
            {
                StartCoroutine(UIAnimationManager.Instance.CountText(killsText, 0, currentKills, 0.5f, "Bajas: ", "", "F0"));
                StartCoroutine(UIAnimationManager.Instance.PulseScale(killsText.transform, 1.1f, 0.5f));
            }

            if (p1TimeText != null)
            {
                StartCoroutine(UIAnimationManager.Instance.CountText(p1TimeText, 0, p1Time, 0.8f, "Resultado: ", "s"));
                StartCoroutine(UIAnimationManager.Instance.PulseScale(p1TimeText.transform, 1.1f, 0.8f));
            }
            
            yield return new WaitForSecondsRealtime(0.8f);
        }
    }

    public void AddKill() 
    { 
        currentKills++; 
        if (killsHUDText != null && UIAnimationManager.Instance != null)
        {
            StartCoroutine(UIAnimationManager.Instance.PulseScale(killsHUDText.transform, 1.2f, 0.15f));
        }
    }

    private void ResetSession()
    {
        survivalTime = 0;
        currentKills = 0;
        isGameOver = false;
        Time.timeScale = 1.0f;
        if (resultsPanel != null) resultsPanel.SetActive(false);
        if (hudGroup != null) hudGroup.alpha = 1f;
    }

    public void RetryGame() { StartGame(currentMode); }

    public void ReturnToMenu() 
    { 
        StartCoroutine(ReturnToMenuRoutine());
    }

    private IEnumerator ReturnToMenuRoutine()
    {
        Debug.Log("<b>[GameManager]</b> Regresando al menú...");
        
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.LeaveRoom();
        }

        // Un pequeño respiro para asegurar el envío del paquete (opcional pero recomendado)
        yield return new WaitForSecondsRealtime(0.1f);
        
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu"); 
    }

    [System.Serializable]
    public class DeathData { public string roomId; public float survivalTime; }
}
