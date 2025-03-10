using UnityEngine;
using Unity.Cinemachine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] habitaciones; // Lista de habitaciones
    public CinemachineCamera camaraActual; // Cámara activa

    public void ActivarHabitacion(GameObject habitacionActiva)
    {
        Debug.Log("Intentando activar: " + habitacionActiva.name);

        // Desactivar todas las habitaciones antes de activar la nueva
        foreach (GameObject habitacion in habitaciones)
        {
            if (habitacion != habitacionActiva)
            {
                Debug.Log("Desactivando: " + habitacion.name);
                habitacion.SetActive(false);
            }
        }

        // Activar la nueva habitación
        habitacionActiva.SetActive(true);
        Debug.Log("Habitación activada: " + habitacionActiva.name);

        // Buscar la nueva CinemachineCamera dentro de la habitación
        CinemachineCamera nuevaCamara = habitacionActiva.GetComponentInChildren<CinemachineCamera>();
        if (nuevaCamara != null)
        {
            Debug.Log("Cambiando a cámara: " + nuevaCamara.name);

            if (camaraActual != null)
            {
                Debug.Log("Bajando prioridad de cámara actual: " + camaraActual.name);
                camaraActual.Priority = 0; // Baja la prioridad de la cámara anterior
            }

            Debug.Log("Subiendo prioridad de cámara nueva: " + nuevaCamara.name);
            nuevaCamara.Priority = 10; // Asegura que la nueva cámara tiene prioridad alta
            camaraActual = nuevaCamara; // Actualiza la referencia de la cámara actual
        }
        else
        {
            //Debug.LogWarning("No se encontró una CinemachineCamera en " + habitacionActiva.name);
        }
    }
}
