using UnityEngine;

public class TangramManager : MonoBehaviour
{
    private TangramPiece piezaSeleccionada = null; // Guardamos la pieza seleccionada

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detectar clic en una pieza
        {
            DetectarPiezaSeleccionada();
        }

        if (piezaSeleccionada != null) // Solo gira la pieza seleccionada
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                piezaSeleccionada.RotarPieza(-5f); // Girar 15° a la izquierda
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                piezaSeleccionada.RotarPieza(5f); // Girar 15° a la derecha
            }
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
                piezaSeleccionada = nuevaPieza; // Guardamos la referencia de la pieza seleccionada
                Debug.Log("Pieza seleccionada: " + piezaSeleccionada.name);
            }
        }
    }

    public TangramPiece GetPiezaSeleccionada()
    {
        return piezaSeleccionada;
    }
}
