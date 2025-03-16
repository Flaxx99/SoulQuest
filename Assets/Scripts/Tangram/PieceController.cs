using UnityEngine;
using UnityEngine.EventSystems;

public class PieceController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isDragging = false;
    private bool isSelected = false;

    private static PieceController piezaSeleccionada = null; // Solo una pieza seleccionada a la vez

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        // Permitir girar si la pieza está seleccionada y el clic está presionado
        if (piezaSeleccionada == this && Input.GetMouseButton(0))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotatePiece(45f); // Gira cada vez que se presiona 'R'
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Si hay otra pieza seleccionada, la deseleccionamos
        if (piezaSeleccionada != null && piezaSeleccionada != this)
        {
            piezaSeleccionada.Deseleccionar();
        }

        // Seleccionamos esta pieza
        piezaSeleccionada = this;
        isSelected = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && canvas != null)
        {
            Vector2 newPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out newPosition);

            rectTransform.anchoredPosition = newPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    /// <summary>
    /// Gira la pieza en ángulos de 45°
    /// </summary>
    private void RotatePiece(float angle)
    {
        float newRotation = Mathf.Round(rectTransform.eulerAngles.z / 45f) * 45f + angle;
        rectTransform.rotation = Quaternion.Euler(0, 0, newRotation);
    }

    /// <summary>
    /// Bloquea la pieza cuando se valida correctamente
    /// </summary>
    public void LockPiece()
    {
        isDragging = false;
        isSelected = false;
        piezaSeleccionada = null; // No hay ninguna pieza seleccionada después de bloquearse
        enabled = false; // Desactiva el script para evitar más movimientos
    }

    /// <summary>
    /// Deselecciona la pieza para evitar que siga girando al cambiar de pieza
    /// </summary>
    private void Deseleccionar()
    {
        isSelected = false;
    }
}
