using TMPro;
using UnityEngine;
using UnityEngine.UI; // Necesario para usar el botón

public class MiniJuegoSpanish : MonoBehaviour
{
    [SerializeField] private float tiempoLimite = 20f; // Tiempo en segundos
    [SerializeField] private TMP_Text textoTemporizador; // Para mostrar el tiempo restante
    private float tiempoRestante;
    private bool minijuegoActivo = false;

    public GameObject miniJuegoCanvas;
    public TMP_InputField[] inputFields;
    public TMP_Text resultadoTexto;
    public string[] respuestasCorrectas = { "pantera", "gato", "leopardo", "iguana", "raton" };
    public ActivarMinijuegoSpanish triggerSpanish; // Nueva referencia pública
    public Button botonAceptar;
    private bool juegoPausado = false;

    void Start()
    {
        botonAceptar.gameObject.SetActive(false); // Oculta el botón al inicio
        botonAceptar.onClick.AddListener(CerrarMiniJuego); // Asigna la función al botón
    }
    private void Update()
    {
        if (minijuegoActivo && !juegoPausado) // 👈 Solo descuenta tiempo si NO está pausado
        {
            tiempoRestante -= Time.unscaledDeltaTime;

            if (tiempoRestante < 0)
            {
                tiempoRestante = 0;
                TiempoTerminado();
            }

            textoTemporizador.text = $"Tiempo: {tiempoRestante:F1}s";
        }
    }


    public void ComprobarRespuestas()
    {
        Debug.Log("Botón comprobar presionado. Revisando respuestas...");
        bool todasCorrectas = true;

        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i].text.ToLower().Trim() != respuestasCorrectas[i])
            {
                todasCorrectas = false;
            }
        }

        if (todasCorrectas)
        {
            Debug.Log("Todas las respuestas son correctas.");
            resultadoTexto.text = "¡Felicidades! Todas las respuestas son correctas.";
            resultadoTexto.color = Color.green;

            // 🚀 *Detener el temporizador*
            minijuegoActivo = false;
            tiempoRestante = 0f;
            textoTemporizador.text = "";

            // Convertir el botón en "Salir" solo si se gana
            botonAceptar.GetComponentInChildren<TMP_Text>().text = "Salir";
            botonAceptar.onClick.RemoveAllListeners();
            botonAceptar.onClick.AddListener(CerrarMiniJuego);
            botonAceptar.gameObject.SetActive(true);

            PersonajeExperiencia personajeExp = Object.FindFirstObjectByType<PersonajeExperiencia>();

            if (personajeExp != null)
            {
                personajeExp.AnadirExperiencia(50);
            }
            else
            {
                Debug.LogWarning("PersonajeExperiencia no encontrado. No se pudo otorgar experiencia.");
            }
        }
        else
        {
            Debug.Log("Algunas respuestas son incorrectas.");
            resultadoTexto.text = "Algunas respuestas son incorrectas, ¡intenta de nuevo!";
            resultadoTexto.color = Color.red;
        }
    }


    public void CerrarMiniJuego()
    {
        Debug.Log("CerrarMiniJuego() se ha ejecutado correctamente.");
        miniJuegoCanvas.SetActive(false);
        minijuegoActivo = false;
        Time.timeScale = 1; // Reanuda el juego principal

        // 🔊 Volver a la música de pasillos
        AudioManager.instancia.CambiarMusica("Pasillos");
    }

    public void ActivarMiniJuego()
    {
        Debug.Log("ActivarMiniJuego() ha sido llamado.");
        miniJuegoCanvas.SetActive(true);
        resultadoTexto.text = "";

        tiempoRestante = tiempoLimite; // Reiniciar temporizador cada vez que se abre el minijuego
        minijuegoActivo = true; // Permite que Update() comience a descontar tiempo

        textoTemporizador.text = $"Tiempo: {tiempoRestante:F1}s"; // Actualizar UI al inicio

        Time.timeScale = 0; // Pausar el juego principal mientras el minijuego está activo
        // 🔊 Asegurar que la música del minijuego se reproduzca
        AudioManager.instancia.CambiarMusica("Minijuego");
    }


    private void TiempoTerminado()
    {
        if (!minijuegoActivo) return; // 🚀 Evita que se ejecute si el jugador ya ganó

        minijuegoActivo = false; // Detiene el temporizador
        resultadoTexto.text = "¡Tiempo agotado! Has perdido.";
        resultadoTexto.color = Color.red;

        // Reducir la vida en 10 puntos solo si el jugador no ganó
        float nuevaVida = UIManager.Instance.VidaActual - 10;
        UIManager.Instance.ActualizarVidaPersonaje(nuevaVida, UIManager.Instance.VidaMax);

        // Reproducir la música nuevamente
        AudioManager.instancia.CambiarMusica("Minijuego");

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

        foreach (TMP_InputField inputField in inputFields)
        {
            inputField.text = "";
        }
        // 🔊 Asegurar que la música se siga reproduciendo
        AudioManager.instancia.CambiarMusica("Minijuego");

        ActivarMiniJuego(); // Reinicia el minijuego sin resetear la vida
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER. El jugador ha perdido toda su vida.");
        UIManager.Instance.MostrarPantallaGameOver();
    }
    public void PausarMinijuego()
    {
        Debug.Log("⏸ Minijuego pausado.");
        juegoPausado = true;
    }

    public void ReanudarMinijuego()
    {
        Debug.Log("▶ Minijuego reanudado.");
        juegoPausado = false;
    }

}