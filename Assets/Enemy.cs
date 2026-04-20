using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float turnSpeed = 2f;
    private Vector3 currentDirection;
    private Transform player;

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
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning($"El enemigo {gameObject.name} no encontró al jugador. Asegúrate de que el jugador tenga el Tag 'Player'.");
        }
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // 1. Calculamos la dirección deseada hacia el jugador (solo en 2D)
        Vector3 targetDirection = (player.position - transform.position).normalized;
        targetDirection.z = 0;

        // 2. Rotamos la dirección actual hacia la deseada gradualmente
        currentDirection = Vector3.RotateTowards(currentDirection, targetDirection, turnSpeed * Time.deltaTime, 0f);
        currentDirection.z = 0;

        // 3. Calculamos la penalización por giro (Dot Product)
        // Dot product es 1 si van en la misma dirección, -1 si son opuestas.
        // Usamos Max(0.2f, dot) para que no se detengan por completo al girar.
        float dot = Vector3.Dot(currentDirection.normalized, targetDirection.normalized);
        float speedModifier = Mathf.Max(0.2f, dot);

        // 4. Aplicamos el movimiento
        transform.position += currentDirection.normalized * (speed * speedModifier) * Time.deltaTime;
        
        // Aseguramos que Z se mantenga en 0
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡Enemigo alcanzó al jugador!");
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Die();
            }
        }
    }
}
