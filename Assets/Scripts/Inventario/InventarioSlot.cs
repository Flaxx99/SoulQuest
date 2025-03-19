using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TipoDeInteraccion
{
    Click,
    Usar,
    Equipar,
    Remover
}
public class InventarioSlot : MonoBehaviour
{
    public static Action<TipoDeInteraccion, int> EventoSlotInteraccion;

    [SerializeField] private Image itemIcono;
    [SerializeField] private GameObject fondoCantidad;
    [SerializeField] private TextMeshProUGUI cantidadTMP;
    public int Index { get; set; }
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    public void ActualizarSlot(InventarioItem item, int cantidad)
    {
        itemIcono.sprite = item.Icono;
        cantidadTMP.text = cantidad.ToString();
    }

    public void ActivarSlotUI(bool estado)
    {
        itemIcono.gameObject.SetActive(estado);
        fondoCantidad.SetActive(estado);
    }

    public void SeleccionarSlot()
    {
        _button.Select();
    }

    public void ClickSlot()
    {
        EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Click, Index);
    }

    public void SlotUsarItem()
    {
        if (Inventario.Instance.ItemsInventario[Index] == null)
        {
            return;
        }

        InventarioItem item = Inventario.Instance.ItemsInventario[Index];

        // Si el item es un arma, en lugar de usarla, la equipa
        if (item.Tipo == TiposDeItem.Armas)
        {
            Debug.Log($"Equipando arma: {item.Nombre}");
            EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Equipar, Index);
            return;
        }

        // Si el item es consumible, se usa normalmente
        if (item.EsConsumible)
        {
            Debug.Log($"Usando consumible: {item.Nombre}");
            EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Usar, Index);
        }
    }


    public void SlotEquiparItem()
    {
        if (Inventario.Instance.ItemsInventario[Index] != null)
        {
            EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Equipar, Index);
        }
    }

    public void SlotRemoverItem()
    {
        if (Inventario.Instance.ItemsInventario[Index] != null)
        {
            EventoSlotInteraccion?.Invoke(TipoDeInteraccion.Remover, Index);
        }
    }
}
