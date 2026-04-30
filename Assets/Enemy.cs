using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : MonoBehaviour
{
    public enum EnemyType { Basic, Interceptor, Stalker }
    public EnemyType type = EnemyType.Basic;

    [Header("Movimiento")]
    public float speed = 3f;
    public float turnSpeed = 2f;
    public float predictionFactor = 0.5f; // Para el Interceptor
    public float orbitDistance = 10f;     // Para el Stalker
    public float orbitMargin = 2f;        // Margen de órbita

    [Header("Combate")]
    public GameObject bulletPrefab;
    public float fireRate = 3f;
    private float nextFireTime;

    private Vector3 currentDirection;
    private Transform player;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private NetworkIdentity networkIdentity;

    void Awake()
    {
        gameObject.tag = "Enemy";
        rb = GetComponent<Rigidbody2D>();
        networkIdentity = GetComponent<NetworkIdentity>();
        
        // Configuración de físicas
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.linearDamping = 3f;
        rb.angularDamping = 10f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col != null) col.isTrigger = false;
    }

    void Start()
    {
        FindPlayer();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        if (player != null)
            currentDirection = (player.position - transform.position).normalized;
        else
            currentDirection = Vector3.right;

        InitializeVisuals();

        // Suscribirse a disparos si soy un cliente
        if (NetworkManager.Instance != null && !NetworkManager.Instance.isHost)
        {
            NetworkManager.Instance.OnEnemyShot += RemoteShoot;
        }
    }

    public void InitializeVisuals()
    {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        renderer.material = new Material(Shader.Find("Custom/SpriteOutline"));
        renderer.material.SetFloat("_OutlineWidth", 2.5f);
        
        switch (type)
        {
            case EnemyType.Interceptor:
                renderer.material.SetColor("_OutlineColor", new Color(0.6f * 15f, 0f, 1f * 15f, 1f)); // Púrpura Neon
                renderer.color = new Color(0.6f, 0f, 1f, 1f);
                break;
            case EnemyType.Stalker:
                renderer.material.SetColor("_OutlineColor", new Color(1.0f * 15f, 0.4f * 15f, 0f, 1f)); // Naranja Neon
                renderer.color = new Color(1.0f, 0.4f, 0f, 1f);
                break;
            default:
                renderer.material.SetColor("_OutlineColor", new Color(0.2f * 15f, 1f * 15f, 0.2f * 15f, 1f)); // Verde Neon
                renderer.color = new Color(0.2f, 1f, 0.2f, 1f);
                break;
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerMovement = playerObj.GetComponent<PlayerMovement>();
        }
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            FindPlayer();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        // Si el jugador está muerto/oculto, no hacemos nada
        var playerRenderer = player.GetComponent<SpriteRenderer>();
        if (playerRenderer != null && !playerRenderer.enabled)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        UpdateMovement();
        
        // Solo el Host o modo Solo gestiona los disparos
        bool isSolo = NetworkManager.Instance == null || string.IsNullOrEmpty(NetworkManager.Instance.currentRoomId);
        bool isHost = NetworkManager.Instance != null && NetworkManager.Instance.isHost;

        if (type == EnemyType.Stalker && (isSolo || isHost))
        {
            HandleShooting();
        }
    }

    private void UpdateMovement()
    {
        Vector3 targetPos = player.position;
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 targetDirection = (targetPos - transform.position).normalized;

        if (type == EnemyType.Interceptor && playerMovement != null)
        {
            targetPos = player.position + (Vector3)playerMovement.Velocity * (distance * predictionFactor * 0.1f);
            targetDirection = (targetPos - transform.position).normalized;
        }
        else if (type == EnemyType.Stalker)
        {
            if (distance > orbitDistance + orbitMargin)
            {
                // Demasiado lejos: Acercarse
                targetDirection = (player.position - transform.position).normalized;
            }
            else if (distance < orbitDistance - orbitMargin)
            {
                // Demasiado cerca: Alejarse
                targetDirection = (transform.position - player.position).normalized;
            }
            else
            {
                // En rango: Orbitar (perpendicular)
                targetDirection = new Vector3(-targetDirection.y, targetDirection.x, 0);
                
                // Añadir una pequeña fuerza de acoso hacia el centro para mantener la órbita
                targetDirection += (player.position - transform.position).normalized * 0.2f;
            }
        }

        targetDirection.z = 0;
        currentDirection = Vector3.RotateTowards(currentDirection, targetDirection.normalized, turnSpeed * Time.fixedDeltaTime, 0f);
        
        float dot = Vector3.Dot(currentDirection.normalized, targetDirection.normalized);
        float speedModifier = Mathf.Max(0.2f, dot);

        float finalSpeed = speed;
        if (GameManager.Instance != null) finalSpeed *= GameManager.Instance.difficultyMultiplier;

        if (rb != null) rb.linearVelocity = currentDirection.normalized * (finalSpeed * speedModifier);
    }

    private void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null) return;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Instantiate(bulletPrefab, transform.position, rotation);

        // Notificar red
        if (NetworkManager.Instance != null && NetworkManager.Instance.isHost)
        {
            NetworkManager.Instance.Emit("ENEMY_SHOOT", new NetworkManager.EnemyShootData {
                enemyId = networkIdentity != null ? networkIdentity.networkId : "",
                x = transform.position.x,
                y = transform.position.y,
                rotation = angle
            });
        }
    }

    private void RemoteShoot(string id, Vector3 pos, float rotation)
    {
        if (networkIdentity != null && networkIdentity.networkId == id)
        {
            if (bulletPrefab != null)
            {
                Instantiate(bulletPrefab, pos, Quaternion.Euler(0, 0, rotation));
            }
        }
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckPlayerCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckPlayerCollision(collision.gameObject);
    }

    private void CheckPlayerCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=red>¡Muerte por contacto!</color>");
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if (pm != null) pm.Die();
        }
    }
}
