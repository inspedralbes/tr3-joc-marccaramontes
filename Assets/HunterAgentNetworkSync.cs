using UnityEngine;
using Networking;

public class HunterAgentNetworkSync : MonoBehaviour
{
    private HunterAgent agent;
    private Rigidbody2D rb;
    private NetworkManager net;

    [Header("Sync Settings")]
    public float syncInterval = 0.1f;
    private float lastSyncTime;

    private void Start()
    {
        agent = GetComponent<HunterAgent>();
        rb = GetComponent<Rigidbody2D>();
        net = NetworkManager.Instance;

        if (net == null)
        {
            Debug.LogError("NetworkManager not found!");
            return;
        }

        // Si no somos el Host, desactivamos el componente Agent y el Rigidbody para que no calcule IA localmente
        if (!net.isHost)
        {
            if (agent != null) agent.enabled = false;
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
            
            // Suscribirse a actualizaciones de red
            net.OnEnemySpawned += HandleRemoteSync;
        }
    }

    private void Update()
    {
        if (net == null || !net.isHost) return;

        if (Time.time - lastSyncTime > syncInterval)
        {
            lastSyncTime = Time.time;
            SyncPosition();
        }
    }

    private void SyncPosition()
    {
        var data = new NetworkManager.EnemyNetData
        {
            enemyId = "HUNTER_AGENT",
            x = transform.position.x,
            y = transform.position.y
        };

        net.Emit("SPAWN_ENEMY", data); // Reutilizamos SPAWN_ENEMY para sincronizar posición del agente
    }

    private void HandleRemoteSync(string id, Vector3 pos)
    {
        if (id == "HUNTER_AGENT")
        {
            transform.position = pos;
        }
    }
}
