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
    private float bestTime;
    private bool isNewRecord;
    private const string BestTimeKey = "BestTime_Solo";

    [Header("Referencias UI")]
    public GameObject resultsPanel;
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

    private void LoadHighScore()
    {
        bestTime = PlayerPrefs.GetFloat(BestTimeKey, 0f);
    }

    private void SaveHighScore()
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) Destroy(enemy);

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets) Destroy(bullet);
        
        Debug.Log("<b>[GameManager]</b> Escena limpia: Enemigos y balas eliminados.");
    }

    private IEnumerator DeathSequenceCoroutine()
    {
        currentState = GameState.DeathTransition;
        Time.timeScale = 0.3f;
        
        Debug.Log("<b>[GameManager]</b> Iniciando secuencia de muerte (Slow-mo 0.3x)...");
        
        yield return new WaitForSecondsRealtime(1.5f);
        
        Time.timeScale = 1.0f;
        CleanupScene();
        SaveHighScore();
        
        currentState = GameState.GameOver;
        ShowResults();
        
        Debug.Log("<b>[GameManager]</b> Secuencia de muerte completada. Mostrando resultados.");
    }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        // "Safe Start" para pruebas en el Editor
        if (sceneName == "SampleScene" && currentState == GameState.Menu)
        {
            Debug.Log("<color=orange><b>[GameManager]</b> Detectada SampleScene. Cambiando estado a Playing.</color>");
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
            survivalTime += Time.deltaTime;
            if (timerHUDText != null) 
            {
                timerHUDText.text = "Tiempo: " + survivalTime.ToString("F2") + "s";
            }
        }
    }

    public void StartGame(GameMode mode)
    {
        currentMode = mode;
        if (mode == GameMode.Online)
        {
            // El inicio real vendrá del servidor
            return;
        }

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
            NetworkManager.Instance.Emit("player_death", new DeathData {
                roomId = NetworkManager.Instance.currentRoomId,
                survivalTime = survivalTime
            });
        }
        else if (currentMode == GameMode.Solo)
        {
            p1Time = survivalTime;
            StartCoroutine(DeathSequenceCoroutine());
        }
        else // Local Multiplayer
        {
            if (currentTurn == TurnState.Player1)
            {
                p1Time = survivalTime;
                currentTurn = TurnState.Player2;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                p2Time = survivalTime;
                StartCoroutine(DeathSequenceCoroutine());
            }
        }
    }

    private void HandleOnlineGameOver(string winnerId, float time)
    {
        isGameOver = true;
        p1Time = time; // Para simplificar, mostramos el tiempo del ganador
        StartCoroutine(DeathSequenceCoroutine());
    }

    public void RegisterResultsUI(ResultsUIRegisterer ui)
    {
        resultsPanel = ui.resultsPanel;
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
        }
        if (menuButton != null) 
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(ReturnToMenu);
        }
    }

    private void ShowResults()
    {
        // Auto-búsqueda de emergencia si la referencia se perdió o no se registró
        if (resultsPanel == null)
        {
            Debug.Log("<b>[GameManager]</b> Intentando auto-localizar Panel de Resultados...");
            GameObject canvas = GameObject.Find("CanvasResultados");
            if (canvas != null)
            {
                // Aseguramos que el Canvas sea visible y tenga escala correcta
                canvas.SetActive(true);
                canvas.transform.localScale = Vector3.one;

                Canvas cComp = canvas.GetComponent<Canvas>();
                if (cComp != null) cComp.enabled = true;

                // Buscamos el panel dentro del canvas (incluyendo desactivados)
                foreach (Transform child in canvas.GetComponentsInChildren<Transform>(true))
                {
                    if (child.name == "PanelResultados")
                    {
                        resultsPanel = child.gameObject;
                        resultsPanel.transform.localScale = Vector3.one; // Forzar escala 1
                        Debug.Log($"<color=green><b>[GameManager]</b> ¡Panel detectado con éxito! en {child.name}</color>");
                        
                        p1TimeText = child.Find("P1TimeText")?.GetComponent<TextMeshProUGUI>();
                        p2TimeText = child.Find("P2TimeText")?.GetComponent<TextMeshProUGUI>();
                        winnerText = child.Find("WinnerText")?.GetComponent<TextMeshProUGUI>();
                        bestTimeText = child.Find("BestTimeText")?.GetComponent<TextMeshProUGUI>();
                        killsText = child.Find("KillsText")?.GetComponent<TextMeshProUGUI>();
                        newRecordBadge = child.Find("NewRecordBadge")?.gameObject;
                        
                        // Y los botones
                        retryButton = child.Find("RetryButton")?.GetComponent<Button>();
                        menuButton = child.Find("MenuButton")?.GetComponent<Button>();
                        
                        if (retryButton != null) {
                            retryButton.onClick.RemoveAllListeners();
                            retryButton.onClick.AddListener(RetryGame);
                        }
                        if (menuButton != null) {
                            menuButton.onClick.RemoveAllListeners();
                            menuButton.onClick.AddListener(ReturnToMenu);
                        }
                        break;
                    }
                }
            }
        }

        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
            
            // Mostrar/Ocultar insignia de récord
            if (newRecordBadge != null) newRecordBadge.SetActive(isNewRecord);
            
            // Mostrar mejor tiempo
            if (bestTimeText != null) bestTimeText.text = $"Mejor: {bestTime:F2}s";

            // Mostrar bajas
            if (killsText != null) killsText.text = $"Bajas: {currentKills}";

            if (currentMode == GameMode.Solo || currentMode == GameMode.Online)
            {
                if (p1TimeText != null) 
                    p1TimeText.text = $"Resultado: {p1Time:F2}s";
                
                if (p2TimeText != null) p2TimeText.gameObject.SetActive(false);
            }
            else
            {
                if (p1TimeText != null) p1TimeText.text = $"P1: {p1Time:F2}s";
                if (p2TimeText != null) p2TimeText.text = $"P2: {p2Time:F2}s";
                if (winnerText != null) winnerText.text = p1Time > p2Time ? "GANA P1" : "GANA P2";
                if (killsText != null) killsText.gameObject.SetActive(false); // Ocultar bajas en multi local por ahora
            }
        }
        else
        {
            Debug.LogError("<color=red><b>[GameManager]</b> ¡CRÍTICO! No se encontró 'PanelResultados' en la escena. Asegúrate de que exista un objeto llamado exactamente así dentro de 'CanvasResultados'.</color>");
        }
    }

    public void AddKill()
    {
        currentKills++;
        Debug.Log($"<b>[GameManager]</b> Baja registrada. Total: {currentKills}");
    }

    private void ResetSession()
    {
        survivalTime = 0;
        currentKills = 0;
        isGameOver = false;
        Time.timeScale = 1.0f;
        if (currentState != GameState.GameOver) currentState = GameState.Playing;
        if (resultsPanel != null) resultsPanel.SetActive(false);
    }

    public void RetryGame() { StartGame(currentMode); }
    public void ReturnToMenu() { SceneManager.LoadScene("Menu"); }

    [System.Serializable]
    public class DeathData { public string roomId; public float survivalTime; }
}
