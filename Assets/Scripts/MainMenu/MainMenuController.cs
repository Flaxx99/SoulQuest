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
        Application.Quit();
    }
}
