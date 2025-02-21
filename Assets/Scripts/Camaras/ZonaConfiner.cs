using Unity.Cinemachine;
using UnityEngine;

public class ZonaConfiner : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camara;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            camara.Priority = 20; // Aumentar prioridad para que esta cámara se active
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            camara.Priority = 10; // Disminuir prioridad para volver a la cámara anterior
        }
    }
}
