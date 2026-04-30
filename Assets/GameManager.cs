using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 
using System.Collections;
using Unity.Netcode;
using AEA.Networking;

public class GameManager : NetworkBehaviour
{
    private NetworkVariable<float> survivalTime = new NetworkVariable<float>(0f);
    private NetworkVariable<float> difficultyMultiplier = new NetworkVariable<float>(1.0f);
    
    private bool isGameOver;
    public bool IsGameOver => isGameOver;
    private bool matchEndedForLocal;

    [Header("Estado de Juego")]
    public GameMode currentMode = GameMode.Solo;
    public TurnState currentTurn = TurnState.Player1;
    public GameState currentState = GameState.Menu;

    public float p1Time;
    public float p2Time;
    public int currentKills;
    private System.Collections.Generic.Dictionary<string, float> playerDeathTimes = new System.Collections.Generic.Dictionary<string, float>();

    [Header("Referencias UI")]
    public GameObject resultsPanel;
    public CanvasGroup resultsCanvasGroup; 
    public GameObject deathFlashOverlay; 
    
    public TextMeshProUGUI p1TimeText;
    public TextMeshProUGUI p2TimeText; 
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI timerHUDText; 
    public TextMeshProUGUI rivalTimerHUDText; 
    public CanvasGroup hudGroup;         
    public TextMeshProUGUI killsHUDText; 
    public Button retryButton;          
    public Button menuButton;            

    public float difficultyMultiplierValue => difficultyMultiplier.Value;

    public static GameManager Instance; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerDeathTimes.Clear();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            survivalTime.Value = 0f;
            difficultyMultiplier.Value = 1.0f;
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

        try {
            GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            foreach (GameObject bullet in enemyBullets) Destroy(bullet);
        } catch { }
    }

    private IEnumerator DeathSequenceCoroutine()
    {
        currentState = GameState.DeathTransition;
        Time.timeScale = 0f; 
        
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
    }

    void Update()
    {
        if (IsServer && currentState == GameState.Playing && !isGameOver)
        {
            survivalTime.Value += Time.unscaledDeltaTime; 
            difficultyMultiplier.Value += Time.unscaledDeltaTime * 0.01f;
        }

        if (currentState == GameState.Playing && !isGameOver && !matchEndedForLocal)
        {
            if (timerHUDText != null) 
            {
                timerHUDText.text = survivalTime.Value.ToString("F2") + "s";
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
        p1Time = p2Time = 0;
        isGameOver = false;
        
        ResetSession();
        SceneManager.LoadScene("SampleScene");
    }

    public void ProcessDeath()
    {
        if (currentState != GameState.Playing || isGameOver) return;
        
        if (currentMode == GameMode.Online)
        {
            matchEndedForLocal = true;
            p1Time = survivalTime.Value;
            
            // Notificar muerte a través de RPC (Nuevo formato Unity 6)
            NotifyDeathServerRpc(Unity.Netcode.NetworkManager.Singleton.LocalClientId, p1Time);

            if (timerHUDText != null) 
            {
                timerHUDText.text = "ESPERANDO AL RIVAL...";
                timerHUDText.color = new Color(1, 1, 1, 0.5f);
            }
        }
        else 
        {
            isGameOver = true;
            p1Time = survivalTime.Value;
            StartCoroutine(DeathSequenceCoroutine());
        }
    }

    [Rpc(SendTo.Server)]
    private void NotifyDeathServerRpc(ulong clientId, float time)
    {
        NotifyDeathClientRpc(clientId, time);
    }

    [Rpc(SendTo.Everyone)]
    private void NotifyDeathClientRpc(ulong clientId, float time)
    {
        string idStr = clientId.ToString();
        if (!playerDeathTimes.ContainsKey(idStr))
        {
            playerDeathTimes[idStr] = time;
            if (clientId != Unity.Netcode.NetworkManager.Singleton.LocalClientId)
            {
                p2Time = time;
                if (timerHUDText != null) timerHUDText.text = "¡RIVAL HA CAÍDO!";
            }
            CheckAllPlayersDead();
        }
    }

    private void CheckAllPlayersDead()
    {
        int targetCount = (Unity.Netcode.NetworkManager.Singleton != null) ? Unity.Netcode.NetworkManager.Singleton.ConnectedClients.Count : 1;

        if (currentMode == GameMode.Online)
        {
            if (playerDeathTimes.Count >= targetCount)
            {
                isGameOver = true;
                
                // Si soy el host, reportar resultados finales a la API
                if (IsServer && AEA.Networking.NetworkManager.Instance != null)
                {
                    AEA.Networking.NetworkManager.Instance.ReportResults(p1Time); 
                }

                StartCoroutine(DeathSequenceCoroutine());
            }
        }
        else
        {
            isGameOver = true;
            StartCoroutine(DeathSequenceCoroutine());
        }
    }

    public void RegisterResultsUI(ResultsUIRegisterer ui)
    {
        if (ui == null || ui.resultsPanel == null) return;

        resultsPanel = ui.resultsPanel;
        if (ui.resultsPanel != null) resultsCanvasGroup = ui.resultsPanel.GetComponent<CanvasGroup>();
        deathFlashOverlay = ui.deathFlashOverlay;
        
        p1TimeText = ui.p1TimeText;
        p2TimeText = ui.p2TimeText; 
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
        }
        if (menuButton != null) 
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(ReturnToMenu);
        }
    }

    private void ShowResults()
    {
        if (resultsPanel != null)
        {
            Canvas parentCanvas = resultsPanel.GetComponentInParent<Canvas>(true);
            if (parentCanvas != null) parentCanvas.gameObject.SetActive(true);
            resultsPanel.SetActive(true);
            StartCoroutine(ShowResultsSequence());
        }
    }

    private IEnumerator ShowResultsSequence()
    {
        if (currentMode == GameMode.Online)
        {
            bool victory = p1Time >= p2Time;
            if (titleText != null) 
            {
                titleText.text = victory ? "¡VICTORIA!" : "¡DERROTA!";
                titleText.color = victory ? Color.green : Color.red;
            }
            if (winnerText != null) 
            {
                winnerText.gameObject.SetActive(true);
                winnerText.text = victory ? "Has sobrevivido más que tu rival" : "Tu rival te ha superado";
            }
        }
        else
        {
            if (titleText != null) { titleText.text = "PARTIDA FINALIZADA"; titleText.color = Color.white; }
            if (winnerText != null) winnerText.gameObject.SetActive(false);
        }

        if (p1TimeText != null) p1TimeText.text = $"Tú: {p1Time:F2}s";
        if (p2TimeText != null) 
        {
            p2TimeText.gameObject.SetActive(currentMode == GameMode.Online);
            p2TimeText.text = $"Rival: {p2Time:F2}s";
        }
        if (killsText != null) killsText.text = $"Bajas: {currentKills}";
        
        if (resultsCanvasGroup != null) resultsCanvasGroup.alpha = 1f;
        yield return null;
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
        StopAllCoroutines();
        isGameOver = false;
        Time.timeScale = 1.0f;
        if (resultsPanel != null) resultsPanel.SetActive(false);
        if (hudGroup != null) hudGroup.alpha = 1f;
        matchEndedForLocal = false;
        playerDeathTimes.Clear();
        currentKills = 0;
    }

    public void RetryGame() { StartGame(currentMode); }

    public void ReturnToMenu() 
    { 
        if (AEA.Networking.NetworkManager.Instance != null) AEA.Networking.NetworkManager.Instance.LeaveRoom();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu"); 
    }
}
