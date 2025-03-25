using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;

public class SlideshowRestauracion : MonoBehaviour
{
    public Image imagenPantalla;
    public Sprite imagenAntes;
    public Sprite imagenDespues;
    public float duracionFade = 2f;
    public float esperaEntreImagenes = 2f;
    public GameObject panelFinalMensaje; // Activamos esto al final
    

    void Start()
    {
        // Aseguramos que la imagen comience con transparencia (alpha 0)
        imagenPantalla.color = new Color(1, 1, 1, 0);
        imagenPantalla.gameObject.SetActive(true); // Activamos la imagen
        Debug.Log("Iniciando secuencia de slideshow.");
        StartCoroutine(SecuenciaSlideshow());
    }

    public IEnumerator SecuenciaSlideshow()
    {
       
        // Mostrar imagen en blanco y negro (Imagen antes)
        imagenPantalla.sprite = imagenAntes;
        yield return StartCoroutine(FadeImage(0f, 1f)); // Hacer fade-in de la imagen

        yield return new WaitForSeconds(esperaEntreImagenes);

        // Mostrar imagen restaurada (Imagen después)
        imagenPantalla.sprite = imagenDespues;
        yield return StartCoroutine(FadeImage(0f, 1f)); // Hacer fade-in de la segunda imagen

        yield return new WaitForSeconds(esperaEntreImagenes + 1f);

        imagenPantalla.gameObject.SetActive(false); // Desactivamos la imagen

        // Activar mensaje final
        if (panelFinalMensaje != null)
        {
            panelFinalMensaje.SetActive(true);
            Debug.Log("Mensaje final activado");
        }

        // Espera antes de cargar los créditos
        yield return new WaitForSeconds(10f); // Ajusta el tiempo de espera si es necesario

        // Cargar la escena de créditos
        Debug.Log("Cargando escena de créditos...");
        SceneManager.LoadScene("Creditos");
    }

    // Función para realizar el fade-in / fade-out en la imagen
    IEnumerator FadeImage(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color color = imagenPantalla.color;
        while (t < duracionFade)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t / duracionFade);
            imagenPantalla.color = color;
            yield return null;
        }
        color.a = endAlpha;
        imagenPantalla.color = color;
    }
}
