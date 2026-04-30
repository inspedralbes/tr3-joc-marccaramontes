using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public float transitionDelay = 0.8f;

    // Función para el modo un solo jugador
    public void PlaySolo()
    {
        Debug.Log("Intentando iniciar modo Solo...");
        StartCoroutine(DescentTransition(() => {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame(GameMode.Solo);
            }
            else
            {
                Debug.LogError("No se encontró GameManager.Instance");
            }
        }));
    }

    // Función para el modo multijugador online (antes local por turnos)
    public void PlayMultiplayer()
    {
        Debug.Log("Redirigiendo a Multijugador Online...");
        StartCoroutine(DescentTransition(() => PlayOnline()));
    }

    public void PlayOnline()
    {
        Debug.Log("Navegando al Lobby Online...");
        SceneManager.LoadScene("Lobby");
    }

    // Función para cerrar la aplicación
    public void QuitGame()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

    private IEnumerator DescentTransition(System.Action onComplete)
    {
        // El InfernalButton ya dispara la explosión al hacer click
        // Aquí solo esperamos el tiempo de la animación antes de cambiar
        yield return new WaitForSecondsRealtime(transitionDelay);
        onComplete?.Invoke();
    }
}
