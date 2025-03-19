using System.Collections;
using UnityEngine;
using TMPro;

public class TransicionConCodigo : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup canvasGroup;  // Referencia al CanvasGroup
    public GameObject efectoTerror;   // Filtro de terror (Canvas de filtro de miedo)
    public GameObject panelTexto;    // Panel de texto durante la transición
    public TextMeshProUGUI textoTransicion; // Texto del panel de transición

    public float duracionTransicion = 0.5f;  // Duración de la transición (en segundos)
    public float tiempoPantallaNegra = 0.5f; // Tiempo de espera con pantalla negra

    private void Start()
    {
        // Al inicio, el filtro de terror está desactivado
        efectoTerror.SetActive(false);
        panelTexto.SetActive(false); // Asegúrate de que el panel de texto esté oculto al principio
    }

    // Método para activar la transición
    public void ActivarTransicion()
    {
        StartCoroutine(RealizarTransicion());
    }

    private IEnumerator RealizarTransicion()
    {
        // 1️⃣ Pantalla negra (Fade in)
        yield return StartCoroutine(FadeCanvas(0f, 1f, duracionTransicion));

        // 2️⃣ Muestra el texto durante la transición
        panelTexto.SetActive(true);  // Activa el panel de texto
        textoTransicion.text = "Algo extraño está sucediendo..."; // Cambia la frase como desees

        // 3️⃣ Espera con pantalla negra y texto
        yield return new WaitForSeconds(tiempoPantallaNegra);

        // 4️⃣ Activa el efecto de terror
        efectoTerror.SetActive(true);

        // 5️⃣ Duración del efecto de terror (si quieres que dure un tiempo específico)
        yield return new WaitForSeconds(3f);  // Ajusta el tiempo que desees

        // Si deseas desactivarlo después del tiempo
        // efectoTerror.SetActive(false);

        // 6️⃣ Vuelve a la normalidad (Fade out)
        yield return StartCoroutine(FadeCanvas(1f, 0f, duracionTransicion));

        // Desactiva el panel de texto
        panelTexto.SetActive(false);
    }

    // Función para hacer el fade (desaparecer o aparecer la pantalla negra)
    private IEnumerator FadeCanvas(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        canvasGroup.alpha = startAlpha;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
