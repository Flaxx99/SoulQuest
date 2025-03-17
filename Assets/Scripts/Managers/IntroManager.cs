using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    // CanvasGroup del Canvas de introducción para controlar el fade out.
    public CanvasGroup introCanvasGroup;
    // Referencia al GameObject de la HUD.
    public GameObject hudCanvas;

    void Start()
    {
        // Oculta la HUD al inicio.
        hudCanvas.SetActive(false);
    }

    void Update()
    {
        // Al presionar cualquier tecla, se inicia la transición.
        if (Input.anyKeyDown)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        float fadeDuration = 2.0f;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            introCanvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }
        // Una vez finalizado el fade out, desactiva el Canvas de introducción y activa la HUD.
        introCanvasGroup.gameObject.SetActive(false);
        hudCanvas.SetActive(true);
    }
}
