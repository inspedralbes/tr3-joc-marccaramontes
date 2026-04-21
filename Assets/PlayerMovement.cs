using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkIdentity))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float platformRadius = 2.5f; 
    public Transform platformCenter; 
    
    [Header("Network")]
    public float networkSendRate = 0.1f; 
    private float lastNetworkSendTime;

    private Rigidbody2D rb;
    private Vector2 movement;
    private NetworkIdentity networkIdentity;
    
    private PlayerInput playerInput;
    private InputAction moveAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        networkIdentity = GetComponent<NetworkIdentity>();
        playerInput = GetComponent<PlayerInput>();

        // Validación de Input System
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
            if (moveAction == null) Debug.LogWarning("<color=red><b>[PlayerMovement]</b> Acción 'Move' no encontrada en PlayerInput. Revisa el archivo de acciones.</color>");
        }
        // Eliminado aviso de fallback para limpiar consola

        // Auto-asignación de autoridad local si no hay red activa
        bool isNetworkActive = NetworkManager.Instance != null && !string.IsNullOrEmpty(NetworkManager.Instance.currentRoomId);
        
        if (!isNetworkActive)
        {
            networkIdentity.isLocalPlayer = true;
            networkIdentity.hasAuthority = true;
        }
        
        if (networkIdentity.isLocalPlayer)
        {
            gameObject.name = "LocalPlayer";
            gameObject.tag = "Player";
        }
        else
        {
            gameObject.name = "RemotePlayer_" + networkIdentity.networkId;
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void Start()
    {
        SetupPlatform();
    }

    void SetupPlatform()
    {
        if (platformCenter == null)
        {
            GameObject platformObj = GameObject.FindGameObjectWithTag("Platform");
            if (platformObj != null) platformCenter = platformObj.transform;
        }
        if (platformCenter != null && networkIdentity.isLocalPlayer) 
            transform.position = platformCenter.position;
    }

    void Update()
    {
        if (!networkIdentity.isLocalPlayer) return;

        // BLOQUEO: No mover si la partida ha terminado o está en transición
        if (GameManager.Instance != null && (GameManager.Instance.IsGameOver || GameManager.Instance.currentState == GameState.DeathTransition))
        {
            movement = Vector2.zero;
            return;
        }

        if (moveAction != null)
        {
            movement = moveAction.ReadValue<Vector2>();
        }
        else
        {
            // Bloqueado en Unity 6 si se activa el New Input System
            movement = Vector2.zero;
        }
        
        CheckBounds();
        HandleNetworkSync();
    }

    void FixedUpdate()
    {
        if (!networkIdentity.isLocalPlayer) return;
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    void HandleNetworkSync()
    {
        if (Time.time - lastNetworkSendTime > networkSendRate)
        {
            lastNetworkSendTime = Time.time;
            if (NetworkManager.Instance != null)
            {
                NetworkManager.Instance.Emit("update_position", new PositionUpdate
                {
                    roomId = NetworkManager.Instance.currentRoomId,
                    x = transform.position.x,
                    y = transform.position.y,
                    rotation = transform.rotation.eulerAngles.z
                });
            }
        }
    }

    void CheckBounds()
    {
        Vector2 center = platformCenter != null ? (Vector2)platformCenter.position : Vector2.zero;
        float distanceSqr = ((Vector2)transform.position - center).sqrMagnitude;

        if (distanceSqr > platformRadius * platformRadius)
        {
            Die();
        }
    }

    public void Die()
    {
        // Efecto visual estilo Devil Daggers
        GameObject explosionGo = new GameObject("PixelExplosion");
        explosionGo.transform.position = transform.position;
        explosionGo.AddComponent<PixelExplosion>().Play();

        // Ocultar jugador
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;
        
        // Desactivar colisiones para evitar múltiples muertes
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ProcessDeath();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    [System.Serializable]
    public class PositionUpdate
    {
        public string roomId;
        public float x;
        public float y;
        public float rotation;
    }
}
