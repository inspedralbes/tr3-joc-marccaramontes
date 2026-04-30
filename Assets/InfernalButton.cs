using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InfernalButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UIThemeSO theme;
    public float hoverScale = 1.1f;
    public float animationDuration = 0.1f;

    private Vector3 originalScale;
    private TextMeshProUGUI buttonText;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        
        if (buttonText != null && theme != null)
        {
            buttonText.color = theme.bloodRed;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScale(originalScale * hoverScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScale(originalScale));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Disintegrate();
    }

    public void Disintegrate()
    {
        if (buttonText != null) buttonText.enabled = false;

        GameObject explosionGO = new GameObject("UIExplosion");
        explosionGO.transform.SetParent(transform.parent);
        explosionGO.transform.position = transform.position;
        
        UIPixelExplosion explosion = explosionGO.AddComponent<UIPixelExplosion>();
        explosion.Initialize(theme != null ? theme.bloodRed : Color.red);
        explosion.Play();
    }

    private System.Collections.IEnumerator AnimateScale(Vector3 targetScale)
    {
        float elapsed = 0f;
        Vector3 initialScale = rectTransform.localScale;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / animationDuration);
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }
}
