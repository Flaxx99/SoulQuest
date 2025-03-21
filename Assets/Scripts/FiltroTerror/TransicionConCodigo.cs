using System.Collections;
using UnityEngine;

public class TransicionConCodigo : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup canvasGroup;  // Referencia al CanvasGroup
    public GameObject efectoTerror;   // Filtro de terror (Canvas de filtro de miedo)
    public GameObject panelTexto;    // Panel de texto durante la transición
    public TMPro.TextMeshProUGUI textoTransicion; // Texto del panel de transición

    public float duracionTransicion = 1f;  // Duración de la transición (en segundos)
    public float tiempoPantallaNegra = 1f; // Tiempo de espera con pantalla negra

    private bool transicionRealizada = false;  // Validación para activar la transición solo una vez

    private void Start()
    {
        // Asegúrate de que el filtro de terror y el panel de texto estén desactivados al inicio
        efectoTerror.SetActive(false);  // El filtro de terror está desactivado al inicio
        panelTexto.SetActive(false);   // El panel de texto está oculto al principio
    }

    // Método para activar la transición
    public void ActivarTransicion()
    {
        if (!transicionRealizada)  // Solo ejecuta la transición si no ha sido realizada previamente
        {
            transicionRealizada = true;  // Marca que la transición ya ha ocurrido
            StartCoroutine(RealizarTransicion());
        }
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

        // 4️⃣ Aparece el efecto de terror inmediatamente
        Debug.Log("Activando filtro de terror...");  // Log para verificar activación
        efectoTerror.SetActive(true);  // Activa el filtro de terror

        // 5️⃣ Duración del efecto de terror
        yield return new WaitForSeconds(1f);  // Puedes ajustar el tiempo que se mantiene el filtro

        // 6️⃣ Vuelve a la normalidad (Fade out)
        yield return StartCoroutine(FadeCanvas(1f, 0f, duracionTransicion));

        // 7️⃣ Desactiva el panel de texto después de la transición
        panelTexto.SetActive(false);

        // Opcional: Desactivar el filtro de terror si ya no se necesita
        // efectoTerror.SetActive(false);
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
