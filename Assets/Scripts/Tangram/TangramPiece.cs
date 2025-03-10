using UnityEngine;

public class TangramPiece : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;

    [Header("Configuración de Validación")]
    public Transform targetPosition; // Se asigna en el Inspector desde Validator
    public float toleranciaPosicion = 0.1f;
    public float toleranciaRotacion = 5f;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + offset;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 10f;
        return cam.ScreenToWorldPoint(mousePoint);
    }

    public void RotarPieza(float angulo)
    {
        transform.Rotate(Vector3.forward, angulo, Space.Self);
    }

    // Método para verificar si la pieza está en su posición y rotación correctas
    public bool EstaCorrecta()
    {
        float distancia = Vector2.Distance(transform.position, targetPosition.position);
        float diferenciaRotacion = Mathf.Abs(transform.rotation.eulerAngles.z - targetPosition.rotation.eulerAngles.z);

        Debug.Log(gameObject.name + " -> Distancia: " + distancia + ", Rotación: " + diferenciaRotacion);

        return distancia < toleranciaPosicion && diferenciaRotacion < toleranciaRotacion;
    }
}
