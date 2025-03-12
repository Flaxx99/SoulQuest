using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinijuegoTangram : MonoBehaviour
{

    [SerializeField] private float tiempoLimite = 120f; // Tiempo en segundos
    [SerializeField] private TMP_Text textoTemporizador; // Para mostrar el tiempo restante
    private float tiempoRestante;
    private bool minijuegoActivo = false;
    public GameObject miniJuegoTangram;
    public TMP_Text resultadoTexto;
    public ActivarMinijuegoTangram triggerTangram; // Nueva referencia pública
    public Button botonAceptar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        botonAceptar.gameObject.SetActive(false); // Oculta el botón al inicio
        botonAceptar.onClick.AddListener(CerrarMiniJuego); // Asigna la función al botón
    }

    public void CerrarMiniJuego()
    {
        Debug.Log("CerrarMiniJuego() se ha ejecutado correctamente.");
        miniJuegoTangram.SetActive(false); // Oculta el minijuego
        minijuegoActivo = false; // Detiene el temporizador
        Time.timeScale = 1; // Reanuda el juego principal
    }


    public void ActivarMiniJuego()
    {
        Debug.Log("ActivarMiniJuego() ha sido llamado.");

        if (miniJuegoTangram == null)
        {
            Debug.LogError("⚠️ miniJuegoTangram NO está asignado en el Inspector.");
            return;
        }

        miniJuegoTangram.SetActive(true);
        Debug.Log($"✅ Estado de miniJuegoTangram después de SetActive(true): {miniJuegoTangram.activeSelf}");

        resultadoTexto.text = "";

        tiempoRestante = tiempoLimite;
        minijuegoActivo = true;
        textoTemporizador.text = $"Tiempo: {tiempoRestante:F1}s";
        Time.timeScale = 0;
    }



    private void Update()
    {
        if (minijuegoActivo)
        {
            tiempoRestante -= Time.unscaledDeltaTime;

            // Asegurar que el tiempo nunca sea menor a 0
            if (tiempoRestante < 0)
            {
                tiempoRestante = 0;
                TiempoTerminado(); // Llamar a la función de pérdida
            }

            // Actualizar el texto del temporizador
            textoTemporizador.text = $"Tiempo: {tiempoRestante:F1}s";
        }
    }
    private void TiempoTerminado()
    {
        if (!minijuegoActivo) return; // Evita que se ejecute si el jugador ya ganó

        minijuegoActivo = false; // Detiene el temporizador
        resultadoTexto.text = "¡Tiempo agotado! Has perdido.";
        resultadoTexto.color = Color.red;

        // Reducir la vida en 10 puntos solo si el jugador no ganó
        float nuevaVida = UIManager.Instance.VidaActual - 10;
        UIManager.Instance.ActualizarVidaPersonaje(nuevaVida, UIManager.Instance.VidaMax);

        // Configurar el botón correctamente
        botonAceptar.gameObject.SetActive(true);
        botonAceptar.onClick.RemoveAllListeners();

        if (nuevaVida <= 0)
        {
            resultadoTexto.text = "¡Has perdido toda tu vida! GAME OVER.";
            botonAceptar.GetComponentInChildren<TMP_Text>().text = "Salir";
            botonAceptar.onClick.AddListener(GameOver);
        }
        else
        {
            botonAceptar.GetComponentInChildren<TMP_Text>().text = "Reintentar";
            botonAceptar.onClick.AddListener(ReintentarMinijuego);
        }
    }


    private void ReintentarMinijuego()
    {
        resultadoTexto.text = "";
        botonAceptar.gameObject.SetActive(false);
        ActivarMiniJuego(); // Reinicia el minijuego sin resetear la vida
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER. El jugador ha perdido toda su vida.");
        UIManager.Instance.MostrarPantallaGameOver();
    }
}
