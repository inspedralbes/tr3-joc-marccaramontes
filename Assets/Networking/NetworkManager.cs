using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

namespace AEA.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance { get; private set; }

        [Header("Configuración Legacy (HTTP)")]
        public string serverHttpUrl = "http://localhost:3001/api";
        public string serverHost = "localhost";
        
        [Header("Estado")]
        public string currentRoomId;
        public bool isHost => Unity.Netcode.NetworkManager.Singleton != null && Unity.Netcode.NetworkManager.Singleton.IsServer;
        public string localPlayerId;
        public string localPlayerName;
        public int playerCount => Unity.Netcode.NetworkManager.Singleton != null ? Unity.Netcode.NetworkManager.Singleton.ConnectedClients.Count : 1;

        // Eventos mantenidos para compatibilidad
        public event Action<string, Vector3, float> OnRemotePlayerMoved;
        public event Action<string, string> OnRemotePlayerJoined;
        public event Action<string[]> OnLobbyPlayersUpdated;
        public event Action<string> OnRemotePlayerLeft;
        public event Action<string, Vector3, float> OnRemotePlayerShot;
        public event Action OnMatchStarted;
        public event Action<string, float> OnGameOver;
        public event Action<string, Vector3, int> OnEnemySpawned;
        public event Action<string, Vector3> OnEnemySynced;
        public event Action<string, Vector3, float> OnEnemyShot;

        private async void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                try {
                    await UnityServices.InitializeAsync();
                    if (!AuthenticationService.Instance.IsSignedIn)
                    {
                        await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    }
                } catch (Exception e) {
                    Debug.LogError($"[NetworkManager] Error UGS: {e.Message}");
                }

                UpdateServerAddress(PlayerPrefs.GetString("ServerAddress", "localhost"));
            }
            else Destroy(gameObject);
        }

        public void UpdateServerAddress(string host)
        {
            if (string.IsNullOrEmpty(host)) host = "localhost";
            serverHost = host;
            serverHttpUrl = $"http://{host}:3001/api";
        }

        public void PostRequest<T>(string endpoint, object data, Action<T> onSuccess, Action<string> onError)
        {
            StartCoroutine(PostRequestRoutine(endpoint, data, onSuccess, onError));
        }

        private IEnumerator PostRequestRoutine<T>(string endpoint, object data, Action<T> onSuccess, Action<string> onError)
        {
            string json = JsonUtility.ToJson(data);
            string url = $"{serverHttpUrl}{endpoint}";
            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try {
                        T response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                        onSuccess?.Invoke(response);
                    } catch { onError?.Invoke("Parse error"); }
                } else onError?.Invoke(request.error);
            }
        }

        public async Task<string> CreateRelayRoom(int maxPlayers = 2)
        {
            try {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                var transport = Unity.Netcode.NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
                Unity.Netcode.NetworkManager.Singleton.StartHost();
                currentRoomId = joinCode;
                OnMatchStarted?.Invoke();
                return joinCode;
            } catch (Exception e) { Debug.LogError($"Relay Create Error: {e.Message}"); return null; }
        }

        public async Task<bool> JoinRelayRoom(string joinCode)
        {
            try {
                JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                var transport = Unity.Netcode.NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
                Unity.Netcode.NetworkManager.Singleton.StartClient();
                currentRoomId = joinCode;
                return true;
            } catch (Exception e) { Debug.LogError($"Relay Join Error: {e.Message}"); return false; }
        }

        public void ReportResults(float survivalTime)
        {
            var request = new ResultRequest { roomId = currentRoomId, playerName = localPlayerName, survivalTime = survivalTime };
            PostRequest<RoomResponse>("/results", request, (r) => {}, (e) => {});
        }

        public void LeaveRoom()
        {
            if (Unity.Netcode.NetworkManager.Singleton != null) Unity.Netcode.NetworkManager.Singleton.Shutdown();
            currentRoomId = "";
        }

        public void Emit(string type, object data) => Debug.Log($"[NetworkManager] Emit {type} redirected to NGO");

        [Serializable] public class RoomRequest { public string playerName; }
        [Serializable] public class JoinRequest { public string roomId; public string playerName; }
        [Serializable] public class RoomResponse { public string roomId; public bool success; }
        [Serializable] public class ResultRequest { public string roomId; public string playerName; public float survivalTime; }
        [Serializable] public class EnemySyncData { public string enemyId; public float x; public float y; }
    }
}
