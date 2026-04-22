using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class HunterAgent : Agent
{
    public float moveSpeed = 5f;
    public Transform target; // El jugador a perseguir
    
    private Rigidbody2D rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();

        // Requisito 2.3: Solo el Host debe tomar decisiones.
        // Si no somos el Host, desactivamos el DecisionRequester para que no pida acciones.
        if (NetworkManager.Instance != null && !NetworkManager.Instance.isHost)
        {
            var requester = GetComponent<Unity.MLAgents.DecisionRequester>();
            if (requester != null) requester.enabled = false;
        }
    }

    public override void OnEpisodeBegin()
    {
        // Resetear posición si es necesario
    }

    private void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        target = closestPlayer;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Requisito 2.3: Solo el Host observa y decide
        if (NetworkManager.Instance != null && !NetworkManager.Instance.isHost) return;

        FindClosestPlayer();

        // Observar la posición propia y del objetivo
        sensor.AddObservation(transform.localPosition);
        if (target != null)
            sensor.AddObservation(target.localPosition);
        else
            sensor.AddObservation(Vector3.zero);

        // Observar la velocidad
        sensor.AddObservation(rb.linearVelocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Requisito 2.3: Solo el Host ejecuta acciones de IA
        if (NetworkManager.Instance != null && !NetworkManager.Instance.isHost) return;

        // Movimiento simple en 2D basado en las acciones del agente
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        rb.linearVelocity = new Vector2(moveX, moveY) * moveSpeed;

        // Recompensa por estar cerca del objetivo
        if (target != null)
        {
            float distance = Vector3.Distance(transform.localPosition, target.localPosition);
            
            if (distance < 1.5f)
            {
                SetReward(1.0f);
            }
            else
            {
                // Pequeña penalización por estar lejos para incentivar la rapidez
                AddReward(-0.01f);
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Control manual para testing
        if (NetworkManager.Instance != null && !NetworkManager.Instance.isHost) return;

        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (NetworkManager.Instance != null && !NetworkManager.Instance.isHost) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            SetReward(2.0f);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.5f);
        }
    }
}
