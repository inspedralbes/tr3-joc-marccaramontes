using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Escalado")]
    public float hoverScale = 1.1f;
    public float animationDuration = 0.1f;
    
    [Header("Colores")]
    public Color hoverTextColor = Color.red;
    private Color originalTextColor;
    
    private Vector3 originalScale;
    private TextMeshProUGUI buttonText;
    private Coroutine currentCoroutine;

    void Awake()
    {
        originalScale = transform.localScale;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null) originalTextColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScale(originalScale * hoverScale));
        if (buttonText != null) buttonText.color = hoverTextColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScale(originalScale));
        if (buttonText != null) buttonText.color = originalTextColor;
    }

    private System.Collections.IEnumerator AnimateScale(Vector3 targetScale)
    {
        float elapsed = 0f;
        Vector3 initialScale = transform.localScale;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / animationDuration);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
