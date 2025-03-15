using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
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
