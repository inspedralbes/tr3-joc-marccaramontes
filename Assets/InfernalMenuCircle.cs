using UnityEngine;

public class InfernalMenuCircle : MonoBehaviour
{
    [Header("Animation Settings")]
    public float breatheSpeed = 2f;
    public float breatheAmount = 0.05f; // 5% scale change
    public bool pulseOnHover = true;

    private Vector3 originalScale;
    private float timer;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Sine wave for breathing effect
        timer += Time.unscaledDeltaTime * breatheSpeed;
        float scaleOffset = Mathf.Sin(timer) * breatheAmount;
        transform.localScale = originalScale * (1f + scaleOffset);
    }

    public void Pulse()
    {
        // Logic for a more aggressive pulse if needed (e.g., on click)
        timer = 0f;
    }
}
