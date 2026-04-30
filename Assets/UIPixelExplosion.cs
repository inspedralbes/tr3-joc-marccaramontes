using UnityEngine;
using UnityEngine.UI;

public class UIPixelExplosion : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystemRenderer psRenderer;

    public void Initialize(Color particleColor)
    {
        ps = gameObject.GetComponent<ParticleSystem>();
        if (ps == null) ps = gameObject.AddComponent<ParticleSystem>();
        
        psRenderer = gameObject.GetComponent<ParticleSystemRenderer>();
        if (psRenderer == null) psRenderer = gameObject.AddComponent<ParticleSystemRenderer>();

        ConfigureParticles(particleColor);
    }

    private void ConfigureParticles(Color color)
    {
        var main = ps.main;
        main.startLifetime = 0.8f;
        main.startSpeed = new ParticleSystem.MinMaxCurve(20f, 50f);
        main.startSize = 5f; // Ajustado para UI (píxeles)
        main.startColor = color;
        main.loop = false;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.stopAction = ParticleSystemStopAction.Destroy;
        main.useUnscaledTime = true;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Rectangle;
        shape.scale = new Vector3(100f, 50f, 1f); // Ajustar al tamaño del botón

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 100) });

        var velocity = ps.velocityOverLifetime;
        velocity.enabled = true;
        // IMPORTANTE: En Unity, los ejes X, Y y Z de la velocidad deben estar en el MISMO modo (ej. los tres en "Dos Constantes")
        velocity.x = new ParticleSystem.MinMaxCurve(-20f, 20f);  // Pequeña dispersión lateral
        velocity.y = new ParticleSystem.MinMaxCurve(-50f, -100f); // Caída vertical
        velocity.z = new ParticleSystem.MinMaxCurve(0f, 0f);      // Eje Z estático

        // Configuración del Renderer
        psRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        psRenderer.material = new Material(Shader.Find("Sprites/Default"));
        // Importante para UI: Sorting Layer
        psRenderer.sortingLayerName = "UI";
        psRenderer.sortingOrder = 100;
    }

    public void Play()
    {
        if (ps != null) ps.Play();
    }
}
