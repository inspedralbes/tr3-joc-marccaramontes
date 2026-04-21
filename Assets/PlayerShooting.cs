using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkIdentity))]
public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 2f; 
    private float nextFireTime = 0f;

    private PlayerInput playerInput;
    private InputAction attackAction;
    private NetworkIdentity networkIdentity;

    void Awake()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            attackAction = playerInput.actions["Attack"];
        }
    }

    void Start()
    {
        if (!networkIdentity.isLocalPlayer)
        {
            if (NetworkManager.Instance != null)
            {
                NetworkManager.Instance.OnRemotePlayerShot += RemoteShoot;
            }
        }
    }

    void Update()
    {
        if (!networkIdentity.isLocalPlayer) return;

        // BLOQUEO: No disparar si la partida ha terminado
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        bool isAttacking = false;
        if (attackAction != null)
        {
            isAttacking = attackAction.IsPressed();
        }
        else
        {
            isAttacking = Input.GetButton("Fire1") || Input.GetMouseButton(0);
        }

        if (isAttacking && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 shootDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        Instantiate(bulletPrefab, transform.position, rotation);

        // Notificar red
        if (NetworkManager.Instance != null && NetworkManager.Instance.currentRoomId != "")
        {
            NetworkManager.Instance.Emit("player_shoot", new ShootData {
                roomId = NetworkManager.Instance.currentRoomId,
                x = transform.position.x,
                y = transform.position.y,
                rotation = angle - 90f
            });
        }
    }

    private void RemoteShoot(string playerId, Vector3 pos, float rot)
    {
        if (playerId == networkIdentity.networkId)
        {
            Instantiate(bulletPrefab, pos, Quaternion.AngleAxis(rot, Vector3.forward));
        }
    }

    [System.Serializable]
    public class ShootData
    {
        public string roomId;
        public float x;
        public float y;
        public float rotation;
    }
}
