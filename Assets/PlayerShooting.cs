using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkObject))]
public class PlayerShooting : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 2f; 
    private float nextFireTime = 0f;

    private PlayerInput playerInput;
    private InputAction attackAction;

    private Color[] neonColors = new Color[] {
        new Color(1f, 0f, 0f),    // Rojo
        new Color(0f, 1f, 0f),    // Verde
        new Color(0f, 0.5f, 1f),  // Azul
        new Color(1f, 1f, 0f),    // Amarillo
        new Color(1f, 0f, 1f),    // Magenta
        new Color(0f, 1f, 1f)     // Cian
    };
    private int bulletColorIndex = 0;

    public override void OnNetworkSpawn()
    {
        playerInput = GetComponent<PlayerInput>();
        if (IsOwner && playerInput != null)
        {
            attackAction = playerInput.actions["Attack"];
            attackAction?.Enable();
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        bool isAttacking = false;
        if (attackAction != null)
        {
            isAttacking = attackAction.IsPressed();
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

        Vector2 mousePos = Vector2.zero;
        if (Mouse.current != null)
        {
            mousePos = Mouse.current.position.ReadValue();
        }
        else return;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        worldMousePosition.z = 0;
        
        Vector3 shootDirection = (worldMousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        // Nuevo formato de RPC en Unity 6
        ShootServerRpc(transform.position, angle - 90f, bulletColorIndex);
        
        bulletColorIndex = (bulletColorIndex + 1) % neonColors.Length;
    }

    [Rpc(SendTo.Server)]
    private void ShootServerRpc(Vector3 pos, float rot, int colorIdx)
    {
        GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.AngleAxis(rot, Vector3.forward));
        var bulletNetworkObject = bullet.GetComponent<NetworkObject>();
        if (bulletNetworkObject != null)
        {
            bulletNetworkObject.Spawn();
            ApplyBulletVisualsClientRpc(bulletNetworkObject.NetworkObjectId, colorIdx);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void ApplyBulletVisualsClientRpc(ulong bulletId, int colorIdx)
    {
        if (Unity.Netcode.NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(bulletId, out NetworkObject bulletObj))
        {
            SpriteRenderer bulletRenderer = bulletObj.GetComponent<SpriteRenderer>();
            if (bulletRenderer != null)
            {
                bulletRenderer.material = new Material(Shader.Find("Custom/SpriteOutline"));
                Color baseColor = neonColors[colorIdx];
                bulletRenderer.material.SetColor("_OutlineColor", baseColor * 15f);
                bulletRenderer.material.SetFloat("_OutlineWidth", 3.0f);
                bulletRenderer.color = baseColor;
            }
        }
    }
}
