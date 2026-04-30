using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AEA.Networking;

public class RemotePlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private Dictionary<string, GhostData> remotePlayers = new Dictionary<string, GhostData>();

    private class GhostData
    {
        public GameObject gameObject;
        public Vector3 targetPosition;
        public Quaternion targetRotation;
    }

    public float lerpSpeed = 10f;

    private void OnEnable()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnRemotePlayerMoved += UpdateRemotePlayer;
            NetworkManager.Instance.OnRemotePlayerJoined += SpawnRemotePlayer;
            NetworkManager.Instance.OnRemotePlayerLeft += RemoveRemotePlayer;
        }
    }

    private void OnDisable()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnRemotePlayerMoved -= UpdateRemotePlayer;
            NetworkManager.Instance.OnRemotePlayerJoined -= SpawnRemotePlayer;
            NetworkManager.Instance.OnRemotePlayerLeft -= RemoveRemotePlayer;
        }
    }

    private void SpawnRemotePlayer(string playerId, string playerName)
    {
        if (string.IsNullOrEmpty(playerId) || remotePlayers.ContainsKey(playerId)) return;
        
        // Don't spawn ghost for local player
        if (NetworkManager.Instance != null && playerId == NetworkManager.Instance.localPlayerId) return;

        GameObject ghostGo = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        
        // Configurar identidad como remota (Legacy - NGO gestiona esto ahora)
        // Pero lo mantenemos para no romper el script si aún se usa el prefab viejo
        var id = ghostGo.GetComponent<NetworkIdentity>();
        if (id != null)
        {
            id.Setup(playerId, false, false);
        }

        // Forzar nombre y estado cinemático para evitar conflictos de física
        ghostGo.name = "RemotePlayer_" + playerId;
        Rigidbody2D rb = ghostGo.GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

        // DESACTIVAR SCRIPTS DE CONTROL LOCAL
        MonoBehaviour movement = ghostGo.GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;
        
        MonoBehaviour shooting = ghostGo.GetComponent<PlayerShooting>();
        if (shooting != null) shooting.enabled = false;
        
        remotePlayers.Add(playerId, new GhostData { 
            gameObject = ghostGo, 
            targetPosition = Vector3.zero, 
            targetRotation = Quaternion.identity 
        });
        Debug.Log($"[RemotePlayerManager] Spawned ghost for {playerName} ({playerId})");
    }

    private void UpdateRemotePlayer(string playerId, Vector3 position, float rotation)
    {
        if (remotePlayers.TryGetValue(playerId, out GhostData data))
        {
            data.targetPosition = position;
            data.targetRotation = Quaternion.Euler(0, 0, rotation);
        }
        else
        {
            Debug.Log($"[RemotePlayerManager] Recibido movimiento de jugador desconocido {playerId}. Spawneando...");
            SpawnRemotePlayer(playerId, playerId);
            
            // Re-intentar obtener los datos tras el spawn para aplicar la posición inmediatamente
            if (remotePlayers.TryGetValue(playerId, out GhostData newData))
            {
                newData.targetPosition = position;
                newData.targetRotation = Quaternion.Euler(0, 0, rotation);
                
                // Snap inicial: Posicionar el objeto físicamente de inmediato
                if (newData.gameObject != null)
                {
                    newData.gameObject.transform.position = position;
                    newData.gameObject.transform.rotation = Quaternion.Euler(0, 0, rotation);
                }
            }
        }
    }

    private void RemoveRemotePlayer(string playerId)
    {
        if (remotePlayers.TryGetValue(playerId, out GhostData data))
        {
            Debug.Log($"[RemotePlayerManager] Removing ghost for {playerId}");
            if (data.gameObject != null) Destroy(data.gameObject);
            remotePlayers.Remove(playerId);
        }
    }

    private void Update()
    {
        foreach (var ghost in remotePlayers.Values)
        {
            if (ghost.gameObject == null) continue;
            ghost.gameObject.transform.position = Vector3.Lerp(ghost.gameObject.transform.position, ghost.targetPosition, Time.deltaTime * lerpSpeed);
            ghost.gameObject.transform.rotation = Quaternion.Lerp(ghost.gameObject.transform.rotation, ghost.targetRotation, Time.deltaTime * lerpSpeed);
        }
    }
}
