using UnityEngine;
using Unity.Cinemachine;

public class ZoneTransition : MonoBehaviour
{
    public GameObject habitacionDestino; // La habitación a activar
    public CinemachineCamera camaraDestino; // La cámara de la nueva zona

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Detecta si el jugador entra en la zona
        {
            CambiarHabitacion();
        }
    }

    private void CambiarHabitacion()
    {
        // Desactivar todas las habitaciones
        foreach (GameObject habitacion in GameObject.FindGameObjectsWithTag("Habitacion"))
        {
            habitacion.SetActive(false);
        }

        // Activar la habitación destino
        habitacionDestino.SetActive(true);

        // Desactivar todas las cámaras
        foreach (CinemachineCamera cam in FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
        {
            cam.Priority = 0;
        }

        // Activar la cámara de la nueva zona
        camaraDestino.Priority = 10;
    }
}
