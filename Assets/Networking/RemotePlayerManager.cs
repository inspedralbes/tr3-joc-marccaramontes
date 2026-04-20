using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnRemotePlayerMoved += UpdateRemotePlayer;
            NetworkManager.Instance.OnRemotePlayerJoined += SpawnRemotePlayer;
        }
    }

    private void SpawnRemotePlayer(string playerId)
    {
        if (remotePlayers.ContainsKey(playerId)) return;

        GameObject ghostGo = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkIdentity id = ghostGo.GetComponent<NetworkIdentity>();
        id.Setup(playerId, false, false);
        
        remotePlayers.Add(playerId, new GhostData { 
            gameObject = ghostGo, 
            targetPosition = Vector3.zero, 
            targetRotation = Quaternion.identity 
        });
        Debug.Log($"[RemotePlayerManager] Spawned ghost for {playerId}");
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
            SpawnRemotePlayer(playerId);
        }
    }

    private void Update()
    {
        foreach (var ghost in remotePlayers.Values)
        {
            ghost.gameObject.transform.position = Vector3.Lerp(ghost.gameObject.transform.position, ghost.targetPosition, Time.deltaTime * lerpSpeed);
            ghost.gameObject.transform.rotation = Quaternion.Lerp(ghost.gameObject.transform.rotation, ghost.targetRotation, Time.deltaTime * lerpSpeed);
        }
    }
}
