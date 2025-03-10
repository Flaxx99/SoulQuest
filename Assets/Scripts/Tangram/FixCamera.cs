using UnityEngine;

public class FixCamera : MonoBehaviour
{
    private Vector3 fixedPosition;

    void Start()
    {
        fixedPosition = transform.position; // Guarda la posición inicial de la cámara
    }

    void LateUpdate()
    {
        transform.position = fixedPosition; // Bloquea la posición de la cámara
    }
}
