using UnityEngine;
using Unity.Netcode;

public class EnemySpawner : NetworkBehaviour
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
        [Range(0, 1)] public float stalkerChance;
    }
    
    public WaveConfig[] waves = new WaveConfig[] {
        new WaveConfig { enemyCount = 15, spawnRate = 3.0f, interceptorChance = 0f, stalkerChance = 0f },
        new WaveConfig { enemyCount = 30, spawnRate = 2.5f, interceptorChance = 0.2f, stalkerChance = 0f },
        new WaveConfig { enemyCount = 50, spawnRate = 2.0f, interceptorChance = 0.4f, stalkerChance = 0.1f },
        new WaveConfig { enemyCount = 80, spawnRate = 1.5f, interceptorChance = 0.5f, stalkerChance = 0.2f },
        new WaveConfig { enemyCount = 120, spawnRate = 1.0f, interceptorChance = 0.6f, stalkerChance = 0.3f }
    };

    public GameObject enemyPrefab;
    public GameObject stalkerBulletPrefab;
    
    [Header("Configuración de Spawneo")]
    public float minSpawnRadius = 10f;
    public float maxSpawnRadius = 12f;
    public Transform spawnCenter;

    private float nextSpawnTime;
    private float restTimer;
    private int currentWaveIndex = 0;
    private int enemiesSpawnedInWave = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
            return;
        }

        if (spawnCenter == null)
        {
            GameObject platform = GameObject.FindGameObjectWithTag("Platform");
            if (platform != null) spawnCenter = platform.transform;
        }
        
        restTimer = 0f; 
        currentState = WaveState.Rest;
    }

    void Update()
    {
        if (!IsServer) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        HandleWaveStates();
    }

    void HandleWaveStates()
    {
        switch (currentState)
        {
            case WaveState.Rest:
                restTimer -= Time.deltaTime;
                if (restTimer <= 0) StartWave();
                break;

            case WaveState.Spawning:
                if (Time.time >= nextSpawnTime)
                {
                    int clusterSize = Random.Range(3, 6);
                    SpawnCluster(clusterSize);
                    enemiesSpawnedInWave += clusterSize;
                    
                    int waveToUse = Mathf.Min(currentWaveIndex, waves.Length - 1);
                    WaveConfig config = waves[waveToUse];
                    int bonusEnemies = (currentWaveIndex >= waves.Length) ? (currentWaveIndex - waves.Length + 1) * 20 : 0;
                    int totalEnemies = config.enemyCount + bonusEnemies;

                    if (enemiesSpawnedInWave >= totalEnemies) currentState = WaveState.WaitingForClear;
                    else
                    {
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
        float currentSpawnRate = Mathf.Max(0.5f, config.spawnRate - (bonusEnemies * 0.01f));

        nextSpawnTime = Time.time + currentSpawnRate;
    }

    void SpawnCluster(int size)
    {
        if (enemyPrefab == null) return;

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
        Vector3 randomOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0f);
        GameObject newEnemy = Instantiate(enemyPrefab, origin + randomOffset, Quaternion.identity);
        
        int waveToUse = Mathf.Min(currentWaveIndex, waves.Length - 1);
        WaveConfig config = waves[waveToUse];
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        
        if (enemyScript != null)
        {
            float roll = Random.value;
            float currentStalkerChance = config.stalkerChance + (currentWaveIndex >= waves.Length ? 0.1f : 0f);
            float currentInterceptorChance = config.interceptorChance + (currentWaveIndex >= waves.Length ? 0.05f : 0f);

            if (roll < currentStalkerChance)
            {
                enemyScript.type = Enemy.EnemyType.Stalker;
                enemyScript.bulletPrefab = stalkerBulletPrefab;
            }
            else if (roll < currentStalkerChance + currentInterceptorChance)
            {
                enemyScript.type = Enemy.EnemyType.Interceptor;
            }
            else
            {
                enemyScript.type = Enemy.EnemyType.Basic;
            }
        }

        // Sincronizar vía NGO
        var netObj = newEnemy.GetComponent<NetworkObject>();
        if (netObj != null) netObj.Spawn();
    }
}
