using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LobbyController : MonoBehaviour
{
    [Header("UI - Main")]
    public CanvasGroup mainPanelGroup; 
    public TMP_InputField nameInputField;
    public TMP_InputField roomInputField;
    public TMP_InputField serverAddressInputField;
    public Button createBtn;
    public Button joinBtn;
    public Button backBtn;

    [Header("UI - Waiting")]
    public CanvasGroup waitingPanelGroup; 
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI roomCodeText;
    public Button startMatchBtn;
    public Button backToMainBtn; // Botón para volver al panel principal

    [Header("Discovery status")]
    public bool isUserTypingAddress = false;
    private float lastTypeTime = 0f;
    private const float typingGracePeriod = 5f;

    private Color originalStatusColor;

    private void Start()
    {
        if (createBtn != null) createBtn.onClick.AddListener(OnCreateRoom);
        if (joinBtn != null) joinBtn.onClick.AddListener(OnJoinRoom);
        if (backBtn != null) backBtn.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
        if (startMatchBtn != null) startMatchBtn.onClick.AddListener(OnStartMatch);
        if (backToMainBtn != null) backToMainBtn.onClick.AddListener(ReturnToMainPanel);

        // Inicialización de estados
        if (mainPanelGroup != null) { mainPanelGroup.alpha = 1; mainPanelGroup.interactable = true; mainPanelGroup.blocksRaycasts = true; }
        if (waitingPanelGroup != null) { waitingPanelGroup.alpha = 0; waitingPanelGroup.interactable = false; waitingPanelGroup.blocksRaycasts = false; }
        
        if (startMatchBtn != null) startMatchBtn.gameObject.SetActive(false);
        if (statusText != null) originalStatusColor = statusText.color;

        // Suscribirse a eventos del NetworkManager
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnMatchStarted += HandleMatchStarted;
            NetworkManager.Instance.OnLobbyPlayersUpdated += HandlePlayersUpdated;
        }
        else
        {
            Debug.LogWarning("<b>[LobbyController]</b> NetworkManager.Instance no encontrado. ¿Has pasado por la escena Menu?");
        }

        // Suscribirse a LAN Discovery
        if (LANDiscoveryManager.Instance != null)
        {
            LANDiscoveryManager.Instance.OnServerFound += HandleServerDiscovered;
            LANDiscoveryManager.Instance.StartListening();
            SetStatus("Buscando servidor local...", false);
        }

        // Cargar nombre previo si existe
        if (PlayerPrefs.HasKey("PlayerName") && nameInputField != null)
        {
            nameInputField.text = PlayerPrefs.GetString("PlayerName");
        }

        // Cargar dirección del servidor previa
        if (serverAddressInputField != null)
        {
            serverAddressInputField.text = PlayerPrefs.GetString("ServerAddress", "localhost");
            serverAddressInputField.onValueChanged.AddListener((val) => {
                if (NetworkManager.Instance != null) NetworkManager.Instance.UpdateServerAddress(val);
                isUserTypingAddress = true;
                lastTypeTime = Time.time;
            });
        }

        // Animación de entrada
        if (mainPanelGroup != null && UIAnimationManager.Instance != null)
            StartCoroutine(UIAnimationManager.Instance.FadeCanvasGroup(mainPanelGroup, 0, 1, 0.5f));
    }

    private void OnDestroy()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnMatchStarted -= HandleMatchStarted;
            NetworkManager.Instance.OnLobbyPlayersUpdated -= HandlePlayersUpdated;
        }

        if (LANDiscoveryManager.Instance != null)
        {
            LANDiscoveryManager.Instance.OnServerFound -= HandleServerDiscovered;
            LANDiscoveryManager.Instance.StopListening();
            LANDiscoveryManager.Instance.StopBroadcasting();
        }
    }

    private void HandleServerDiscovered(string ip)
    {
        // Si el usuario está escribiendo o ha escrito recientemente, no sobreescribimos
        if (isUserTypingAddress && (Time.time - lastTypeTime) < typingGracePeriod) return;

        if (serverAddressInputField != null && serverAddressInputField.text != ip)
        {
            serverAddressInputField.text = ip;
            if (NetworkManager.Instance != null) NetworkManager.Instance.UpdateServerAddress(ip);
            SetStatus($"Servidor detectado en {ip}", false);
            Debug.Log($"[Lobby] IP del servidor auto-rellenada: {ip}");
        }
    }

    private void HandlePlayersUpdated(string[] players)
    {
        if (statusText == null) return;
        
        string list = "Jugadores:\n";
        foreach (string p in players)
        {
            list += $"- {p}\n";
        }
        statusText.text = list;
    }

    private void OnCreateRoom()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName)) return;

        SavePlayerName(playerName);
        SaveServerAddress();
        SetStatus("Creando sala vía HTTP...", false);
        SwitchToWaitingPanel();

        var request = new NetworkManager.RoomRequest { playerName = playerName };
        NetworkManager.Instance.PostRequest<NetworkManager.RoomResponse>("/rooms/create", request, 
            (response) => {
                Debug.Log($"Sala creada: {response.roomId}");
                NetworkManager.Instance.ConnectToSocket(response.roomId);
                
                // Iniciar broadcast al ser el host
                if (LANDiscoveryManager.Instance != null)
                {
                    LANDiscoveryManager.Instance.StopListening(); // Dejar de escuchar si somos host
                    LANDiscoveryManager.Instance.StartBroadcasting();
                }

                if (UIAnimationManager.Instance != null && roomCodeText != null)
                    StartCoroutine(UIAnimationManager.Instance.PulseScale(roomCodeText.transform, 1.1f, 1.0f)); // Más sutil para espera
            },
            (error) => {
                SetStatus("Error al crear sala: " + error, true);
                Invoke("ReturnToMainPanel", 2f);
            }
        );
    }

    private void OnJoinRoom()
    {
        string playerName = nameInputField.text.Trim();
        string roomId = roomInputField.text.ToUpper().Trim();
        if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(roomId)) return;

        SavePlayerName(playerName);
        SaveServerAddress();
        SetStatus($"Uniéndose a {roomId} vía HTTP...", false);
        SwitchToWaitingPanel();

        var request = new NetworkManager.JoinRequest { roomId = roomId, playerName = playerName };
        NetworkManager.Instance.PostRequest<NetworkManager.RoomResponse>("/rooms/join", request, 
            (response) => {
                Debug.Log("Unido con éxito vía HTTP. Conectando Socket...");
                NetworkManager.Instance.ConnectToSocket(response.roomId);
                if (UIAnimationManager.Instance != null && roomCodeText != null)
                    StartCoroutine(UIAnimationManager.Instance.PulseScale(roomCodeText.transform, 1.1f, 1.0f));
            },
            (error) => {
                SetStatus("Error al unirse: " + error, true);
                Invoke("ReturnToMainPanel", 2f);
            }
        );
    }

    private void SetStatus(string message, bool isError)
    {
        statusText.text = message;
        statusText.color = isError ? Color.red : originalStatusColor;
        
        if (isError && UIAnimationManager.Instance != null)
            StartCoroutine(UIAnimationManager.Instance.PulseScale(statusText.transform, 1.05f, 0.2f));
    }

    private void SwitchToWaitingPanel()
    {
        if (UIAnimationManager.Instance != null && mainPanelGroup != null && waitingPanelGroup != null)
        {
            StartCoroutine(TransitionPanels(mainPanelGroup, waitingPanelGroup));
        }
    }

    private void ReturnToMainPanel()
    {
        if (UIAnimationManager.Instance != null && mainPanelGroup != null && waitingPanelGroup != null)
        {
            StartCoroutine(TransitionPanels(waitingPanelGroup, mainPanelGroup));
        }
        else
        {
            if (waitingPanelGroup != null) { waitingPanelGroup.alpha = 0; waitingPanelGroup.gameObject.SetActive(false); }
            if (mainPanelGroup != null) { mainPanelGroup.alpha = 1; mainPanelGroup.gameObject.SetActive(true); }
        }
    }

    private IEnumerator TransitionPanels(CanvasGroup from, CanvasGroup to)
    {
        to.gameObject.SetActive(true);
        to.alpha = 0;
        to.interactable = true;
        to.blocksRaycasts = true;

        from.interactable = false;
        from.blocksRaycasts = false;

        // Iniciar desvanecimientos en paralelo
        float duration = 0.3f;
        StartCoroutine(UIAnimationManager.Instance.FadeCanvasGroup(from, 1, 0, duration));
        yield return UIAnimationManager.Instance.FadeCanvasGroup(to, 0, 1, duration);

        from.gameObject.SetActive(false); // "Saca de escena" el panel anterior
    }

    private void SavePlayerName(string name)
    {
        NetworkManager.Instance.localPlayerName = name;
        NetworkManager.Instance.localPlayerId = name; // Vinculación de identidad
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();
    }

    private void SaveServerAddress()
    {
        if (serverAddressInputField != null)
        {
            string host = serverAddressInputField.text.Trim();
            if (string.IsNullOrEmpty(host)) host = "localhost";
            
            PlayerPrefs.SetString("ServerAddress", host);
            PlayerPrefs.Save();
            
            if (NetworkManager.Instance != null)
                NetworkManager.Instance.UpdateServerAddress(host);
        }
    }

    private void OnStartMatch()
    {
        Debug.Log($"<b>[Lobby]</b> Emitiendo START_MATCH para sala: {NetworkManager.Instance.currentRoomId}");
        // Envolver en un objeto para que el servidor lo reciba como JSON
        var startData = new NetworkManager.StartMatchData { roomId = NetworkManager.Instance.currentRoomId };
        NetworkManager.Instance.Emit("START_MATCH", startData);
    }

    private void HandleMatchStarted()
    {
        Debug.Log("Partida iniciada. Cargando escena de juego...");
        SceneManager.LoadScene("SampleScene");
    }

    private void Update()
    {
        // Validación básica de botones
        bool hasName = !string.IsNullOrEmpty(nameInputField.text.Trim());
        createBtn.interactable = hasName;
        joinBtn.interactable = hasName && !string.IsNullOrEmpty(roomInputField.text.Trim());

        if (waitingPanelGroup != null && waitingPanelGroup.gameObject.activeSelf)
        {
            roomCodeText.text = "Código: " + NetworkManager.Instance.currentRoomId;
            
            if (NetworkManager.Instance.isHost)
            {
                if (!startMatchBtn.gameObject.activeSelf)
                {
                    Debug.Log("<b>[Lobby]</b> Soy Host. Activando botón de inicio.");
                    startMatchBtn.gameObject.SetActive(true);
                }
                
                if (statusText.text.Contains("Esperando a que el Host")) 
                    SetStatus("Esperando jugadores...", false);
            }
            else
            {
                if (startMatchBtn.gameObject.activeSelf)
                    startMatchBtn.gameObject.SetActive(false);

                if (statusText.text.Contains("Esperando jugadores")) 
                    SetStatus("Esperando a que el Host inicie...", false);
            }
        }
    }
}
