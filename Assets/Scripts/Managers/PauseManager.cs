using UnityEngine;
using UnityEngine.SceneManagement; // 📌 Importante para cambiar de escena

public class PausaManager : MonoBehaviour
{
    public static bool JuegoPausado = false;
    public GameObject menuPausaUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        if (menuPausaUI != null)
        {
            menuPausaUI.SetActive(true);
        }
        Time.timeScale = 0f;
        JuegoPausado = true;
    }

    public void Reanudar()
    {
        if (menuPausaUI != null)
        {
            menuPausaUI.SetActive(false);
        }
        Time.timeScale = 1f;
        JuegoPausado = false;
    }

    // 🚀 **Nueva función para cargar el Menú Principal**
    public void IrAlMenuPrincipal()
    {
        Debug.Log("🔄 Cargando MainMenu...");
        Time.timeScale = 1f; // Reanudar el tiempo

        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.CambiarMusica("MainMenu");
        }

        SceneManager.LoadScene("MainMenu");
    }

}
