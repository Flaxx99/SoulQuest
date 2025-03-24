using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    // Referencia al ScrollRect del Scroll View
    public ScrollRect scrollRect;
    // Velocidad de desplazamiento (valor normalizado por segundo)
    public float scrollSpeed = 0.1f;

    void Update()
    {
        // La propiedad verticalNormalizedPosition va de 1 (parte superior) a 0 (parte inferior)
        // Se reduce para mover el contenido hacia arriba
        if (scrollRect.verticalNormalizedPosition > 0)
        {
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
        }
        else
        {
            // Opcional: Cuando llega al final, se puede detener o cargar otra escena
            this.enabled = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}