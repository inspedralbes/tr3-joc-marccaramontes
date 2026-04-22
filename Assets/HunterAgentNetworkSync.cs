using UnityEngine;
using Networking;

public class HunterAgentNetworkSync : MonoBehaviour
{
    private HunterAgent agent;
    private Rigidbody2D rb;
    private NetworkManager net;

    [Header("Sync Settings")]
    public float syncInterval = 0.1f;
    public float lerpSpeed = 10f;
    private float lastSyncTime;
    private Vector3 targetPosition;

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

        targetPosition = transform.position;

        // Si no somos el Host, desactivamos el componente Agent y el Rigidbody para que no calcule IA localmente
        if (!net.isHost)
        {
            if (agent != null) agent.enabled = false;
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
            
            // Suscribirse a actualizaciones de red semánticas
            net.OnEnemySynced += HandleRemoteSync;
        }
    }

    private void Update()
    {
        if (net == null) return;

        if (net.isHost)
        {
            if (Time.time - lastSyncTime > syncInterval)
            {
                lastSyncTime = Time.time;
                SyncPosition();
            }
        }
        else
        {
            // Interpolación suave en los clientes
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        }
    }

    private void SyncPosition()
    {
        // Requisito 3.1: Usar el nuevo DTO y evento semántico
        var data = new NetworkManager.EnemySyncData
        {
            enemyId = "HUNTER_AGENT",
            x = transform.position.x,
            y = transform.position.y
        };

        net.Emit("ENEMY_SYNC", data);
    }

    private void HandleRemoteSync(string id, Vector3 pos)
    {
        if (id == "HUNTER_AGENT")
        {
            targetPosition = pos;
        }
    }
}
