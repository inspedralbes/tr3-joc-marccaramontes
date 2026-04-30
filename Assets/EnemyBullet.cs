using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 5f;

    void Awake()
    {
        gameObject.tag = "EnemyBullet";
    }

    void Start()
    {
        // Se destruye automáticamente tras el tiempo de vida definido
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Se mueve en la dirección "up" del objeto (que habremos rotado hacia el jugador al disparar)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("<color=orange>¡Impacto de bala enemiga!</color>");
            PlayerMovement pm = collision.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.Die();
            }
            Destroy(gameObject); // Destruye la bala al impactar
        }
    }
}
