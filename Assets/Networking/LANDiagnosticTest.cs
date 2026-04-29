using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class LANDiagnosticTest : MonoBehaviour
{
    [ContextMenu("Simular Servidor Encontrado")]
    public void SimulateServerFound()
    {
        try
        {
            UdpClient client = new UdpClient();
            string msg = "AEA_SERVER_DISCOVERY|192.168.1.100|3000";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 4545);
            client.Send(data, data.Length, ep);
            client.Close();
            Debug.Log("<color=cyan>[Test] Paquete de descubrimiento simulado enviado.</color>");
        }
        catch (System.Exception e)
        {
            Debug.LogError("[Test] Error: " + e.Message);
        }
    }

    [ContextMenu("Test Local IP")]
    public void TestIP()
    {
        if (LANDiscoveryManager.Instance != null)
        {
            Debug.Log("<color=yellow>[Test] Tu IP Local detectada es: " + LANDiscoveryManager.Instance.GetLocalIPv4() + "</color>");
        }
    }
}
