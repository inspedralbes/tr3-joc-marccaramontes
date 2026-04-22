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
    public float bestTime;
    public bool isNewRecord;
    private const string BestTimeKey = "BestTime_Solo";

    [Header("Referencias UI")]
    public GameObject resultsPanel;
    public CanvasGroup resultsCanvasGroup; 
    public GameObject deathFlashOverlay; 
    
    public TextMeshProUGUI p1TimeText;
    public TextMeshProUGUI p2TimeText;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI killsText;
    public GameObject newRecordBadge;
    public TextMeshProUGUI timerHUDText; 
    public Button retryButton;          
    public Button menuButton;            

    public static GameManager Instance; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void LoadHighScore()
    {
        bestTime = PlayerPrefs.GetFloat(BestTimeKey, 0f);
    }

    public void SaveHighScore()
    {
        if (survivalTime > bestTime)
        {
            bestTime = survivalTime;
            PlayerPrefs.SetFloat(BestTimeKey, bestTime);
            PlayerPrefs.Save();
            isNewRecord = true;
        }
        else
        {
            isNewRecord = false;
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
        SaveHighScore();
        
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
                timerHUDText.text = "Tiempo: " + survivalTime.ToString("F2") + "s";
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
        resultsPanel = ui.resultsPanel;
        if (ui.resultsPanel != null) resultsCanvasGroup = ui.resultsPanel.GetComponent<CanvasGroup>();
        deathFlashOverlay = ui.deathFlashOverlay;
        
        p1TimeText = ui.p1TimeText;
        p2TimeText = ui.p2TimeText;
        winnerText = ui.winnerText;
        bestTimeText = ui.bestTimeText;
        killsText = ui.killsText;
        newRecordBadge = ui.newRecordBadge;
        timerHUDText = ui.timerHUDText;
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
                p2TimeText = go.transform.Find("P2TimeText")?.GetComponent<TextMeshProUGUI>();
                winnerText = go.transform.Find("WinnerText")?.GetComponent<TextMeshProUGUI>();
                killsText = go.transform.Find("KillsText")?.GetComponent<TextMeshProUGUI>();
                bestTimeText = go.transform.Find("BestTimeText")?.GetComponent<TextMeshProUGUI>();
                newRecordBadge = go.transform.Find("NewRecordBadge")?.gameObject;
                
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

        if (newRecordBadge != null) newRecordBadge.SetActive(isNewRecord);
        if (bestTimeText != null) bestTimeText.text = $"Mejor: {bestTime:F2}s";

        if (killsText != null) 
        {
            if (UIAnimationManager.Instance != null)
                yield return UIAnimationManager.Instance.CountText(killsText, 0, currentKills, 0.5f, "Bajas: ", "", "F0");
            else
                killsText.text = $"Bajas: {currentKills}";
        }

        if (p1TimeText != null) 
        {
            if (UIAnimationManager.Instance != null)
                yield return UIAnimationManager.Instance.CountText(p1TimeText, 0, p1Time, 0.8f, "Resultado: ", "s");
            else
                p1TimeText.text = $"Resultado: {p1Time:F2}s";
        }
    }

    public void AddKill() { currentKills++; }

    private void ResetSession()
    {
        survivalTime = 0;
        currentKills = 0;
        isGameOver = false;
        Time.timeScale = 1.0f;
        if (resultsPanel != null) resultsPanel.SetActive(false);
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
