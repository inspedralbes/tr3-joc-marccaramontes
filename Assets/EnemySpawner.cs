using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum WaveState { Idle, Spawning, WaitingForClear, Rest }
    
    [Header("Configuración de Oleadas")]
    public WaveState currentState = WaveState.Rest;
    public float restDuration = 5f;
    
    [System.Serializable]
    public struct WaveConfig
    {
        public int enemyCount;
        public float spawnRate;
        [Range(0, 1)] public float interceptorChance;
    }
    
    public WaveConfig[] waves = new WaveConfig[] {
        new WaveConfig { enemyCount = 15, spawnRate = 3.0f, interceptorChance = 0f },
        new WaveConfig { enemyCount = 30, spawnRate = 2.5f, interceptorChance = 0.2f },
        new WaveConfig { enemyCount = 50, spawnRate = 2.0f, interceptorChance = 0.4f },
        new WaveConfig { enemyCount = 80, spawnRate = 1.5f, interceptorChance = 0.5f },
        new WaveConfig { enemyCount = 120, spawnRate = 1.0f, interceptorChance = 0.6f }
    };

    public GameObject enemyPrefab;
    
    [Header("Configuración de Spawneo")]
    public float minSpawnRadius = 10f;
    public float maxSpawnRadius = 12f;
    public Transform spawnCenter;

    private float nextSpawnTime;
    private float restTimer;
    private int currentWaveIndex = 0;
    private int enemiesSpawnedInWave = 0;

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
        
        restTimer = 0f; // Empieza la acción desde el segundo 1
        currentState = WaveState.Rest;
    }

    void Update()
    {
        // Solo el Host o el modo Solo spawnean enemigos automáticamente
        bool isMultiplayer = NetworkManager.Instance != null && NetworkManager.Instance.currentRoomId != "";
        bool isHost = NetworkManager.Instance != null && NetworkManager.Instance.isHost;

        if (isMultiplayer && !isHost) return; 

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        HandleWaveStates();
    }

    void HandleWaveStates()
    {
        switch (currentState)
        {
            case WaveState.Rest:
                restTimer -= Time.deltaTime;
                if (restTimer <= 0)
                {
                    StartWave();
                }
                break;

            case WaveState.Spawning:
                if (Time.time >= nextSpawnTime)
                {
                    // Spawneo por Clústers (grupos de 3 a 5)
                    int clusterSize = Random.Range(3, 6);
                    SpawnCluster(clusterSize);
                    enemiesSpawnedInWave += clusterSize;
                    
                    int waveToUse = Mathf.Min(currentWaveIndex, waves.Length - 1);
                    WaveConfig config = waves[waveToUse];
                    int bonusEnemies = (currentWaveIndex >= waves.Length) ? (currentWaveIndex - waves.Length + 1) * 20 : 0;
                    int totalEnemies = config.enemyCount + bonusEnemies;

                    if (enemiesSpawnedInWave >= totalEnemies)
                    {
                        currentState = WaveState.WaitingForClear;
                    }
                    else
                    {
                        // Pausa entre clústers
                        float currentSpawnRate = Mathf.Max(0.5f, config.spawnRate - (bonusEnemies * 0.01f));
                        nextSpawnTime = Time.time + currentSpawnRate;
                    }
                }
                break;

            case WaveState.WaitingForClear:
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                {
                    currentWaveIndex++;
                    restTimer = restDuration;
                    currentState = WaveState.Rest;
                    Debug.Log($"<color=cyan><b>[WaveSystem]</b> Oleada completada. Próxima en {restDuration}s.</color>");
                }
                break;
        }
    }

    void StartWave()
    {
        enemiesSpawnedInWave = 0;
        currentState = WaveState.Spawning;
        
        int waveToUse = Mathf.Min(currentWaveIndex, waves.Length - 1);
        WaveConfig config = waves[waveToUse];
        int bonusEnemies = (currentWaveIndex >= waves.Length) ? (currentWaveIndex - waves.Length + 1) * 20 : 0;
        int totalEnemies = config.enemyCount + bonusEnemies;
        float currentSpawnRate = Mathf.Max(0.5f, config.spawnRate - (bonusEnemies * 0.01f));

        nextSpawnTime = Time.time + currentSpawnRate;
        Debug.Log($"<color=cyan><b>[WaveSystem]</b> Iniciando Oleada {currentWaveIndex + 1} ({totalEnemies} enemigos).</color>");
    }

    void SpawnCluster(int size)
    {
        if (enemyPrefab == null) return;

        // Punto de spawn único para todo el clúster
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float radius = Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 spawnPosRel = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
        Vector3 centerPos = spawnCenter != null ? spawnCenter.position : transform.position;
        Vector3 clusterOrigin = new Vector3(centerPos.x + spawnPosRel.x, centerPos.y + spawnPosRel.y, 0f);

        for (int i = 0; i < size; i++)
        {
            SpawnIndividualInCluster(clusterOrigin);
        }
    }

    void SpawnIndividualInCluster(Vector3 origin)
    {
        string networkId = System.Guid.NewGuid().ToString();
        // Aumentamos el desplazamiento aleatorio inicial para que nazcan más separados
        // y el motor de físicas tenga menos trabajo de 'despegue'.
        Vector3 randomOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0f);
        GameObject newEnemy = Instantiate(enemyPrefab, origin + randomOffset, Quaternion.identity);
        
        // Decidir tipo
        int waveToUse = Mathf.Min(currentWaveIndex, waves.Length - 1);
        WaveConfig config = waves[waveToUse];
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            float chance = config.interceptorChance + (currentWaveIndex >= waves.Length ? 0.1f : 0f);
            enemyScript.type = Random.value < Mathf.Min(0.8f, chance) ? Enemy.EnemyType.Interceptor : Enemy.EnemyType.Basic;
        }

        newEnemy.GetComponent<NetworkIdentity>()?.Setup(networkId, false, true);

        // Notificar al resto
        if (NetworkManager.Instance != null && NetworkManager.Instance.isHost)
        {
            NetworkManager.Instance.Emit("spawn_enemy", new EnemySpawnData {
                roomId = NetworkManager.Instance.currentRoomId,
                enemyId = networkId,
                x = origin.x + randomOffset.x,
                y = origin.y + randomOffset.y,
                type = (int)(enemyScript != null ? enemyScript.type : Enemy.EnemyType.Basic)
            });
        }
    }

    private void RemoteSpawn(string id, Vector3 position, int type)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.type = (Enemy.EnemyType)type;
        }
        newEnemy.GetComponent<NetworkIdentity>()?.Setup(id, false, false);
    }

    [System.Serializable]
    public class EnemySpawnData
    {
        public string roomId;
        public string enemyId;
        public float x;
        public float y;
        public int type;
    }
}
