using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsUIRegisterer : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject resultsPanel;
    public GameObject deathFlashOverlay; 
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI p1TimeText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI timerHUDText; 
    public CanvasGroup hudGroup;        
    public TextMeshProUGUI killsHUDText; 
    public Button retryButton;          
    public Button menuButton;            

    private void Awake()
    {
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
        if (resultsPanel != null) resultsPanel.SetActive(false);
    }
}
