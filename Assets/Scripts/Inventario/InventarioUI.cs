using System.Collections.Generic;
using System.Diagnostics.Tracing;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventarioUI : Singleton<InventarioUI> 
{
    [Header("Panel Inventario Descripcion")]
    [SerializeField] private GameObject panelInventarioDescripcion;
    [SerializeField] private Image itemIcono;
    [SerializeField] private TextMeshProUGUI itemNombre;
    [SerializeField] private TextMeshProUGUI itemDescripcion;

    [SerializeField] private InventarioSlot slotPrefab;
    [SerializeField] private Transform contenedor;

    public InventarioSlot SlotSeleccionado { get; private set; }

    private List<InventarioSlot> slotsDisponibles = new List<InventarioSlot>();
    
    void Start()
    {
        InicializarInventario();
    }

    private void Update()
    {
        ActualizarSlotSeleccionado();
    }

    private void InicializarInventario()
    {
        for (int i = 0; i < Inventario.Instance.NumeroDeSlots; i++)
        {
            InventarioSlot nuevoSlot = Instantiate(slotPrefab, contenedor);
            nuevoSlot.Index = i;
            slotsDisponibles.Add(nuevoSlot);
        }
    }

    private void ActualizarSlotSeleccionado()
    {
        GameObject goSeleccionado = EventSystem.current.currentSelectedGameObject;
        if (goSeleccionado == null)
        {
            return;
        }

        InventarioSlot slot = goSeleccionado.GetComponent<InventarioSlot>();
        if (slot != null)
        {
            SlotSeleccionado = slot;
        }
    }

    public void DibujarItemEnInventario(InventarioItem itemPorAgregar, int cantidad, int itemIndex)
    {
        InventarioSlot slot = slotsDisponibles[itemIndex];
        if (itemPorAgregar != null)
        {
            slot.ActivarSlotUI(true);
            slot.ActualizarSlot(itemPorAgregar, cantidad);
        }
        else
        {
            slot.ActivarSlotUI(false);
        }
    }

    private void ActualizarInventarioDescripcion(int index)
    {
        // Validar que el índice sea válido antes de continuar
        if (index < 0 || index >= Inventario.Instance.ItemsInventario.Length)
        {
            Debug.LogWarning($"Intento de actualizar inventario con índice inválido: {index}");
            panelInventarioDescripcion.SetActive(false);
            return;
        }

        InventarioItem item = Inventario.Instance.ItemsInventario[index];

        // Si el slot está vacío, ocultar la descripción
        if (item == null)
        {
            panelInventarioDescripcion.SetActive(false);
            return;
        }

        // Actualizar la UI con los datos del item
        itemIcono.sprite = item.Icono;
        itemNombre.text = item.Nombre;
        itemDescripcion.text = item.Descripcion;
        panelInventarioDescripcion.SetActive(true);

        // Buscar el botón "Usar" en la UI
        GameObject botonUsarGO = GameObject.Find("Usar - Button");
        if (botonUsarGO == null)
        {
            Debug.LogError("No se encontró el botón 'BotonUsar' en la escena.");
            return;
        }

        Button botonUsar = botonUsarGO.GetComponent<Button>();
        TextMeshProUGUI textoBotonUsar = botonUsarGO.GetComponentInChildren<TextMeshProUGUI>();

        if (textoBotonUsar == null)
        {
            Debug.LogError("No se encontró el componente TextMeshProUGUI en el botón 'BotonUsar'.");
            return;
        }

        // Cambiar el texto del botón según el tipo de objeto
        if (item.Tipo == TiposDeItem.Armas)
        {
            textoBotonUsar.text = "EQUIPAR";
        }
        else if (item.EsConsumible)
        {
            textoBotonUsar.text = "USAR";
        }
        else
        {
            textoBotonUsar.text = "ACCIÓN";
        }
    }



    public void UsarItem()
    {
        if (SlotSeleccionado != null)
        {
            SlotSeleccionado.SlotUsarItem();
            SlotSeleccionado.SeleccionarSlot();
        }
    }

    public void EquiparItem()
    {
        if (SlotSeleccionado != null)
        {
            SlotSeleccionado.SlotEquiparItem();
            SlotSeleccionado.SeleccionarSlot();
        }
    }

    public void RemoverItem()
    {
        if (SlotSeleccionado != null)
        {
            SlotSeleccionado.SlotRemoverItem();
            SlotSeleccionado.SeleccionarSlot();
        }
    }

    #region Evento
    private void SlotInteraccionRespuesta(TipoDeInteraccion tipo, int index)
    {
        if (tipo == TipoDeInteraccion.Click)
        {
            ActualizarInventarioDescripcion(index);
        }
    }

    private void OnEnable()
    {
        InventarioSlot.EventoSlotInteraccion += SlotInteraccionRespuesta;
    }

    private void OnDisable()
    {
        InventarioSlot.EventoSlotInteraccion -= SlotInteraccionRespuesta;
    }
    #endregion
}
