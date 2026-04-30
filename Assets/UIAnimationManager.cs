using UnityEngine;
using TMPro;
using System.Collections;

public class UIAnimationManager : MonoBehaviour
{
    public static UIAnimationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Aseguramos que el manager de animaciones persista si es necesario, 
            // aunque normalmente vivirá en el objeto _Managers de la escena.
            // DontDestroyOnLoad(gameObject); 
            Debug.Log("<b>[UIAnimationManager]</b> Instancia global establecida.");
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Realiza un Fade In/Out de un CanvasGroup usando tiempo real (unscaled).
    /// </summary>
    public IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float duration)
    {
        if (group == null) yield break;
        float elapsed = 0f;
        group.alpha = start;

        while (elapsed < duration)
        {
            if (group == null) yield break;
            elapsed += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        if (group != null) group.alpha = end;
    }

    /// <summary>
    /// Realiza un conteo numérico en un TextMeshProUGUI.
    /// </summary>
    public IEnumerator CountText(TextMeshProUGUI textElement, float start, float end, float duration, string prefix = "", string suffix = "", string format = "F2")
    {
        if (textElement == null) yield break;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            if (textElement == null) yield break;
            elapsed += Time.unscaledDeltaTime;
            float current = Mathf.Lerp(start, end, elapsed / duration);
            textElement.text = $"{prefix}{current.ToString(format)}{suffix}";
            yield return null;
        }

        if (textElement != null) textElement.text = $"{prefix}{end.ToString(format)}{suffix}";
    }

    /// <summary>
    /// Realiza un efecto de pulso (escala) en un Transform.
    /// </summary>
    public IEnumerator PulseScale(Transform target, float scaleAmount, float duration)
    {
        if (target == null) yield break;
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * scaleAmount;
        float halfDuration = duration / 2f;

        // Scale Up
        float elapsed = 0f;
        while (elapsed < halfDuration)
        {
            if (target == null) yield break;
            elapsed += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / halfDuration);
            yield return null;
        }

        // Scale Down
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            if (target == null) yield break;
            elapsed += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / halfDuration);
            yield return null;
        }

        if (target != null) target.localScale = originalScale;
    }
}
