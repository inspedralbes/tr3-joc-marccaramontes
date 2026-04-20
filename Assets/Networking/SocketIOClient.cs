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
    public class SocketIOClient : MonoBehaviour
    {
        private ClientWebSocket webSocket;
        private Uri serverUri;
        private CancellationTokenSource cancellationTokenSource;

        public event Action<string, string> OnEventReceived;
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
                Debug.LogError("[SocketIO] Error al conectar: " + connectTask.Exception.Message);
                yield break;
            }

            Debug.Log("[SocketIO] Conectado al servidor.");
            OnConnected?.Invoke();

            // Enviar paquete de conexión inicial de Socket.io (40)
            SendRaw("40");

            StartCoroutine(ReceiveRoutine());
        }

        private IEnumerator ReceiveRoutine()
        {
            byte[] buffer = new byte[1024 * 4];
            while (IsConnected)
            {
                var receiveTask = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token);
                while (!receiveTask.IsCompleted) yield return null;

                if (receiveTask.Result.MessageType == WebSocketMessageType.Close)
                {
                    OnDisconnected?.Invoke();
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, receiveTask.Result.Count);
                ParsePacket(message);
            }
        }

        private void ParsePacket(string packet)
        {
            // Socket.io packet format: 42["event", {...}]
            if (packet.StartsWith("42"))
            {
                string jsonArray = packet.Substring(2);
                // Muy básico: buscamos el nombre del evento y el objeto JSON
                // Esto es una simplificación extrema para el prototipo
                try 
                {
                    // Formato esperado: ["event", {data}]
                    int firstComma = jsonArray.IndexOf(',');
                    if (firstComma != -1)
                    {
                        string eventName = jsonArray.Substring(2, firstComma - 3); // Quita [" y ",
                        string eventData = jsonArray.Substring(firstComma + 1, jsonArray.Length - firstComma - 2);
                        OnEventReceived?.Invoke(eventName, eventData);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning("[SocketIO] Error parseando paquete: " + packet + " -> " + e.Message);
                }
            }
        }

        public void Emit(string eventName, object data)
        {
            if (!IsConnected) return;

            string json = JsonUtility.ToJson(data);
            string packet = $"42[\"{eventName}\",{json}]";
            SendRaw(packet);
        }

        private async void SendRaw(string packet)
        {
            if (!IsConnected) return;
            byte[] buffer = Encoding.UTF8.GetBytes(packet);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationTokenSource.Token);
        }

        private void OnDestroy()
        {
            cancellationTokenSource?.Cancel();
            webSocket?.Dispose();
        }
    }
}
