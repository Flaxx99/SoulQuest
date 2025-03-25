using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinalMessageController : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup panelCanvasGroup;           // Fondo negro con CanvasGroup
    public GameObject panelTexto;                  // Panel del texto
    public TextMeshProUGUI messageText;            // Componente de texto (escrito en Inspector)
    public GameObject terrorEffect;                // Efecto visual terrorífico
    public string nextSceneName = "Creditos";      // Nombre de la escena de créditos

    [Header("Tiempos")]
    public float fadeDuration = 2f;
    public float velocidadEscritura = 0.05f;       // Tiempo entre letras
    public float esperaDespuesDeTexto = 3f;        // Tiempo que espera luego de escribir todo
    public float esperaPostTerror = 2f;            // Tiempo tras apagar terror antes de créditos

    [Header("UI que se debe ocultar")]
    public GameObject[] panelesOcultables;

    [Header("Audio")]
    public AudioClip musicaFinal;
    private AudioSource audioSource;

    public GameObject slideshowRestauracion;  // Referencia al objeto del slideshow
    

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void ShowFinalMessage()
    {
        Debug.Log("✅ ShowFinalMessage() INVOCADO.");
        StartCoroutine(SecuenciaFinal());
    }

    public IEnumerator SecuenciaFinal()
    {
        // Ocultar los paneles
        foreach (GameObject panel in panelesOcultables)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        panelTexto.SetActive(false);
        panelCanvasGroup.gameObject.SetActive(true);

        // Fade IN
        Debug.Log("🎬 Iniciando fade IN...");
        yield return StartCoroutine(FadeCanvas(0f, 1f, fadeDuration));

        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.GetAudioSource().Pause();
        }

        // Música final
        if (musicaFinal != null && audioSource != null)
        {
            Debug.Log("🎵 Reproduciendo música final...");
            audioSource.clip = musicaFinal;
            audioSource.loop = true;
            StartCoroutine(FadeInAudio(audioSource, 0.8f, 2f)); // ← duración del fade y volumen final
        }

        // Mostrar el mensaje letra por letra
        panelTexto.SetActive(true);
        string textoCompleto = messageText.text;
        yield return StartCoroutine(MostrarTextoMaquina(messageText, textoCompleto, velocidadEscritura));

        // Esperar después de mostrar todo el texto
        yield return new WaitForSeconds(esperaDespuesDeTexto);

        // Fade OUT
        Debug.Log("🎬 Iniciando fade OUT...");
        yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));

        panelTexto.SetActive(false);

        yield return new WaitForSeconds(esperaPostTerror);

        // Activar el contenedor y sus hijos
        GameObject canvaPantallaCambio = GameObject.Find("CanvaPantallaCambio");

        if (canvaPantallaCambio != null)
        {
            // Activar el contenedor y todos sus hijos
            canvaPantallaCambio.SetActive(true);
            Debug.Log("CanvaPantallaCambio activado.");

            // Activar PanelFondo
            GameObject panelFondo = canvaPantallaCambio.transform.Find("PanelFondo").gameObject;
            if (panelFondo != null)
            {
                panelFondo.SetActive(true);
                Debug.Log("PanelFondo activado.");
            }

            // Activar ImagenPantalla
            GameObject imagenPantalla = canvaPantallaCambio.transform.Find("ImagenPantalla").gameObject;
            if (imagenPantalla != null)
            {
                imagenPantalla.SetActive(true);
                Debug.Log("ImagenPantalla activada.");
            }

            // Activar SlideShowRestauracion
            GameObject slideshow = canvaPantallaCambio.transform.Find("SlideShowRestauracion").gameObject;
            if (slideshow != null)
            {
                slideshow.SetActive(true);  // Activar el slideshow
                Debug.Log("SlideShowRestauracion activado.");

                // Iniciar la secuencia del slideshow
                SlideshowRestauracion slideshowScript = slideshow.GetComponent<SlideshowRestauracion>();
                if (slideshowScript != null)
                {
                    StartCoroutine(slideshowScript.SecuenciaSlideshow());  // Iniciar la secuencia
                }
            }
        }
        else
        {
            Debug.LogError("No se encontró el objeto Canvas con nombre 'CanvaPantallaCambio'");
        }

        // Apagar efecto de terror
        if (terrorEffect != null)
        {
            Debug.Log("🧯 Apagando efecto de terror...");
            terrorEffect.SetActive(false);
        }

        // Esperar el tiempo necesario antes de cargar los créditos
        yield return new WaitForSeconds(10f);

        // Fade-out final antes de cargar los créditos
        yield return StartCoroutine(FadeCanvas(0f, 1f,1f));  // Realizamos un fade-out adicional para suavizar la transición

        // Esperar un poco más antes de cargar los créditos
        yield return new WaitForSeconds(1f);

        // Cargar la escena de créditos
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator MostrarTextoMaquina(TextMeshProUGUI texto, string contenido, float velocidad)
    {
        texto.text = "";
        foreach (char letra in contenido)
        {
            texto.text += letra;
            yield return new WaitForSeconds(velocidad);
        }
    }

    private IEnumerator FadeCanvas(float start, float end, float duration)
    {
        float t = 0f;
        panelCanvasGroup.alpha = start;
        panelCanvasGroup.gameObject.SetActive(true);

        while (t < duration)
        {
            t += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }

        panelCanvasGroup.alpha = end;

        if (end == 0f)
            panelCanvasGroup.gameObject.SetActive(false);
    }
    private IEnumerator FadeInAudio(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = 0f;
        source.volume = 0f;
        source.Play();

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }

}
