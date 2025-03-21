using UnityEngine;

public class LibroInteraccion : MonoBehaviour
{
    // 1. Coloca la referencia al script TransicionConCodigo aquí:
    public TransicionConCodigo transicion;  // Referencia al script de transición

    // 2. Aquí va el método OnTriggerEnter2D para activar la transición cuando el jugador entra en el área de colisión.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 3. Asegúrate de que transicion no esté nula antes de llamar a su método
            if (transicion != null)
            {
                transicion.ActivarTransicion();  // Activa la transición cuando el jugador entra en el trigger
            }
            else
            {
                Debug.LogError("Transicion no asignada en el Inspector");
            }
        }
    }
}
