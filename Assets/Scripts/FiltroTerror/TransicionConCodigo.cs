using System.Collections;
using UnityEngine;

public class TransicionConCodigo : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup canvasGroup;               // Referencia al CanvasGroup
    public GameObject efectoTerror;               // Filtro de terror (Canvas de filtro de miedo)
    public AudioClip musicaTransicion;            // El clip de audio para la transición
    public GameObject panelTexto;                 // Panel de texto durante la transición
    public TMPro.TextMeshProUGUI textoTransicion;  // Texto del panel de transición

    public float duracionTransicion = 1f;         // Duración de la transición (en segundos)
    public float tiempoPantallaNegra = 1f;         // Tiempo de espera con pantalla negra

    private bool transicionRealizada = false;     // Para activar la transición solo una vez

    private void Start()
    {
        // Desactivar inicialmente el filtro de terror y el panel de texto
        efectoTerror.SetActive(false);
        panelTexto.SetActive(false);

    }

    // Método para activar la transición
    public void ActivarTransicion()
    {
        if (!transicionRealizada)
        {
            transicionRealizada = true;

            panelTexto.SetActive(true);

            // Detener la música actual del AudioManager para evitar interferencias
            if (AudioManager.instancia != null)
            {

               //AudioManager.instancia.GetAudioSource().Stop();
                Debug.Log("AudioManager detenido para reproducir el audio de transición.");
            }

            // Reproducir el audio de transición usando el AudioSource global del AudioManager
            if (AudioManager.instancia != null && musicaTransicion != null)
            {
                AudioSource globalSource = AudioManager.instancia.GetAudioSource();
                if (globalSource.volume <= 0f)
                {
                    globalSource.volume = 1f;
                    Debug.Log("Forzado: volumen del AudioSource ajustado a 1.");
                }
                Debug.Log("Reproduciendo audio de transición.");
                globalSource.PlayOneShot(musicaTransicion);
            }
            else
            {
                Debug.LogError("AudioManager o clip de transición no asignado.");
            }

            StartCoroutine(RealizarTransicion());
        }
    }

    private IEnumerator RealizarTransicion()
    {
        // 1️⃣ Fade in: pantalla negra
        yield return StartCoroutine(FadeCanvas(0f, 1f, duracionTransicion));

        // 2️⃣ Mostrar panel de texto con el mensaje
        panelTexto.SetActive(true);
        textoTransicion.text = "Algo extraño está sucediendo...";

        // 3️⃣ Espera con pantalla negra y texto
        yield return new WaitForSeconds(tiempoPantallaNegra);

        // 4️⃣ Activar el efecto de terror
        Debug.Log("Activando filtro de terror...");
        efectoTerror.SetActive(true);

        // 5️⃣ Mantener el efecto de terror durante 3 segundos
        yield return new WaitForSeconds(3f);

        // 6️⃣ Fade out: volver a la normalidad
        yield return StartCoroutine(FadeCanvas(1f, 0f, duracionTransicion));

        // 7️⃣ Desactivar el panel de texto
        panelTexto.SetActive(false);

        // 8️⃣ En lugar de detener el audio, se deja que el clip de transición se reproduzca completamente.
        // Si se desea forzar el fin del clip, se podría agregar una espera adicional igual a la duración restante del clip.
        // Por ejemplo:
        // yield return new WaitForSeconds(musicaTransicion.length - tiempoTranscurrido);

        // 9️⃣ Reanudar la música de fondo
        ReanudarMusicaDeFondo();
    }

    // Función para reanudar la música de fondo (por ejemplo, "Pasillos")
    private void ReanudarMusicaDeFondo()
    {
        if (AudioManager.instancia != null)
        {
            Debug.Log("Reanudando música de fondo: Pasillos");
            AudioManager.instancia.CambiarMusica("Pasillos");
        }
        else
        {
            Debug.LogError("AudioManager.instancia es null al reanudar la música.");
        }
    }

    // Coroutine para el fade del canvas (transición de opacidad)
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
