using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla el comportamiento de cada pieza del Tangram
/// (movimiento con el mouse y validación de su posición).
/// </summary>
public class TangramPiece : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;

    [Header("Validación")]
    public float toleranciaPosicion = 20f; // Ajusta según tu escala
    public float toleranciaRotacion = 10f; // Ajusta según lo que quieras permitir
    public List<Transform> targetPositions = new List<Transform>(); // Referencias donde encaja

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Asegurar que la pieza siempre esté en Z = 0 (2D)
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
    }

    /// <summary>
    /// Se llama cuando haces click en la pieza con el mouse
    /// </summary>
    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
    }

    /// <summary>
    /// Se llama mientras mantienes el click y mueves el mouse
    /// </summary>
    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + offset;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        // Ajusta el 'z' en función de la distancia de la cámara en tu escena
        mousePoint.z = 10f;
        return cam.ScreenToWorldPoint(mousePoint);
    }

    /// <summary>
    /// Rota la pieza en torno al eje Z
    /// </summary>
    public void RotarPieza(float angulo)
    {
        transform.Rotate(Vector3.forward, angulo, Space.Self);
    }

    /// <summary>
    /// Verifica si la pieza se encuentra en alguna de sus "targetPositions"
    /// dentro de las tolerancias de posición y rotación definidas.
    /// </summary>
    public bool EstaCorrecta()
    {
        // Recorremos todas las referencias posibles de la pieza
        foreach (Transform refPos in targetPositions)
        {
            if (refPos == null) continue;

            // Distancia 2D (ignoramos Z)
            float distancia = Vector2.Distance(transform.position, refPos.position);

            // Diferencia de ángulos usando DeltaAngle para que 359 vs 0 sea 1 grado
            float rotPieza = transform.eulerAngles.z;
            float rotRef = refPos.eulerAngles.z;
            float diferenciaRot = Mathf.Abs(Mathf.DeltaAngle(rotPieza, rotRef));

            // ¿Dentro de la tolerancia?
            if (distancia <= toleranciaPosicion && diferenciaRot <= toleranciaRotacion)
            {
                return true;
            }
        }

        // Si ninguna referencia coincide, no está correcta
        return false;
    }
}
