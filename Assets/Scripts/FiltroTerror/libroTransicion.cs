using UnityEngine;

public class LibroInteraccion : MonoBehaviour
{
    public TransicionConCodigo transicion;  // Referencia al script de transición
    private bool transicionRealizada = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !transicionRealizada)
        {
            transicionRealizada = true;
            transicion.ActivarTransicion();  // Activa la transición cuando el jugador entra en el trigger
        }
    }
}
