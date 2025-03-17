using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TangramPiece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;

    [Header("Validación")]
    public float toleranciaPosicion = 20f; // Ajusta según el tamaño de las piezas
    public float toleranciaRotacion = 10f; // Ajusta la tolerancia de rotación
    public List<RectTransform> targetPositions = new List<RectTransform>(); // Posiciones válidas

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); // Obtiene el Canvas más cercano
    }

    /// <summary>
    /// Comienza el arrastre y guarda la posición inicial.
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = rectTransform.anchoredPosition - eventData.position;
    }

    /// <summary>
    /// Mueve la pieza mientras se arrastra.
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
        {
            rectTransform.anchoredPosition = eventData.position + offset;
        }
    }

    /// <summary>
    /// Se llama al soltar la pieza. Aquí podríamos validar la posición.
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // Opcional: Comprobar si la pieza está en su posición correcta al soltar
    }

    /// <summary>
    /// Rota la pieza en torno al eje Z.
    /// </summary>
    public void RotarPieza(float angulo)
    {
        rectTransform.Rotate(Vector3.forward, angulo);
    }

    /// <summary>
    /// Comprueba si la pieza está correctamente posicionada.
    /// </summary>
    public bool EstaCorrecta()
    {
        foreach (RectTransform target in targetPositions)
        {
            if (target == null) continue;

            // Calculamos la distancia en UI (espacio local)
            float distancia = Vector2.Distance(rectTransform.anchoredPosition, target.anchoredPosition);

            // Diferencia de rotación (permitimos ángulos de 45° y 180°)
            float rotPieza = rectTransform.eulerAngles.z;
            float rotRef = target.eulerAngles.z;
            float diferenciaRot = Mathf.Abs(Mathf.DeltaAngle(rotPieza, rotRef));

            // Ajustar tolerancias para aceptar posiciones espejadas
            float toleranciaPos = 15f;
            float toleranciaRot = 10f;

            // Si la rotación es múltiplo de 45° o 180° (para inversiones), lo validamos
            if (distancia <= toleranciaPos && (diferenciaRot % 45f <= toleranciaRot || diferenciaRot % 180f <= toleranciaRot))
            {
                return true;
            }
        }
        return false;
    }

    public void HacerAlgo()
    {
        Debug.Log($"{gameObject.name}: Método HacerAlgo() ejecutado.");
    }


}
