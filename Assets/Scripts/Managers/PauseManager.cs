using UnityEngine;
using UnityEngine.SceneManagement; // 📌 Importante para cambiar de escena

public class PausaManager : MonoBehaviour
{
    public static bool JuegoPausado = false;
    public GameObject menuPausaUI;

    private MiniJuegoSpanish miniJuegoSpanish;
    private MinijuegoTangram miniJuegoTangram;
    //private MiniJuegoQuemados miniJuegoQuemados;


    void Start()
    {
        miniJuegoSpanish = FindFirstObjectByType<MiniJuegoSpanish>();
        miniJuegoTangram = FindFirstObjectByType<MinijuegoTangram>();
        // miniJuegoQuemados = FindFirstObjectByType<MiniJuegoQuemados>();  // Si tienes este minijuego, descoméntalo
    }


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

        // 🔥 Pausar todos los minijuegos si están activos
        if (miniJuegoSpanish != null) miniJuegoSpanish.PausarMinijuego();
        if (miniJuegoTangram != null) miniJuegoTangram.PausarMinijuego();
        //if (miniJuegoQuemados != null) miniJuegoQuemados.PausarMinijuego();
    }


    public void Reanudar()
    {
        if (menuPausaUI != null)
        {
            menuPausaUI.SetActive(false);
        }

        Time.timeScale = 1f;
        JuegoPausado = false;

        // 🔥 Reanudar todos los minijuegos si están activos
        if (miniJuegoSpanish != null) miniJuegoSpanish.ReanudarMinijuego();
        if (miniJuegoTangram != null) miniJuegoTangram.ReanudarMinijuego();
        //if (miniJuegoQuemados != null) miniJuegoQuemados.ReanudarMinijuego();
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
