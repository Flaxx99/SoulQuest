using UnityEngine;

public class TriggerInteraction : MonoBehaviour
{
    public ShakeEffect shakeEffect; // Referencia al script ShakeEffect

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el jugador ha entrado en el trigger
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga el tag "Player"
        {
            // Activar el efecto de shake
            shakeEffect.Shake();
        }
    }
}
