using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(NetworkObject))]
public class Enemy : NetworkBehaviour
{
    public enum EnemyType { Basic, Interceptor, Stalker }
    public EnemyType type = EnemyType.Basic;

    [Header("Movimiento")]
    public float speed = 3f;
    public float turnSpeed = 2f;
    public float predictionFactor = 0.5f; 
    public float orbitDistance = 10f;     
    public float orbitMargin = 2f;        

    [Header("Combate")]
    public GameObject bulletPrefab;
    public float fireRate = 3f;
    private float nextFireTime;

    private Vector3 currentDirection;
    private Transform player;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    void Awake()
    {
        gameObject.tag = "Enemy";
        rb = GetComponent<Rigidbody2D>();
        
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.linearDamping = 3f;
        rb.angularDamping = 10f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col != null) col.isTrigger = false;
    }

    public override void OnNetworkSpawn()
    {
        InitializeVisuals();
        
        if (IsServer)
        {
            FindPlayer();
            if (player != null)
                currentDirection = (player.position - transform.position).normalized;
            else
                currentDirection = Vector3.right;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
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
                renderer.material.SetColor("_OutlineColor", new Color(9f, 0f, 15f, 1f)); 
                renderer.color = new Color(0.6f, 0f, 1f, 1f);
                break;
            case EnemyType.Stalker:
                renderer.material.SetColor("_OutlineColor", new Color(15f, 6f, 0f, 1f));
                renderer.color = new Color(1.0f, 0.4f, 0f, 1f);
                break;
            default:
                renderer.material.SetColor("_OutlineColor", new Color(3f, 15f, 3f, 1f));
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
        if (!IsServer) return;

        if (player == null)
        {
            FindPlayer();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        var playerRenderer = player.GetComponent<SpriteRenderer>();
        if (playerRenderer != null && !playerRenderer.enabled)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        UpdateMovement();
        
        if (type == EnemyType.Stalker)
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
            if (distance > orbitDistance + orbitMargin) targetDirection = (player.position - transform.position).normalized;
            else if (distance < orbitDistance - orbitMargin) targetDirection = (transform.position - player.position).normalized;
            else
            {
                targetDirection = new Vector3(-targetDirection.y, targetDirection.x, 0);
                targetDirection += (player.position - transform.position).normalized * 0.2f;
            }
        }

        targetDirection.z = 0;
        currentDirection = Vector3.RotateTowards(currentDirection, targetDirection.normalized, turnSpeed * Time.fixedDeltaTime, 0f);
        
        float dot = Vector3.Dot(currentDirection.normalized, targetDirection.normalized);
        float speedModifier = Mathf.Max(0.2f, dot);

        float finalSpeed = speed;
        if (GameManager.Instance != null) finalSpeed *= GameManager.Instance.difficultyMultiplierValue;

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
        
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        var bulletNetObj = bullet.GetComponent<NetworkObject>();
        if (bulletNetObj != null) bulletNetObj.Spawn();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;
        CheckPlayerCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;
        CheckPlayerCollision(collision.gameObject);
    }

    private void CheckPlayerCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if (pm != null) pm.Die();
        }
    }
}
