using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Networking
{
    [Serializable]
    public class NetworkPacket
    {
        public string type;
        public string playerId; // Injected by server
        public string payload;
    }

    public class NativeWebSocketClient : MonoBehaviour
    {
        private ClientWebSocket webSocket;
        private Uri serverUri;
        private CancellationTokenSource cancellationTokenSource;
        private int retryCount = 0;
        private const int MaxRetries = 3;

        public event Action<string, string, string> OnMessageReceived;
        public event Action OnConnected;
        public event Action OnDisconnected;

        public bool IsConnected => webSocket != null && webSocket.State == WebSocketState.Open;

        public void Connect(string url)
        {
            serverUri = new Uri(url);
            cancellationTokenSource = new CancellationTokenSource();
            StartCoroutine(ConnectRoutine());
        }

        private IEnumerator ConnectRoutine()
        {
            webSocket = new ClientWebSocket();
            Task connectTask = webSocket.ConnectAsync(serverUri, cancellationTokenSource.Token);

            while (!connectTask.IsCompleted) yield return null;

            if (connectTask.IsFaulted)
            {
                Debug.LogError("[WS] Connection Error: " + connectTask.Exception.Message);
                if (retryCount < MaxRetries)
                {
                    retryCount++;
                    Debug.Log($"[WS] Retrying connection ({retryCount}/{MaxRetries})...");
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(ConnectRoutine());
                }
                yield break;
            }

            retryCount = 0;
            Debug.Log("[WS] Connected to server.");
            OnConnected?.Invoke();

            StartCoroutine(ReceiveRoutine());
        }

        private IEnumerator ReceiveRoutine()
        {
            byte[] buffer = new byte[1024 * 8];
            while (IsConnected)
            {
                var receiveTask = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token);
                while (!receiveTask.IsCompleted) yield return null;

                if (receiveTask.Result.MessageType == WebSocketMessageType.Close || webSocket.State != WebSocketState.Open)
                {
                    Debug.Log("[WS] Connection closed or lost.");
                    OnDisconnected?.Invoke();
                    
                    // Si perdimos la conexión inesperadamente, intentar reconectar
                    if (retryCount < MaxRetries)
                    {
                        Debug.Log("[WS] Attempting automatic reconnection...");
                        StartCoroutine(ConnectRoutine());
                    }
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, receiveTask.Result.Count);
                try 
                {
                    var packet = JsonUtility.FromJson<NetworkPacket>(message);
                    OnMessageReceived?.Invoke(packet.type, packet.playerId, packet.payload);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[WS] Error parsing packet: " + message + " -> " + e.Message);
                }
            }
        }

        public void Send(string type, object data)
        {
            if (!IsConnected) return;

            string jsonPayload = JsonUtility.ToJson(data);
            var packet = new NetworkPacket { type = type, payload = jsonPayload };
            string jsonPacket = JsonUtility.ToJson(packet);

            SendRaw(jsonPacket);
        }

        private async void SendRaw(string message)
        {
            if (!IsConnected) return;
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationTokenSource.Token);
        }

        public void Disconnect()
        {
            cancellationTokenSource?.Cancel();
            webSocket?.Dispose();
        }

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}
