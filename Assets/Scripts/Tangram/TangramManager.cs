using UnityEngine;

public class TangramManager : MonoBehaviour
{
    private TangramPiece piezaSeleccionada = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectarPiezaSeleccionada();
        }

        if (piezaSeleccionada != null && Input.GetKeyDown(KeyCode.R))
        {
            piezaSeleccionada.RotarPieza(-45f); // Rota en sentido horario
            Debug.Log($"🔄 Rotando {piezaSeleccionada.name} en sentido horario");
        }
    }

    private void DetectarPiezaSeleccionada()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePosition);

        if (hit != null)
        {
            TangramPiece nuevaPieza = hit.GetComponent<TangramPiece>();

            if (nuevaPieza != null)
            {
                piezaSeleccionada = nuevaPieza;
                Debug.Log("Pieza seleccionada: " + piezaSeleccionada.name);
            }
        }
    }
}
