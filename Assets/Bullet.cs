using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f;

    void Awake()
    {
        gameObject.tag = "Bullet";
    }

    void Start()
    {
        // Se destruye automáticamente tras el tiempo de vida definido
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Se mueve en la dirección "up" del objeto (que habremos rotado hacia el ratón)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Impacto en enemigo: " + collision.gameObject.name);
            if (GameManager.Instance != null) GameManager.Instance.AddKill();
            Destroy(collision.gameObject);
            Destroy(gameObject); // Destruye la bala al impactar
        }
    }
}
