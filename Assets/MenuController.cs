using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Función para el modo un solo jugador
    public void PlaySolo()
    {
        Debug.Log("Intentando iniciar modo Solo...");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(GameMode.Solo);
        }
        else
        {
            Debug.LogError("No se encontró GameManager.Instance");
        }
    }

    // Función para el modo multijugador online (antes local por turnos)
    public void PlayMultiplayer()
    {
        Debug.Log("Redirigiendo a Multijugador Online...");
        PlayOnline();
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
}
