using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    public CanvasGroup introCanvasGroup;
    public GameObject hudCanvas;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.vieneDelMainMenu)
        {
            Debug.Log("🟢 Mostrando Intro porque el jugador viene del Main Menu");

            introCanvasGroup.gameObject.SetActive(true);
            hudCanvas.SetActive(false);

            // 🚀 Marcar que la intro ya se mostró, para evitar que vuelva a aparecer en reinicios
            GameManager.Instance.vieneDelMainMenu = false;
        }
        else
        {
            Debug.Log("⚠ No se muestra la intro porque es un reinicio");
            introCanvasGroup.gameObject.SetActive(false);
            hudCanvas.SetActive(true);
        }
    }

   public void CerrarIntro()
    {
        Debug.Log("🟢 Botón Aceptar presionado, cerrando intro...");
        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        float fadeDuration = 2.0f;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            introCanvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        introCanvasGroup.gameObject.SetActive(false);
        hudCanvas.SetActive(true);
    }
}
