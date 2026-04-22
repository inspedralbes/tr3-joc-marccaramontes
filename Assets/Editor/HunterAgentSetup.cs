using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;

# if UNITY_EDITOR
using UnityEditor;

public class HunterAgentSetup : MonoBehaviour
{
    [MenuItem("Tools/ML-Agents/Setup Hunter Agent")]
    public static void Setup()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null) return;

        // Add or get Agent script
        var agent = selected.GetComponent<HunterAgent>();
        if (agent == null) agent = selected.AddComponent<HunterAgent>();

        // Add RayPerceptionSensor2D
        var sensor = selected.GetComponent<RayPerceptionSensorComponent2D>();
        if (sensor == null) sensor = selected.AddComponent<RayPerceptionSensorComponent2D>();
        
        // Configure Sensor
        sensor.SensorName = "HunterSensor";
        sensor.DetectableTags = new System.Collections.Generic.List<string> { "Player", "Wall" };
        sensor.RayLength = 10f;
        sensor.RaysPerDirection = 3;
        sensor.MaxRayDegrees = 70;

        // Add Decision Requester
        var decision = selected.GetComponent<DecisionRequester>();
        if (decision == null) decision = selected.AddComponent<DecisionRequester>();
        decision.DecisionPeriod = 5;

        Debug.Log("Hunter Agent configured successfully on " + selected.name);
    }
}
# endif
