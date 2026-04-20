using System;
using UnityEngine;
using Networking;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    [Header("Configuración")]
    public string serverUrl = "ws://localhost:3000/socket.io/?EIO=4&transport=websocket";
    
    [Header("Estado")]
    public string currentRoomId;
    public bool isHost;
    public string localPlayerId;

    private SocketIOClient client;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            client = gameObject.AddComponent<SocketIOClient>();
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
        client.OnEventReceived += HandleEvent;
        
        // Auto-conectar para pruebas si es necesario, 
        // pero normalmente se haría desde el Lobby
        // Connect();
    }

    public void Connect()
    {
        if (!client.IsConnected)
        {
            client.Connect(serverUrl);
        }
    }

    public void CreateRoom()
    {
        client.Emit("create_room", new {});
    }

    public void JoinRoom(string roomId)
    {
        client.Emit("join_room", roomId);
    }

    public void Emit(string eventName, object data)
    {
        client.Emit(eventName, data);
    }

    private void HandleConnected()
    {
        Debug.Log("[NetworkManager] Conexión establecida con éxito.");
    }

    private void HandleDisconnected()
    {
        Debug.LogWarning("[NetworkManager] Conexión perdida.");
    }

    public event Action<string, Vector3, float> OnRemotePlayerMoved;
    public event Action<string> OnRemotePlayerJoined;

    public event Action<string, Vector3, float> OnRemotePlayerShot;
    public event Action OnMatchStarted;
    public event Action<string, float> OnGameOver;
    public event Action<string, Vector3> OnEnemySpawned;

    private void HandleEvent(string eventName, string data)
    {
        Debug.Log($"[NetworkManager] Evento recibido: {eventName} -> {data}");
        
        switch (eventName)
        {
            case "room_created":
                var createdData = JsonUtility.FromJson<RoomData>(data);
                currentRoomId = createdData.roomId;
                isHost = createdData.isHost;
                break;
            case "room_joined":
                var joinedData = JsonUtility.FromJson<RoomData>(data);
                currentRoomId = joinedData.roomId;
                isHost = joinedData.isHost;
                break;
            case "player_joined":
                var joinData = JsonUtility.FromJson<JoinData>(data);
                OnRemotePlayerJoined?.Invoke(joinData.playerId);
                break;
            case "player_moved":
                var moveData = JsonUtility.FromJson<MoveData>(data);
                OnRemotePlayerMoved?.Invoke(moveData.playerId, new Vector3(moveData.x, moveData.y, 0), moveData.rotation);
                break;
            case "enemy_spawned":
                var enemyData = JsonUtility.FromJson<EnemyNetData>(data);
                OnEnemySpawned?.Invoke(enemyData.enemyId, new Vector3(enemyData.x, enemyData.y, 0));
                break;
            case "player_shot":
                var shootData = JsonUtility.FromJson<ShootNetData>(data);
                OnRemotePlayerShot?.Invoke(shootData.playerId, new Vector3(shootData.x, shootData.y, 0), shootData.rotation);
                break;
            case "match_started":
                OnMatchStarted?.Invoke();
                break;
            case "game_over":
                var gameOverData = JsonUtility.FromJson<GameOverData>(data);
                OnGameOver?.Invoke(gameOverData.playerId, gameOverData.survivalTime);
                break;
        }
    }

    [Serializable]
    public class ShootNetData
    {
        public string playerId;
        public float x;
        public float y;
        public float rotation;
    }

    [Serializable]
    public class GameOverData
    {
        public string playerId;
        public float survivalTime;
    }

    [Serializable]
    public class EnemyNetData
    {
        public string enemyId;
        public float x;
        public float y;
    }

    [Serializable]
    public class RoomData
    {
        public string roomId;
        public bool isHost;
    }

    [Serializable]
    public class JoinData
    {
        public string playerId;
    }

    [Serializable]
    public class MoveData
    {
        public string playerId;
        public float x;
        public float y;
        public float rotation;
    }
}
