using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 

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

    [Header("Referencias UI")]
    public GameObject resultsPanel;
    public TextMeshProUGUI p1TimeText;
    public TextMeshProUGUI p2TimeText;
    public TextMeshProUGUI winnerText;
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
        }
        else
        {
            Destroy(gameObject); 
        }
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
            currentState = GameState.GameOver; 
            ShowResults();
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
                currentState = GameState.GameOver; 
                ShowResults();
            }
        }
    }

    private void HandleOnlineGameOver(string winnerId, float time)
    {
        isGameOver = true;
        currentState = GameState.GameOver;
        p1Time = time; // Para simplificar, mostramos el tiempo del ganador
        ShowResults();
    }

    public void RegisterResultsUI(ResultsUIRegisterer ui)
    {
        resultsPanel = ui.resultsPanel;
        p1TimeText = ui.p1TimeText;
        p2TimeText = ui.p2TimeText;
        winnerText = ui.winnerText;
        timerHUDText = ui.timerHUDText;
        retryButton = ui.retryButton;
        menuButton = ui.menuButton;

        if (retryButton != null) retryButton.onClick.AddListener(RetryGame);
        if (menuButton != null) menuButton.onClick.AddListener(ReturnToMenu);
    }

    private void ShowResults()
    {
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
            if (currentMode == GameMode.Solo || currentMode == GameMode.Online)
            {
                if (p1TimeText != null) p1TimeText.text = "Resultado: " + p1Time.ToString("F2") + "s";
                if (p2TimeText != null) p2TimeText.gameObject.SetActive(false);
            }
            else
            {
                p1TimeText.text = "P1: " + p1Time.ToString("F2") + "s";
                p2TimeText.text = "P2: " + p2Time.ToString("F2") + "s";
                winnerText.text = p1Time > p2Time ? "GANA P1" : "GANA P2";
            }
        }
    }

    private void ResetSession()
    {
        survivalTime = 0;
        isGameOver = false;
        if (currentState != GameState.GameOver) currentState = GameState.Playing;
        if (resultsPanel != null) resultsPanel.SetActive(false);
    }

    public void RetryGame() { StartGame(currentMode); }
    public void ReturnToMenu() { SceneManager.LoadScene("Menu"); }

    [System.Serializable]
    public class DeathData { public string roomId; public float survivalTime; }
}
