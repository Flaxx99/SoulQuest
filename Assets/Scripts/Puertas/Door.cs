using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject habitacionDestino; // La habitación a la que lleva esta puerta
    private RoomManager roomManager;
    public Transform puntoEntrada; // Punto donde aparecerá el jugador en la nueva habitación

    private void Start()
    {
        roomManager = FindFirstObjectByType<RoomManager>(); // Encuentra el RoomManager
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Si el jugador entra en la puerta
        {
            Debug.Log("El jugador entró en la puerta. Cambiando de habitación...");
            roomManager.ActivarHabitacion(habitacionDestino);

            // Mover al jugador a la posición de entrada en la nueva habitación
            if (puntoEntrada != null)
            {
                Debug.Log("Moviendo al jugador a: " + puntoEntrada.position);
                other.transform.position = puntoEntrada.position; // Se mueve dentro del salón
            }
            else
            {
                Debug.LogWarning("No se asignó un punto de entrada para esta puerta.");
            }
        }
    }
}
