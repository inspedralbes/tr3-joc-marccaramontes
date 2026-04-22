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
    }

    public override void OnEpisodeBegin()
    {
        // Resetear posición si es necesario
        // transform.localPosition = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
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
                // EndEpisode(); // Podríamos terminar el episodio si "atrapa" al jugador
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
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
