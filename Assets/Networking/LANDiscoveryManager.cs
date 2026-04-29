using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LANDiscoveryManager : MonoBehaviour
{
    public static LANDiscoveryManager Instance { get; private set; }

    [Header("Configuración")]
    public int discoveryPort = 4545;
    public string discoveryPrefix = "AEA_SERVER_DISCOVERY";
    public float broadcastInterval = 2f;

    public event Action<string> OnServerFound;

    private UdpClient udpClient;
    private bool isListening = false;
    private bool isBroadcasting = false;
    private string localIP;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            localIP = GetLocalIPv4();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- BROADCASTER (Para el Host) ---

    public void StartBroadcasting()
    {
        if (isBroadcasting) return;
        isBroadcasting = true;
        isListening = false; // El host no necesita escucharse a sí mismo
        
        Debug.Log($"[LANDiscovery] Iniciando broadcast en puerto {discoveryPort}...");
        InvokeRepeating(nameof(SendBroadcastPacket), 0f, broadcastInterval);
    }

    public void StopBroadcasting()
    {
        isBroadcasting = false;
        CancelInvoke(nameof(SendBroadcastPacket));
        Debug.Log("[LANDiscovery] Broadcast detenido.");
    }

    private void SendBroadcastPacket()
    {
        try
        {
            if (udpClient == null) udpClient = new UdpClient();
            
            string message = $"{discoveryPrefix}|{localIP}|3000";
            byte[] data = Encoding.UTF8.GetBytes(message);
            
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, discoveryPort);
            udpClient.Send(data, data.Length, endPoint);
        }
        catch (Exception e)
        {
            Debug.LogError($"[LANDiscovery] Error enviando broadcast: {e.Message}");
        }
    }

    // --- LISTENER (Para el Cliente) ---

    public void StartListening()
    {
        if (isListening || isBroadcasting) return;
        
        try
        {
            if (udpClient != null) udpClient.Close();
            
            udpClient = new UdpClient(discoveryPort);
            udpClient.EnableBroadcast = true;
            isListening = true;
            
            Debug.Log($"[LANDiscovery] Escuchando en puerto {discoveryPort}...");
            BeginReceive();
        }
        catch (Exception e)
        {
            Debug.LogError($"[LANDiscovery] No se pudo iniciar el listener: {e.Message}");
        }
    }

    public void StopListening()
    {
        isListening = false;
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }
        Debug.Log("[LANDiscovery] Listener detenido.");
    }

    private async void BeginReceive()
    {
        while (isListening)
        {
            try
            {
                UdpReceiveResult result = await udpClient.ReceiveAsync();
                string message = Encoding.UTF8.GetString(result.Buffer);
                
                if (message.StartsWith(discoveryPrefix))
                {
                    string[] parts = message.Split('|');
                    if (parts.Length >= 2)
                    {
                        string discoveredIP = parts[1];
                        // Si recibimos nuestro propio broadcast (por si acaso), lo ignoramos
                        if (discoveredIP != localIP)
                        {
                            MainThreadDispatcher.Execute(() => OnServerFound?.Invoke(discoveredIP));
                        }
                    }
                }
            }
            catch (ObjectDisposedException) { break; }
            catch (Exception e)
            {
                if (isListening) Debug.LogError($"[LANDiscovery] Error recibiendo: {e.Message}");
                break;
            }
        }
    }

    // --- UTILIDADES ---

    public string GetLocalIPv4()
    {
        string hostName = Dns.GetHostName();
        IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1";
    }

    private void OnDestroy()
    {
        StopBroadcasting();
        StopListening();
    }
}

// Clase de utilidad para ejecutar acciones en el hilo principal de Unity
public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly System.Collections.Generic.Queue<Action> executionQueue = new System.Collections.Generic.Queue<Action>();

    public static void Execute(Action action)
    {
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue().Invoke();
            }
        }
    }

    private static MainThreadDispatcher instance;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("MainThreadDispatcher");
            instance = go.AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(go);
        }
    }
}
