using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using AEA.Networking;

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
    public Button backToMainBtn;

    private Color originalStatusColor;

    private void Start()
    {
        if (createBtn != null) createBtn.onClick.AddListener(OnCreateRoom);
        if (joinBtn != null) joinBtn.onClick.AddListener(OnJoinRoom);
        if (backBtn != null) backBtn.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
        if (startMatchBtn != null) startMatchBtn.onClick.AddListener(OnStartMatch);
        if (backToMainBtn != null) backToMainBtn.onClick.AddListener(ReturnToMainPanel);

        if (mainPanelGroup != null) { mainPanelGroup.alpha = 1; mainPanelGroup.interactable = true; mainPanelGroup.blocksRaycasts = true; }
        if (waitingPanelGroup != null) { waitingPanelGroup.alpha = 0; waitingPanelGroup.interactable = false; waitingPanelGroup.blocksRaycasts = false; }
        
        if (startMatchBtn != null) startMatchBtn.gameObject.SetActive(false);
        if (statusText != null) originalStatusColor = statusText.color;

        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnMatchStarted += HandleMatchStarted;
        }

        if (PlayerPrefs.HasKey("PlayerName") && nameInputField != null)
        {
            nameInputField.text = PlayerPrefs.GetString("PlayerName");
        }

        if (serverAddressInputField != null)
        {
            serverAddressInputField.text = PlayerPrefs.GetString("ServerAddress", "localhost");
            serverAddressInputField.onValueChanged.AddListener((val) => {
                if (NetworkManager.Instance != null) NetworkManager.Instance.UpdateServerAddress(val);
            });
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnMatchStarted -= HandleMatchStarted;
        }
    }

    private async void OnCreateRoom()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName)) return;

        SavePlayerName(playerName);
        SetStatus("Solicitando Relay Join Code...", false);
        SwitchToWaitingPanel();

        string joinCode = await NetworkManager.Instance.CreateRelayRoom();
        if (!string.IsNullOrEmpty(joinCode))
        {
            Debug.Log($"Sala creada en Relay: {joinCode}");
            if (roomCodeText != null) roomCodeText.text = "Código: " + joinCode;
            SetStatus("Sala lista. Esperando jugadores...", false);
        }
        else
        {
            SetStatus("Error al crear sala en Relay", true);
            Invoke("ReturnToMainPanel", 2f);
        }
    }

    private async void OnJoinRoom()
    {
        string playerName = nameInputField.text.Trim();
        string joinCode = roomInputField.text.ToUpper().Trim();
        if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(joinCode)) return;

        SavePlayerName(playerName);
        SetStatus($"Uniéndose a {joinCode} vía Relay...", false);
        SwitchToWaitingPanel();

        bool success = await NetworkManager.Instance.JoinRelayRoom(joinCode);
        if (success)
        {
            Debug.Log("Unido con éxito vía Relay.");
            if (roomCodeText != null) roomCodeText.text = "Código: " + joinCode;
            SetStatus("Conectado. Esperando inicio...", false);
        }
        else
        {
            SetStatus("Error al unirse: Código inválido o timeout", true);
            Invoke("ReturnToMainPanel", 2f);
        }
    }

    private void SetStatus(string message, bool isError)
    {
        if (statusText == null) return;
        statusText.text = message;
        statusText.color = isError ? Color.red : originalStatusColor;
    }

    private void SwitchToWaitingPanel()
    {
        if (mainPanelGroup != null && waitingPanelGroup != null)
        {
            StartCoroutine(TransitionPanels(mainPanelGroup, waitingPanelGroup));
        }
    }

    private void ReturnToMainPanel()
    {
        if (NetworkManager.Instance != null) NetworkManager.Instance.LeaveRoom();

        if (mainPanelGroup != null && waitingPanelGroup != null)
        {
            StartCoroutine(TransitionPanels(waitingPanelGroup, mainPanelGroup));
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

        float elapsed = 0;
        float duration = 0.3f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            from.alpha = 1 - t;
            to.alpha = t;
            yield return null;
        }
        from.alpha = 0;
        to.alpha = 1;
        from.gameObject.SetActive(false);
    }

    private void SavePlayerName(string name)
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.localPlayerName = name;
            NetworkManager.Instance.localPlayerId = name;
        }
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();
    }

    private void OnStartMatch()
    {
        if (NetworkManager.Instance.isHost)
        {
            // En NGO, el Host carga la escena y los clientes la siguen automáticamente 
            // si NetworkManager tiene EnableSceneManagement activo.
            // Para compatibilidad con el flujo actual, usamos SceneManager pero NGO lo sincronizará.
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void HandleMatchStarted()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Update()
    {
        bool hasName = !string.IsNullOrEmpty(nameInputField.text.Trim());
        if (createBtn != null) createBtn.interactable = hasName;
        if (joinBtn != null) joinBtn.interactable = hasName && !string.IsNullOrEmpty(roomInputField.text.Trim());

        if (waitingPanelGroup != null && waitingPanelGroup.gameObject.activeSelf)
        {
            if (NetworkManager.Instance.isHost)
            {
                if (startMatchBtn != null && !startMatchBtn.gameObject.activeSelf)
                    startMatchBtn.gameObject.SetActive(true);
            }
            else
            {
                if (startMatchBtn != null && startMatchBtn.gameObject.activeSelf)
                    startMatchBtn.gameObject.SetActive(false);
            }
        }
    }
}
