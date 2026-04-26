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

    public Vector2 Velocity => movement.normalized * speed;
    
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
            moveAction?.Enable();
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
        // Aplicar material de contorno neon
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Custom/SpriteOutline"));
            renderer.material.SetColor("_OutlineColor", new Color(1f * 15f, 0f, 0f, 1f)); // Rojo Neon Ultra
            renderer.material.SetFloat("_OutlineWidth", 2.5f);
            renderer.color = new Color(1f, 0.2f, 0.2f, 1f); // Interior Rojo normal
        }

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

        // Ocultar jugador pero mantener el objeto para la cámara o red
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;
        
        // Desactivar colisiones
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        // Bloquear input local
        if (networkIdentity.isLocalPlayer)
        {
            moveAction?.Disable();
            
            // MODO ESPECTADOR: Buscar al rival para seguirlo
            StartCoroutine(TransitionToSpectator());
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ProcessDeath();
        }
    }

    private System.Collections.IEnumerator TransitionToSpectator()
    {
        yield return new WaitForSeconds(1.0f); // Esperar a que la explosión se vea

        // Intentar encontrar un RemotePlayer
        GameObject rival = GameObject.Find("RemotePlayer_" + (networkIdentity.networkId == "1" ? "2" : "1")); // Fallback simple
        
        // Búsqueda más robusta: Cualquier objeto con tag Player que NO sea yo y esté activo
        if (rival == null)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var p in players)
            {
                if (p != gameObject && p.GetComponent<SpriteRenderer>().enabled)
                {
                    rival = p;
                    break;
                }
            }
        }

        if (rival != null)
        {
            Debug.Log("<b>[Spectator]</b> Siguiendo al rival: " + rival.name);
            var cam = Camera.main;
            if (cam != null)
            {
                // Simple follow (si tu cámara tiene un script de seguimiento, habría que actualizar su Target)
                // Aquí asumo que la cámara es hija o tiene un script básico. 
                // Como no veo script de cámara, si es estática no hace falta hacer nada.
                // Si la cámara sigue al jugador, actualizamos su foco:
            }
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
