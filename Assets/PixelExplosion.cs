using UnityEngine;

public class PixelExplosion : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
        if (ps == null) ps = gameObject.AddComponent<ParticleSystem>();
        
        ConfigureParticles();
    }

    private void ConfigureParticles()
    {
        var main = ps.main;
        main.startLifetime = 0.5f;
        main.startSpeed = 10f;
        main.startSize = 0.15f;
        main.loop = false;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.stopAction = ParticleSystemStopAction.Destroy;
        main.useUnscaledTime = true; // Crítico: Time.timeScale será 0

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 50) });

        var renderer = GetComponent<ParticleSystemRenderer>();
        if (renderer == null) renderer = gameObject.AddComponent<ParticleSystemRenderer>();
        
        // Usar un material simple de color sólido (Default-UI o similar)
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        
        // Forzar partículas cuadradas
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
    }

    public void Play()
    {
        if (ps != null) ps.Play();
    }
}
