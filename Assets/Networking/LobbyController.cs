using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    [Header("UI - Main")]
    public CanvasGroup mainPanelGroup; 
    public TMP_InputField nameInputField;
    public TMP_InputField roomInputField;
    public Button createBtn;
    public Button joinBtn;
    public Button backBtn;

    [Header("UI - Waiting")]
    public CanvasGroup waitingPanelGroup; 
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI roomCodeText;
    public Button startMatchBtn;
    public Button backToMainBtn; // Botón para volver al panel principal

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
        }
        else
        {
            Debug.LogWarning("<b>[LobbyController]</b> NetworkManager.Instance no encontrado. ¿Has pasado por la escena Menu?");
        }

        // Cargar nombre previo si existe
        if (PlayerPrefs.HasKey("PlayerName") && nameInputField != null)
        {
            nameInputField.text = PlayerPrefs.GetString("PlayerName");
        }

        // Animación de entrada
        if (mainPanelGroup != null && UIAnimationManager.Instance != null)
            StartCoroutine(UIAnimationManager.Instance.FadeCanvasGroup(mainPanelGroup, 0, 1, 0.5f));
    }

    private void OnDestroy()
    {
        if (NetworkManager.Instance != null)
            NetworkManager.Instance.OnMatchStarted -= HandleMatchStarted;
    }

    private void OnCreateRoom()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName)) return;

        SavePlayerName(playerName);
        SetStatus("Creando sala vía HTTP...", false);
        SwitchToWaitingPanel();

        var request = new NetworkManager.RoomRequest { playerName = playerName };
        NetworkManager.Instance.PostRequest<NetworkManager.RoomResponse>("/rooms/create", request, 
            (response) => {
                Debug.Log($"Sala creada: {response.roomId}");
                NetworkManager.Instance.ConnectToSocket(response.roomId);
                if (UIAnimationManager.Instance != null)
                    StartCoroutine(UIAnimationManager.Instance.PulseScale(roomCodeText.transform, 1.2f, 0.5f));
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
        SetStatus($"Uniéndose a {roomId} vía HTTP...", false);
        SwitchToWaitingPanel();

        var request = new NetworkManager.JoinRequest { roomId = roomId, playerName = playerName };
        NetworkManager.Instance.PostRequest<NetworkManager.RoomResponse>("/rooms/join", request, 
            (response) => {
                Debug.Log("Unido con éxito vía HTTP. Conectando Socket...");
                NetworkManager.Instance.ConnectToSocket(response.roomId);
                if (UIAnimationManager.Instance != null)
                    StartCoroutine(UIAnimationManager.Instance.PulseScale(roomCodeText.transform, 1.2f, 0.5f));
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
            StartCoroutine(UIAnimationManager.Instance.FadeCanvasGroup(mainPanelGroup, 1, 0, 0.3f));
            StartCoroutine(UIAnimationManager.Instance.FadeCanvasGroup(waitingPanelGroup, 0, 1, 0.3f));
            mainPanelGroup.interactable = false;
            mainPanelGroup.blocksRaycasts = false;
            waitingPanelGroup.interactable = true;
            waitingPanelGroup.blocksRaycasts = true;
        }
    }

    private void ReturnToMainPanel()
    {
        if (UIAnimationManager.Instance != null && mainPanelGroup != null && waitingPanelGroup != null)
        {
            StartCoroutine(UIAnimationManager.Instance.FadeCanvasGroup(waitingPanelGroup, 1, 0, 0.3f));
            StartCoroutine(UIAnimationManager.Instance.FadeCanvasGroup(mainPanelGroup, 0, 1, 0.3f));
            waitingPanelGroup.interactable = false;
            waitingPanelGroup.blocksRaycasts = false;
            mainPanelGroup.interactable = true;
            mainPanelGroup.blocksRaycasts = true;
        }
        else
        {
            if (waitingPanelGroup != null) waitingPanelGroup.alpha = 0;
            if (mainPanelGroup != null) mainPanelGroup.alpha = 1;
        }
    }

    private void SavePlayerName(string name)
    {
        NetworkManager.Instance.localPlayerName = name;
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();
    }

    private void OnStartMatch()
    {
        NetworkManager.Instance.Emit("start_match", NetworkManager.Instance.currentRoomId);
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

        if (waitingPanelGroup != null && waitingPanelGroup.alpha > 0.5f)
        {
            roomCodeText.text = "Código: " + NetworkManager.Instance.currentRoomId;
            
            if (NetworkManager.Instance.isHost)
            {
                if (statusText.text.Contains("Esperando a que el Host")) SetStatus("Esperando jugadores...", false);
                startMatchBtn.gameObject.SetActive(true);
            }
            else
            {
                if (statusText.text.Contains("Esperando jugadores")) SetStatus("Esperando a que el Host inicie...", false);
                startMatchBtn.gameObject.SetActive(false);
            }
        }
    }
}
