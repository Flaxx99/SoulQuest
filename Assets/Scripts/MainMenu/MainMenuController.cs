using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.instancia != null)
        {
            Debug.Log("Cambiando música a MainMenu...");
            AudioManager.instancia.CambiarMusica("MainMenu");
        }
        else
        {
            Debug.LogError("No se encontró el AudioManager.");
        }
    }


    public void PlayGame()
    {
        Debug.Log("🎮 Iniciando juego desde el Menú...");

        // 🔥 Asegurar que la música de los pasillos suene al entrar al juego
        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.CambiarMusica("Pasillos");
        }

        SceneManager.LoadScene("OriginalYoshy"); // Reemplaza con el nombre real de tu escena de juego.
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");

        // Cierra el juego en compilación
        Application.Quit();

        // Solo para el editor de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }
}
