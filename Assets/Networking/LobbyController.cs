using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyController : MonoBehaviour
{
    [Header("UI - Main")]
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
        backBtn.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Menu"));
        startMatchBtn.onClick.AddListener(OnStartMatch);

        waitingPanel.SetActive(false);
        startMatchBtn.gameObject.SetActive(false);

        NetworkManager.Instance.Connect();
    }

    private void OnCreateRoom()
    {
        NetworkManager.Instance.CreateRoom();
        statusText.text = "Creando sala...";
        waitingPanel.SetActive(true);
    }

    private void OnJoinRoom()
    {
        string roomId = roomInputField.text.ToUpper();
        if (string.IsNullOrEmpty(roomId)) return;

        NetworkManager.Instance.JoinRoom(roomId);
        statusText.text = "Uniéndose a " + roomId + "...";
        waitingPanel.SetActive(true);
    }

    private void OnStartMatch()
    {
        NetworkManager.Instance.Emit("start_match", NetworkManager.Instance.currentRoomId);
    }

    private void Update()
    {
        if (waitingPanel.activeSelf)
        {
            roomCodeText.text = "Código: " + NetworkManager.Instance.currentRoomId;
            
            if (NetworkManager.Instance.isHost)
            {
                statusText.text = "Esperando jugadores...";
                // En un prototipo simple, dejamos que el host empiece cuando quiera 
                // o cuando el server notifique que hay alguien más.
                startMatchBtn.gameObject.SetActive(true);
            }
            else
            {
                statusText.text = "Esperando a que el Host inicie...";
            }
        }
    }
}
