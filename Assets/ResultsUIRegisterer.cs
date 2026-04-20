using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsUIRegisterer : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject resultsPanel;
    public TextMeshProUGUI p1TimeText;
    public TextMeshProUGUI p2TimeText;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI timerHUDText; 
    public Button retryButton;          
    public Button menuButton;            

    private void Awake()
    {
        Debug.Log($"<b>[ResultsUI]</b> Despertando en el objeto: {gameObject.name}");
    }

    void Start()
    {
        // Intentamos registrar al inicio
        TryRegister();
        
        // Ocultar panel por seguridad si está asignado
        if (resultsPanel != null) resultsPanel.SetActive(false);
    }

    public void TryRegister()
    {
        if (GameManager.Instance != null)
        {
            if (resultsPanel == null)
            {
                Debug.LogWarning("<b>[ResultsUI]</b> No se puede registrar: 'resultsPanel' es nulo. Ejecuta 'Tools/Setup HUD and Results UI'.");
                return;
            }

            GameManager.Instance.RegisterResultsUI(this);
        }
        else
        {
            // Si el GameManager no está listo, lo intentamos un poco más tarde
            Invoke("TryRegister", 0.1f);
        }
    }
}