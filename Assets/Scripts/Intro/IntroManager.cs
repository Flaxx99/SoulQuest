using UnityEngine;
using UnityEngine.UI;               // Necesario para CanvasGroup y UI
using UnityEngine.SceneManagement;  // Opcional, para cargar escenas (no usado en este ejemplo)
using System.Collections;

public class IntroManager : MonoBehaviour
{
    // Asigna en el Inspector el CanvasGroup del Canvas de la introducción.
    public CanvasGroup introCanvasGroup;

    // Asigna en el Inspector el GameObject que contiene la HUD o los elementos principales del juego.
    public GameObject hudCanvas;

    // Duración del efecto de desvanecimiento (fade out).
    public float fadeDuration = 2.0f;

    // Este método se invoca al presionar el botón "Aceptar"
    // Debe ser público, sin parámetros y retornar void para que aparezca en OnClick().
    public void OnAcceptButtonPressed()
    {
        Debug.Log("Botón Aceptar presionado");
        StartCoroutine(FadeOutAndActivateHUD());
    }

    // Coroutine que realiza el efecto de fade out y, al finalizar, activa la HUD.
    private IEnumerator FadeOutAndActivateHUD()
    {
        float startAlpha = introCanvasGroup.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            // Interpolación lineal entre la opacidad inicial y 0.
            float newAlpha = Mathf.Lerp(startAlpha, 0f, timeElapsed / fadeDuration);
            introCanvasGroup.alpha = newAlpha;
            yield return null;
        }

        // Aseguramos que la opacidad sea 0 al finalizar
        introCanvasGroup.alpha = 0f;
        // Desactivamos el Canvas de la introducción
        introCanvasGroup.gameObject.SetActive(false);
        // Activamos la HUD o los elementos principales del juego
        hudCanvas.SetActive(true);
    }
}
