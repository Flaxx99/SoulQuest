using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MinijuegoTangram : MonoBehaviour
{
    [SerializeField] private float tiempoLimite = 120f; // Tiempo en segundos
    [SerializeField] private TMP_Text textoTemporizador; // Para mostrar el tiempo restante
    private float tiempoRestante;
    private bool minijuegoActivo = false;

    public GameObject miniJuegoTangram;
    public TMP_Text resultadoTexto;
    public Button botonAceptar;
    public Button botonComprobar;
    public TangramValidator tangramValidator; // Referencia al script TangramValidator
    private bool juegoPausado = false;

    public GameObject PanelBotones;
    public GameObject PanelArmaEquipada;
    void Start()
    {
        botonAceptar.gameObject.SetActive(false); // Oculta el botón al inicio
        botonAceptar.onClick.AddListener(CerrarMiniJuego); // Asigna la función al botón

        botonComprobar.onClick.AddListener(CheckSolution); // Asocia el botón de "Comprobar" a la función CheckSolution
    }

    private void Update()
    {
        if (minijuegoActivo && !juegoPausado) // Solo descuenta tiempo si está activo
        {
            tiempoRestante -= Time.unscaledDeltaTime;

            if (tiempoRestante < 0)
            {
                tiempoRestante = 0;
                TiempoTerminado();
            }

            textoTemporizador.text = $"Tiempo: {tiempoRestante:F1}s"; // Actualiza el tiempo restante
        }
    }

    private bool experienciaOtorgada = false; // Nueva variable para controlar la experiencia

    public void CheckSolution()
    {
        tangramValidator.CheckSolution();

        if (tangramValidator.piezasCorrectas == tangramValidator.posicionesObjetivo.Count)
        {
            resultadoTexto.text = "¡Felicidades! Todas las piezas están correctamente posicionadas.";
            resultadoTexto.color = Color.green;

            minijuegoActivo = false;

            botonAceptar.GetComponentInChildren<TMP_Text>().text = "Salir";
            botonAceptar.gameObject.SetActive(true);
            botonAceptar.onClick.RemoveAllListeners();
            botonAceptar.onClick.AddListener(CerrarMiniJuego);

            // ✅ Asegurar que solo se otorga experiencia UNA VEZ por victoria
            if (!experienciaOtorgada)
            {
                PersonajeExperiencia personajeExp = Object.FindFirstObjectByType<PersonajeExperiencia>();

                if (personajeExp != null)
                {
                    personajeExp.AnadirExperiencia(50);
                    experienciaOtorgada = true; // Bloquea la experiencia hasta un nuevo intento
                }
                else
                {
                    Debug.LogWarning("PersonajeExperiencia no encontrado. No se pudo otorgar experiencia.");
                }
            }
        }
        else
        {
            resultadoTexto.text = "Algunas piezas están incorrectas, ¡intenta de nuevo!";
            resultadoTexto.color = Color.red;
        }
    }



    // Método para detener el temporizador
    private void DetenerTemporizador()
    {
        if (minijuegoActivo)
        {
            minijuegoActivo = false;  // Detener la actualización del temporizador
            tiempoRestante = 0f;
            textoTemporizador.text = $"Tiempo: {tiempoRestante:F1}s"; // Actualiza el texto del temporizador
        }
    }
    private void TiempoTerminado()
    {
        if (!minijuegoActivo) return;

        minijuegoActivo = false;
        resultadoTexto.text = "¡Tiempo agotado! Has perdido.";
        resultadoTexto.color = Color.red;

        // ✅ Ocultar el botón "Comprobar" cuando el tiempo se acaba
        botonComprobar.gameObject.SetActive(false);

        // 🔄 Reiniciar las piezas del Tangram cuando el tiempo se agote
        tangramValidator.ReiniciarTangram();

        // 🚫 Bloquear la interacción con las piezas
        foreach (var posicion in tangramValidator.posicionesObjetivo)
        {
            if (posicion.piezaAsignada != null)
            {
                posicion.piezaAsignada.BloquearInteraccion();
            }
        }

        float nuevaVida = Mathf.Max(UIManager.Instance.VidaActual - 10, 0); // Asegura que la vida no baje de 0
        UIManager.Instance.ActualizarVidaPersonaje(nuevaVida, UIManager.Instance.VidaMax);
        AudioManager.instancia.CambiarMusica("Minijuego");

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
        botonComprobar.gameObject.SetActive(true);

        tangramValidator.ReiniciarTangram(); // Reiniciar la posición de las piezas

        foreach (var posicion in tangramValidator.posicionesObjetivo)
        {
            if (posicion.piezaAsignada != null)
            {
                posicion.piezaAsignada.PermitirInteraccion();
            }
        }

        // ✅ Permitir ganar experiencia solo si el jugador gana después del reintento
        experienciaOtorgada = false;

        AudioManager.instancia.CambiarMusica("Minijuego");

        ActivarMiniJuego();
    }



    private void GameOver()
    {
        Debug.Log("GAME OVER. El jugador ha perdido toda su vida.");
        UIManager.Instance.MostrarPantallaGameOver();
    }

   public void ActivarMiniJuego()
    {
        miniJuegoTangram.SetActive(true);
        resultadoTexto.text = "";

        tiempoRestante = tiempoLimite; // Reinicia el temporizador
        minijuegoActivo = true; // Permite que Update() comience a descontar tiempo
        PanelBotones.SetActive(false);
        PanelArmaEquipada.SetActive(false);
        textoTemporizador.text = $"Tiempo: {tiempoRestante:F1}s"; // Actualizar la UI al inicio

        // Usar tiempo no escalado para que las interacciones de UI no se vean afectadas
        Time.timeScale = 1; // Asegúrate de que el juego no esté pausado
        AudioManager.instancia.CambiarMusica("Minijuego"); // Reproducir música del minijuego
    }


    // Función para cerrar el minijuego
    public void CerrarMiniJuego()
    {
        miniJuegoTangram.SetActive(false); // Ocultar el canvas del minijuego
        minijuegoActivo = false; // Desactivar el minijuego
        Time.timeScale = 1; // Reanudar el tiempo del juego principal
        AudioManager.instancia.CambiarMusica("Pasillos"); // Volver a la música de fondo
        PanelBotones.SetActive(true);
        PanelArmaEquipada.SetActive(true);

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




