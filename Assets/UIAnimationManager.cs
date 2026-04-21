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
        float elapsed = 0f;
        group.alpha = start;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        group.alpha = end;
    }

    /// <summary>
    /// Realiza un conteo numérico en un TextMeshProUGUI.
    /// </summary>
    public IEnumerator CountText(TextMeshProUGUI textElement, float start, float end, float duration, string prefix = "", string suffix = "", string format = "F2")
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float current = Mathf.Lerp(start, end, elapsed / duration);
            textElement.text = $"{prefix}{current.ToString(format)}{suffix}";
            yield return null;
        }

        textElement.text = $"{prefix}{end.ToString(format)}{suffix}";
    }
}
