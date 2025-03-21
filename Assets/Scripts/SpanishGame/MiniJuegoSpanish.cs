using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniJuegoSpanish : MonoBehaviour
{
    [SerializeField] private float tiempoLimite = 20f;
    [SerializeField] private TMP_Text textoTemporizador;
    private float tiempoRestante;
    private bool minijuegoActivo = false;

    public GameObject miniJuegoCanvas;
    public TMP_InputField[] inputFields;
    public TMP_Text resultadoTexto;
    public Button botonAceptar;
    public Button botonComprobar; // Botón de comprobar respuestas
    private bool juegoPausado = false;


    public GameObject PanelBotones;
    public GameObject PanelArmaEquipada;

    private Dictionary<string, string> anagramasDeAnimales = new Dictionary<string, string>()
    {
        {"pantera", "trapean"},
        {"gato", "toga"},
        {"leopardo", "peor lado"},
        {"iguana", "guaina"},
        {"raton", "notar"},
        {"ballena", "llenaba"},
        {"burro", "rubor"},
        {"cerdo", "cedro"},
        {"cobra", "barco"},
        {"gacela", "acelga"},
        {"gorila", "gloria"},
        {"jirafa", "fijara"},
        {"morsa", "ramos"},
        {"orca", "caro"},
        {"paloma", "aploma"},
        {"serpiente", "presiente"},
        {"tigre", "grite"}
    };

    private string[] palabrasSeleccionadas;
    private string[] respuestasCorrectas;

    void Start()
    {
        botonAceptar.gameObject.SetActive(false);
        botonComprobar.gameObject.SetActive(true);
        botonAceptar.onClick.AddListener(CerrarMiniJuego);
        botonComprobar.onClick.AddListener(ComprobarRespuestas);
    }

    private void Update()
    {
        if (minijuegoActivo && !juegoPausado)
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

    private void SeleccionarPalabrasAleatorias()
    {
        List<string> claves = new List<string>(anagramasDeAnimales.Keys);
        palabrasSeleccionadas = new string[inputFields.Length];
        respuestasCorrectas = new string[inputFields.Length];

        for (int i = 0; i < inputFields.Length; i++)
        {
            int randomIndex = Random.Range(0, claves.Count);
            string respuestaCorrecta = claves[randomIndex]; // La palabra correcta
            string anagrama = anagramasDeAnimales[respuestaCorrecta]; // El anagrama a mostrar

            palabrasSeleccionadas[i] = anagrama; // El anagrama es lo que se muestra
            respuestasCorrectas[i] = respuestaCorrecta; // Guardamos la respuesta correcta

            claves.RemoveAt(randomIndex);
        }
    }

    public void ActivarMiniJuego()
    {
        Debug.Log("ActivarMiniJuego() ha sido llamado.");
        miniJuegoCanvas.SetActive(true);
        resultadoTexto.text = "";

        tiempoRestante = tiempoLimite;
        minijuegoActivo = true;
        PanelBotones.SetActive(false);
        PanelArmaEquipada.SetActive(false);

        SeleccionarPalabrasAleatorias();

        // Asegurar que cada anagrama se muestre en la UI y los InputField estén vacíos
        for (int i = 0; i < inputFields.Length; i++)
        {
            TMP_Text textoAnagrama = inputFields[i].transform.parent.GetComponentInChildren<TMP_Text>();
            if (textoAnagrama != null)
            {
                textoAnagrama.text = palabrasSeleccionadas[i]; // Mostrar el anagrama en la UI
            }

            inputFields[i].text = ""; // Dejar el campo de entrada vacío
        }

        botonComprobar.gameObject.SetActive(true);
        botonAceptar.gameObject.SetActive(false);

        textoTemporizador.text = $"Tiempo: {tiempoRestante:F1}s";
        Time.timeScale = 0;
        AudioManager.instancia.CambiarMusica("Minijuego");
    }

    private bool experienciaOtorgada = false; // Variable local en lugar de PlayerPrefs

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

            minijuegoActivo = false;
            tiempoRestante = 0f;
            textoTemporizador.text = "";

            botonComprobar.gameObject.SetActive(false);
            botonAceptar.GetComponentInChildren<TMP_Text>().text = "Salir";
            botonAceptar.onClick.RemoveAllListeners();
            botonAceptar.onClick.AddListener(CerrarMiniJuego);
            botonAceptar.gameObject.SetActive(true);

            // ✅ Solo otorgar experiencia UNA VEZ por victoria
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
            Debug.Log("Algunas respuestas son incorrectas.");
            resultadoTexto.text = "Algunas respuestas son incorrectas, ¡intenta de nuevo!";
            resultadoTexto.color = Color.red;
        }
    }

    private void TiempoTerminado()
    {
        if (!minijuegoActivo) return;

        minijuegoActivo = false;
        resultadoTexto.text = "¡Tiempo agotado! Has perdido.";
        resultadoTexto.color = Color.red;

        botonComprobar.gameObject.SetActive(false); // ✅ Ocultar botón Comprobar cuando se acaba el tiempo
        botonAceptar.gameObject.SetActive(true);

        float nuevaVida = Mathf.Max(UIManager.Instance.VidaActual - 10, 0); // Asegura que la vida no baje de 0

        UIManager.Instance.ActualizarVidaPersonaje(nuevaVida, UIManager.Instance.VidaMax);

        AudioManager.instancia.CambiarMusica("Minijuego");

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

        foreach (TMP_InputField inputField in inputFields)
        {
            inputField.text = "";
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

    public void CerrarMiniJuego()
    {
        miniJuegoCanvas.SetActive(false);
        minijuegoActivo = false;
        Time.timeScale = 1;
        PanelBotones.SetActive(true);
        PanelArmaEquipada.SetActive(true);
        AudioManager.instancia.CambiarMusica("Pasillos");
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
