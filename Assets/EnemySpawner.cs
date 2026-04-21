using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 2f;
    
    [Header("Configuración de Spawneo")]
    public float minSpawnRadius = 10f;
    public float maxSpawnRadius = 12f;
    public Transform spawnCenter;

    private float nextSpawnTime;

    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("¡ATENCIÓN! No has asignado el 'Enemy Prefab' en el EnemySpawner.");
        }

        if (spawnCenter == null)
        {
            try {
                GameObject platform = GameObject.FindGameObjectWithTag("Platform");
                if (platform != null) spawnCenter = platform.transform;
            } catch {
                Debug.LogWarning("<b>[EnemySpawner]</b> Tag 'Platform' no definido. Usando posición propia.");
            }
        }

        // Si somos el cliente, nos suscribimos a los eventos del Host
        if (NetworkManager.Instance != null && !NetworkManager.Instance.isHost)
        {
            NetworkManager.Instance.OnEnemySpawned += RemoteSpawn;
        }
        
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        // Solo el Host o el modo Solo spawnean enemigos automáticamente
        bool isMultiplayer = NetworkManager.Instance != null && NetworkManager.Instance.currentRoomId != "";
        bool isHost = NetworkManager.Instance != null && NetworkManager.Instance.isHost;

        if (isMultiplayer && !isHost) return; 

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
            if (spawnRate > 0.5f) spawnRate -= 0.01f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float radius = Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 spawnPosRel = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
        Vector3 centerPos = spawnCenter != null ? spawnCenter.position : transform.position;
        Vector3 spawnPosFinal = new Vector3(centerPos.x + spawnPosRel.x, centerPos.y + spawnPosRel.y, 0f);
        
        string networkId = System.Guid.NewGuid().ToString();
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosFinal, Quaternion.identity);
        newEnemy.GetComponent<NetworkIdentity>()?.Setup(networkId, false, true);

        // Notificar al resto
        if (NetworkManager.Instance != null && NetworkManager.Instance.isHost)
        {
            NetworkManager.Instance.Emit("spawn_enemy", new EnemySpawnData {
                roomId = NetworkManager.Instance.currentRoomId,
                enemyId = networkId,
                x = spawnPosFinal.x,
                y = spawnPosFinal.y
            });
        }
    }

    private void RemoteSpawn(string id, Vector3 position)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        newEnemy.GetComponent<NetworkIdentity>()?.Setup(id, false, false);
    }

    [System.Serializable]
    public class EnemySpawnData
    {
        public string roomId;
        public string enemyId;
        public float x;
        public float y;
    }
}
