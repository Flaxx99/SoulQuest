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

    public void ShowFinalMessage()
    {
        Debug.Log("✅ ShowFinalMessage() INVOCADO.");
        StartCoroutine(SecuenciaFinal());
    }

    private IEnumerator SecuenciaFinal()
    {
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

        // Mostrar mensaje letra por letra
        panelTexto.SetActive(true);
        string textoCompleto = messageText.text; // Tomamos lo que ya tiene asignado
        yield return StartCoroutine(MostrarTextoMaquina(messageText, textoCompleto, velocidadEscritura));

        // Espera final con texto completo visible
        yield return new WaitForSeconds(esperaDespuesDeTexto);

        // Fade OUT
        Debug.Log("🎬 Iniciando fade OUT...");
        yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));

        panelTexto.SetActive(false);

        // Apagar efecto de terror
        if (terrorEffect != null)
        {
            Debug.Log("🧯 Apagando efecto de terror...");
            terrorEffect.SetActive(false);
        }

        yield return new WaitForSeconds(esperaPostTerror);

        // Cargar escena de créditos
        Debug.Log("🎞 Cargando escena de créditos...");
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
}
