using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Networking;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    [Header("Configuración")]
    public string serverWsUrl = "ws://localhost:3000/ws";
    public string serverHttpUrl = "http://localhost:3000/api";
    
    [Header("Estado")]
    public string currentRoomId;
    public bool isHost;
    public string localPlayerId;
    public string localPlayerName;

    private NativeWebSocketClient client;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            client = gameObject.AddComponent<NativeWebSocketClient>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        client.OnConnected += HandleConnected;
        client.OnDisconnected += HandleDisconnected;
        client.OnMessageReceived += HandleMessage;
    }

    // --- Comunicación HTTP (UnityWebRequest) ---

    public void PostRequest<T>(string endpoint, object data, Action<T> onSuccess, Action<string> onError)
    {
        StartCoroutine(PostRequestRoutine(endpoint, data, onSuccess, onError));
    }

    private IEnumerator PostRequestRoutine<T>(string endpoint, object data, Action<T> onSuccess, Action<string> onError)
    {
        string json = JsonUtility.ToJson(data);
        // El endpoint ya debe empezar por / (ej: /rooms/create)
        string url = $"{serverHttpUrl}{endpoint}";
        
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[NetworkManager] Error en POST {endpoint}: {request.error}");
                onError?.Invoke(request.error);
            }
            else
            {
                Debug.Log($"[NetworkManager] POST {endpoint} exitoso: {request.downloadHandler.text}");
                try {
                    T response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                    onSuccess?.Invoke(response);
                } catch (Exception e) {
                    Debug.LogError($"[NetworkManager] Error parseando respuesta: {e.Message}");
                    onError?.Invoke("Error de parseo de datos");
                }
            }
        }
    }

    // --- Flujo de Conexión ---

    public void ConnectToSocket(string roomId)
    {
        currentRoomId = roomId;
        if (!client.IsConnected)
        {
            client.Connect(serverWsUrl);
        }
        else
        {
            EmitJoinSocket();
        }
    }

    private void HandleConnected()
    {
        Debug.Log("[NetworkManager] WS Connected. Joining room...");
        EmitJoinSocket();
    }

    private void EmitJoinSocket()
    {
        var data = new JoinSocketData { 
            roomId = currentRoomId, 
            playerName = localPlayerName 
        };
        client.Send("JOIN_ROOM", data);
    }

    public void Emit(string type, object data)
    {
        client.Send(type, data);
    }

    public void ReportResults(float survivalTime)
    {
        var request = new ResultRequest {
            roomId = currentRoomId,
            playerName = localPlayerName,
            survivalTime = survivalTime
        };

        PostRequest<RoomResponse>("/results", request, 
            (response) => Debug.Log("Resultados enviados con éxito via HTTP"),
            (error) => Debug.LogError("Error enviando resultados: " + error)
        );
    }

    private void HandleDisconnected()
    {
        Debug.LogWarning("[NetworkManager] WS Disconnected.");
    }

    // --- Eventos de Juego ---

    public event Action<string, Vector3, float> OnRemotePlayerMoved;
    public event Action<string, string> OnRemotePlayerJoined;
    public event Action<string> OnRemotePlayerLeft;
    public event Action<string, Vector3, float> OnRemotePlayerShot;
    public event Action OnMatchStarted;
    public event Action<string, float> OnGameOver;
    public event Action<string, Vector3> OnEnemySpawned;

    private void HandleMessage(string type, string payload)
    {
        Debug.Log($"[NetworkManager] Message received: {type}");
        
        switch (type)
        {
            case "ROOM_JOINED_CONFIRMED":
                var confirmedData = JsonUtility.FromJson<RoomConfirmedData>(payload);
                isHost = confirmedData.isHost;
                break;
            case "PLAYER_JOINED":
                var joinData = JsonUtility.FromJson<JoinData>(payload);
                OnRemotePlayerJoined?.Invoke(joinData.playerId, joinData.playerName);
                break;
            case "PLAYER_LEFT":
                var leftData = JsonUtility.FromJson<JoinData>(payload);
                OnRemotePlayerLeft?.Invoke(leftData.playerId);
                break;
            case "MOVE":
                var moveData = JsonUtility.FromJson<MoveData>(payload);
                OnRemotePlayerMoved?.Invoke(moveData.playerId, new Vector3(moveData.x, moveData.y, 0), moveData.rotation);
                break;
            case "SPAWN_ENEMY":
                var enemyData = JsonUtility.FromJson<EnemyNetData>(payload);
                OnEnemySpawned?.Invoke(enemyData.enemyId, new Vector3(enemyData.x, enemyData.y, 0));
                break;
            case "SHOOT":
                var shootData = JsonUtility.FromJson<ShootNetData>(payload);
                OnRemotePlayerShot?.Invoke(shootData.playerId, new Vector3(shootData.x, shootData.y, 0), shootData.rotation);
                break;
            case "START_MATCH":
                OnMatchStarted?.Invoke();
                break;
            case "DEATH":
                var gameOverData = JsonUtility.FromJson<GameOverData>(payload);
                OnGameOver?.Invoke(gameOverData.playerId, gameOverData.survivalTime);
                break;
        }
    }

    public void LeaveRoom()
    {
        if (string.IsNullOrEmpty(currentRoomId)) return;
        
        Debug.Log($"[NetworkManager] Leaving room {currentRoomId}...");
        client.Send("LEAVE_ROOM", new LeaveData { roomId = currentRoomId });
        currentRoomId = "";
        isHost = false;
    }

    // --- DTOs ---

    [Serializable] public class RoomRequest { public string playerName; }
    [Serializable] public class JoinRequest { public string roomId; public string playerName; }
    [Serializable] public class RoomResponse { public string roomId; public bool success; }
    [Serializable] public class ResultRequest { public string roomId; public string playerName; public float survivalTime; }

    [Serializable] public class LeaveData { public string roomId; }
    [Serializable] public class JoinSocketData { public string roomId; public string playerName; }
    [Serializable] public class RoomConfirmedData { public string roomId; public bool isHost; }
    [Serializable] public class JoinData { public string playerId; public string playerName; }
    [Serializable] public class ShootNetData { public string playerId; public float x; public float y; public float rotation; }
    [Serializable] public class GameOverData { public string playerId; public float survivalTime; }
    [Serializable] public class EnemyNetData { public string enemyId; public float x; public float y; }
    [Serializable] public class MoveData { public string playerId; public float x; public float y; public float rotation; }
}
