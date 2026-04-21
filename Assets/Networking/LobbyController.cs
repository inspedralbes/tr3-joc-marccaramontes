using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    [Header("UI - Main")]
    public TMP_InputField nameInputField; // NUEVO: Para el requisito de parámetros
    public TMP_InputField roomInputField;
    public Button createBtn;
    public Button joinBtn;
    public Button backBtn;

    [Header("UI - Waiting")]
    public GameObject waitingPanel;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI roomCodeText;
    public Button startMatchBtn; // Solo visible para el Host

    private void Start()
    {
        createBtn.onClick.AddListener(OnCreateRoom);
        joinBtn.onClick.AddListener(OnJoinRoom);
        backBtn.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
        startMatchBtn.onClick.AddListener(OnStartMatch);

        waitingPanel.SetActive(false);
        startMatchBtn.gameObject.SetActive(false);

        // Suscribirse a eventos del NetworkManager
        NetworkManager.Instance.OnMatchStarted += HandleMatchStarted;

        // Cargar nombre previo si existe
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            nameInputField.text = PlayerPrefs.GetString("PlayerName");
        }
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
        statusText.text = "Creando sala vía HTTP...";
        waitingPanel.SetActive(true);

        var request = new NetworkManager.RoomRequest { playerName = playerName };
        NetworkManager.Instance.PostRequest<NetworkManager.RoomResponse>("/rooms/create", request, 
            (response) => {
                Debug.Log($"Sala creada: {response.roomId}");
                NetworkManager.Instance.ConnectToSocket(response.roomId);
            },
            (error) => {
                statusText.text = "Error al crear sala: " + error;
                Invoke("HideWaitingPanel", 2f);
            }
        );
    }

    private void OnJoinRoom()
    {
        string playerName = nameInputField.text.Trim();
        string roomId = roomInputField.text.ToUpper().Trim();
        if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(roomId)) return;

        SavePlayerName(playerName);
        statusText.text = $"Uniéndose a {roomId} vía HTTP...";
        waitingPanel.SetActive(true);

        var request = new NetworkManager.JoinRequest { roomId = roomId, playerName = playerName };
        NetworkManager.Instance.PostRequest<NetworkManager.RoomResponse>("/rooms/join", request, 
            (response) => {
                Debug.Log("Unido con éxito vía HTTP. Conectando Socket...");
                NetworkManager.Instance.ConnectToSocket(response.roomId);
            },
            (error) => {
                statusText.text = "Error al unirse: " + error;
                Invoke("HideWaitingPanel", 2f);
            }
        );
    }

    private void SavePlayerName(string name)
    {
        NetworkManager.Instance.localPlayerName = name;
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();
    }

    private void HideWaitingPanel()
    {
        waitingPanel.SetActive(false);
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

        if (waitingPanel.activeSelf)
        {
            roomCodeText.text = "Código: " + NetworkManager.Instance.currentRoomId;
            
            if (NetworkManager.Instance.isHost)
            {
                statusText.text = "Esperando jugadores...";
                startMatchBtn.gameObject.SetActive(true);
            }
            else
            {
                statusText.text = "Esperando a que el Host inicie...";
                startMatchBtn.gameObject.SetActive(false);
            }
        }
    }
}
