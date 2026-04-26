using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : MonoBehaviour
{
    public enum EnemyType { Basic, Interceptor }
    public EnemyType type = EnemyType.Basic;

    public float speed = 3f;
    public float turnSpeed = 2f;
    public float predictionFactor = 0.5f; // Factor de predicción para el Interceptor

    private Vector3 currentDirection;
    private Transform player;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    void Awake()
    {
        gameObject.tag = "Enemy";
        rb = GetComponent<Rigidbody2D>();
        
        // Configuración de físicas para empujones naturales
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.linearDamping = 3f; // Un poco más de fricción para que no patinen tanto
        rb.angularDamping = 10f; // Evita rotaciones raras al chocar
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Forzamos que el collider NO sea trigger para que choquen entre ellos
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col != null) col.isTrigger = false;
    }

    void Start()
    {
        FindPlayer();
        
        // Aseguramos que el enemigo empiece en Z=0 para ser visible
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        // Inicializamos la dirección hacia el jugador si existe
        if (player != null)
        {
            currentDirection = (player.position - transform.position).normalized;
            currentDirection.z = 0;
        }
        else
        {
            currentDirection = Vector3.right;
        }

        // Aplicar material de contorno neon
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Custom/SpriteOutline"));
            renderer.material.SetFloat("_OutlineWidth", 2.5f);
            
            if (type == EnemyType.Interceptor)
            {
                renderer.material.SetColor("_OutlineColor", new Color(0.6f * 15f, 0f, 1f * 15f, 1f)); // Púrpura Neon
                renderer.color = new Color(0.6f, 0f, 1f, 1f); // Interior Púrpura normal
            }
            else
            {
                renderer.material.SetColor("_OutlineColor", new Color(0.2f * 15f, 1f * 15f, 0.2f * 15f, 1f)); // Verde Neon
                renderer.color = new Color(0.2f, 1f, 0.2f, 1f); // Interior Verde normal
            }
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
        else
        {
            Debug.LogWarning($"El enemigo {gameObject.name} no encontró al jugador. Asegúrate de que el jugador tenga el Tag 'Player'.");
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

        Vector3 targetPos = player.position;

        // Lógica de Intercepción
        if (type == EnemyType.Interceptor && playerMovement != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            targetPos = player.position + (Vector3)playerMovement.Velocity * (distance * predictionFactor * 0.1f);
        }

        // 1. Calculamos la dirección deseada hacia el target
        Vector3 targetDirection = (targetPos - transform.position).normalized;
        targetDirection.z = 0;

        // 2. Rotamos la dirección actual hacia la deseada
        currentDirection = Vector3.RotateTowards(currentDirection, targetDirection, turnSpeed * Time.fixedDeltaTime, 0f);
        currentDirection.z = 0;

        // 3. Calculamos la penalización por giro (Dot Product)
        float dot = Vector3.Dot(currentDirection.normalized, targetDirection.normalized);
        float speedModifier = Mathf.Max(0.2f, dot);

        // 4. Aplicamos el movimiento usando VELOCIDAD
        float finalSpeed = speed;
        if (GameManager.Instance != null) finalSpeed *= GameManager.Instance.difficultyMultiplier;

        // Establecemos la velocidad directamente para permitir que el motor de físicas
        // resuelva colisiones y empujones lateralmente.
        if (rb != null) rb.linearVelocity = currentDirection.normalized * (finalSpeed * speedModifier);
    }

    void Update()
    {
        // Aseguramos que Z se mantenga en 0 para visualización
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
