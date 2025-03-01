using System.Collections;
using TMPro;
using UnityEngine;

public class MiniJuegoSpanish : MonoBehaviour
{
    public GameObject miniJuegoCanvas;
    public TMP_InputField[] inputFields;
    public TMP_Text resultadoTexto;
    public string[] respuestasCorrectas = { "pantera", "gato", "leopardo", "iguana", "raton" };
    public ActivarMinijuegoSpanish triggerSpanish; // Nueva referencia pública


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

            CerrarMiniJuego();
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
        miniJuegoCanvas.SetActive(false); // Oculta el minijuego
        Debug.Log("miniJuegoCanvas ha sido desactivado.");
        Time.timeScale = 1; // Reanuda el juego
    }


    public void ActivarMiniJuego()
    {
        Debug.Log("ActivarMiniJuego() ha sido llamado.");
        miniJuegoCanvas.SetActive(true);
        resultadoTexto.text = "";
        Time.timeScale = 0;
    }

}
