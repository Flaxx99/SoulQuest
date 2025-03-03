using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject habitacionDestino; // La habitación a la que lleva esta puerta
    private RoomManager roomManager;
    public Transform puntoEntrada; // Punto donde aparecerá el jugador en la nueva habitación

    private void Start()
    {
        roomManager = FindFirstObjectByType<RoomManager>();// Asegurar que roomManager no sea nulo
        if (roomManager == null)
        {
            Debug.LogError("RoomManager no encontrado en la escena. Asegúrate de que exista un RoomManager en la jerarquía.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (habitacionDestino == null)
            {
                Debug.LogError("HabitacionDestino no está asignado en " + gameObject.name);
                return;
            }

            if (puntoEntrada == null)
            {
                Debug.LogError("PuntoEntrada no está asignado en " + gameObject.name);
                return;
            }

            Debug.Log("El jugador entró en la puerta. Cambiando de habitación...");
            roomManager.ActivarHabitacion(habitacionDestino);

            // Mover al jugador a la posición de entrada en la nueva habitación
            other.transform.position = puntoEntrada.position;
        }
    }
}
