using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NetworkObject))]
public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    public float platformRadius = 2.5f; 
    public Transform platformCenter; 
    
    private Rigidbody2D rb;
    private Vector2 movement;

    public Vector2 Velocity => movement.normalized * speed;
    
    private PlayerInput playerInput;
    private InputAction moveAction;

    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        if (IsOwner)
        {
            gameObject.name = "LocalPlayer";
            gameObject.tag = "Player";
            
            if (playerInput != null)
            {
                moveAction = playerInput.actions["Move"];
                moveAction?.Enable();
            }

            SetupPlatform();
        }
        else
        {
            gameObject.name = "RemotePlayer_" + OwnerClientId;
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
            if (playerInput != null) playerInput.enabled = false;
        }

        // Fallback para luz
        EnsureLight();
    }

    private void EnsureLight()
    {
        var light = GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
        if (light == null || light.lightType == UnityEngine.Rendering.Universal.Light2D.LightType.Global)
        {
            GameObject lightGo = new GameObject("TorchLight_Fallback", typeof(UnityEngine.Rendering.Universal.Light2D));
            lightGo.transform.SetParent(transform);
            lightGo.transform.localPosition = Vector3.zero;
            var l = lightGo.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            l.lightType = UnityEngine.Rendering.Universal.Light2D.LightType.Point;
            l.pointLightOuterRadius = 8f;
            l.intensity = 1.0f;
            l.color = new Color(1f, 0.8f, 0.53f, 1f);
        }
    }

    void SetupPlatform()
    {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Custom/SpriteOutline"));
            renderer.material.SetColor("_OutlineColor", new Color(15f, 0f, 0f, 1f));
            renderer.material.SetFloat("_OutlineWidth", 2.5f);
            renderer.color = new Color(1f, 0.2f, 0.2f, 1f);
        }

        if (platformCenter == null)
        {
            GameObject platformObj = GameObject.FindGameObjectWithTag("Platform");
            if (platformObj != null) platformCenter = platformObj.transform;
        }
        if (platformCenter != null && IsOwner) 
            transform.position = platformCenter.position;
    }

    void Update()
    {
        if (!IsOwner) return;

        if (GameManager.Instance != null && (GameManager.Instance.IsGameOver || GameManager.Instance.currentState == GameState.DeathTransition))
        {
            movement = Vector2.zero;
            return;
        }

        if (moveAction != null)
        {
            movement = moveAction.ReadValue<Vector2>();
        }
        
        CheckBounds();
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
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
        if (!IsOwner) return;

        // Notificar al GameManager local
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ProcessDeath();
        }

        // RPC para visuales en otros clientes si fuera necesario, 
        // pero aquí solo ocultamos y spawneamos explosión localmente para feedback inmediato.
        SpawnDeathVisualsServerRpc();
        
        // El Host debería encargarse de la limpieza oficial si fuera necesario.
    }

    [ServerRpc]
    private void SpawnDeathVisualsServerRpc()
    {
        SpawnDeathVisualsClientRpc();
    }

    [ClientRpc]
    private void SpawnDeathVisualsClientRpc()
    {
        GameObject explosionGo = new GameObject("PixelExplosion");
        explosionGo.transform.position = transform.position;
        explosionGo.AddComponent<PixelExplosion>().Play();

        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;
        
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
    }
}
