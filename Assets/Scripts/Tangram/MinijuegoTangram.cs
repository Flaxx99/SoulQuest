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

    public void CheckSolution()
    {
        // Llamamos a la validación de las piezas en el TangramValidator
        tangramValidator.CheckSolution();

        // Verifica si todas las piezas están correctas
        if (tangramValidator.piezasCorrectas == tangramValidator.posicionesObjetivo.Count)
        {
            // Mostrar mensaje de "¡Felicidades!"
            resultadoTexto.text = "¡Felicidades! Todas las piezas están correctamente posicionadas.";
            resultadoTexto.color = Color.green;

            // 🚀 **Detener el temporizador**
            minijuegoActivo = false; // Desactivamos el minijuego, por lo que el temporizador dejará de actualizarse

            // Cambiar el botón a "Salir" solo si se gana
            botonAceptar.GetComponentInChildren<TMP_Text>().text = "Salir";
            botonAceptar.gameObject.SetActive(true);
            botonAceptar.onClick.RemoveAllListeners();
            botonAceptar.onClick.AddListener(CerrarMiniJuego); // Salir del minijuego
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
        if (!minijuegoActivo) return; // 🚀 Evita que se ejecute si el jugador ya ganó

        minijuegoActivo = false; // Detener el temporizador
        resultadoTexto.text = "¡Tiempo agotado! Has perdido.";  // Muestra el mensaje de que se ha agotado el tiempo
        resultadoTexto.color = Color.red;

        // Reducir la vida en 10 puntos solo si el jugador no ganó
        float nuevaVida = UIManager.Instance.VidaActual - 10;
        UIManager.Instance.ActualizarVidaPersonaje(nuevaVida, UIManager.Instance.VidaMax);  // Reducir vida

        // Reproducir la música nuevamente
        AudioManager.instancia.CambiarMusica("Minijuego");

        // Configurar el botón correctamente
        botonAceptar.gameObject.SetActive(true); // Mostrar el botón
        botonAceptar.onClick.RemoveAllListeners(); // Limpiar listeners previos

        if (nuevaVida <= 0)
        {
            resultadoTexto.text = "¡Has perdido toda tu vida! GAME OVER.";
            botonAceptar.GetComponentInChildren<TMP_Text>().text = "Salir";
            botonAceptar.onClick.AddListener(GameOver); // Llamar a GameOver si la vida llega a 0
        }
        else
        {
            botonAceptar.GetComponentInChildren<TMP_Text>().text = "Reintentar";
            botonAceptar.onClick.AddListener(ReintentarMinijuego); // Configura el botón para reintentar
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

    public void ActivarMiniJuego()
    {
        miniJuegoTangram.SetActive(true);
        resultadoTexto.text = "";

        tiempoRestante = tiempoLimite; // Reinicia el temporizador
        minijuegoActivo = true; // Permite que Update() comience a descontar tiempo

        textoTemporizador.text = $"Tiempo: {tiempoRestante:F1}s"; // Actualizar la UI al inicio

        Time.timeScale = 0; // Pausar el juego principal
        AudioManager.instancia.CambiarMusica("Minijuego"); // Reproducir música del minijuego
    }

    // Función para cerrar el minijuego
    public void CerrarMiniJuego()
    {
        miniJuegoTangram.SetActive(false); // Ocultar el canvas del minijuego
        minijuegoActivo = false; // Desactivar el minijuego
        Time.timeScale = 1; // Reanudar el tiempo del juego principal
        AudioManager.instancia.CambiarMusica("Pasillos"); // Volver a la música de fondo
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
