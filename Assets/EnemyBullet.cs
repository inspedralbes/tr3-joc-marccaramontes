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
        InitializeVisuals();
        // Se destruye automáticamente tras el tiempo de vida definido
        Destroy(gameObject, lifeTime);
    }

    private void InitializeVisuals()
    {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            // Instanciar material para no afectar al asset global (opcional pero recomendado para personalización por bala)
            renderer.material = new Material(Shader.Find("Custom/SpriteOutline"));
            
            // Color Naranja Neon Intenso (HDR)
            Color neonOrange = new Color(1.0f, 0.45f, 0f, 1f);
            renderer.material.SetColor("_OutlineColor", neonOrange * 15f);
            renderer.material.SetFloat("_OutlineWidth", 2.5f);
            renderer.color = Color.white; // Interior blanco para contraste
        }
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
