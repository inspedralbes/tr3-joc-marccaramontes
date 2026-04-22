using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsUIRegisterer : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject resultsPanel;
    public GameObject deathFlashOverlay; 
    public TextMeshProUGUI p1TimeText;
    public TextMeshProUGUI p2TimeText; // Restaurado para evitar error de compilación
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI timerHUDText; 
    public CanvasGroup hudGroup;        
    public TextMeshProUGUI killsHUDText; 
    public Button retryButton;          
    public Button menuButton;            

    private void Awake()
    {
        // Forzar activación del objeto para asegurar el registro
        gameObject.SetActive(true);
        TryRegister();
    }

    public void TryRegister()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterResultsUI(this);
            Debug.Log("<b>[ResultsUI]</b> Registrado en GameManager.");
        }
        else
        {
            Invoke("TryRegister", 0.1f);
        }
    }

    void Start()
    {
        // Solo ocultamos el panel, NO el CanvasResultados
        if (resultsPanel != null) resultsPanel.SetActive(false);
    }
}
