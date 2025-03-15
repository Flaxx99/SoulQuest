using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Inventario : Singleton<Inventario>
{
    [Header("Items")]
    [SerializeField] private InventarioItem[] itemsInventario;
    [SerializeField] private Personaje personaje;
    [SerializeField] private int numeroDeSlots;

    public Personaje Personaje => personaje;
    public int NumeroDeSlots => numeroDeSlots;
    public InventarioItem[] ItemsInventario => itemsInventario;

    private void Start()
    {
        itemsInventario = new InventarioItem[numeroDeSlots];
    }

    public void AgregarItem(InventarioItem itemPorAgregar, int cantidad)
    {
        if (itemPorAgregar == null)
        {
            return;
        }

        //Verificacion en caso de tener un item similar en inventario
        List<int> indexes = VerificarExistencias(itemPorAgregar.ID);
        if (itemPorAgregar.EsAcumulable)
        {
            if (indexes.Count > 0)
            {
                for (int i = 0; i < indexes.Count; i++)
                {
                    if (itemsInventario[indexes[i]].Cantidad < itemPorAgregar.AcumulacionMax)
                    {
                        itemsInventario[indexes[i]].Cantidad += cantidad;
                        if (itemsInventario[indexes[i]].Cantidad > itemPorAgregar.AcumulacionMax)
                        {
                            int diferencia = itemsInventario[indexes[i]].Cantidad - itemPorAgregar.AcumulacionMax;
                            itemsInventario[indexes[i]].Cantidad = itemPorAgregar.AcumulacionMax;
                            AgregarItem(itemPorAgregar, diferencia);
                        }

                        InventarioUI.Instance.DibujarItemEnInventario(itemPorAgregar,
                            itemsInventario[indexes[i]].Cantidad, indexes[i]);
                        return;
                    }
                }
            }
        }

        if (cantidad <= 0)
        {
            return;
        }

        if (cantidad > itemPorAgregar.AcumulacionMax)
        {
            AgregarItemEnSlotDisponible(itemPorAgregar, itemPorAgregar.AcumulacionMax);
            cantidad -= itemPorAgregar.AcumulacionMax;
            AgregarItem(itemPorAgregar, cantidad);
        }
        else
        {
            AgregarItemEnSlotDisponible(itemPorAgregar, cantidad);
        }
    }

    private List<int> VerificarExistencias(string itemID)
    {
        List<int> indexesDeItem = new List<int>();
        for (int i = 0; i < itemsInventario.Length; i++)
        {
            if (itemsInventario[i] != null)
            {
                if (itemsInventario[i].ID == itemID)
                {
                    indexesDeItem.Add(i);
                }
            }
            
        }

        return indexesDeItem;
    }

    private void AgregarItemEnSlotDisponible(InventarioItem item, int cantidad)
    {
        for (int i = 0; i < itemsInventario.Length; i++)
        {
            if (itemsInventario[i] == null)
            {
                itemsInventario[i] = item.CopiarItem();
                itemsInventario[i].Cantidad = cantidad;
                InventarioUI.Instance.DibujarItemEnInventario(item, cantidad, i);
                return;
            }
        }
    }

    private void EliminarItem(int index)
    {
        itemsInventario[index].Cantidad--;
        if (itemsInventario[index].Cantidad <= 0)
        {
            itemsInventario[index].Cantidad = 0;
            itemsInventario[index] = null;
            InventarioUI.Instance.DibujarItemEnInventario(null, 0, index);
        }
        else
        {
            InventarioUI.Instance.DibujarItemEnInventario(itemsInventario[index],
                itemsInventario[index].Cantidad, index);
        }
    }

    private void UsarItem(int index)
    {
        if (itemsInventario[index] == null)
        {
            return;
        }

        if (itemsInventario[index].UsarItem())
        {
            EliminarItem(index);
        }
    }

    private void EquiparItem(int index)
    {
        if (itemsInventario[index] == null)
        {
            return;
        }

        if (itemsInventario[index].Tipo != TiposDeItem.Armas)
        {
            return;
        }

        itemsInventario[index].EquiparItem();
    }

    private void RemoverItem(int index)
    {
        if (itemsInventario[index] == null)
        {
            return;
        }

        if (itemsInventario[index].Tipo != TiposDeItem.Armas)
        {
            return;
        }

        itemsInventario[index].RemoverItem();
    }

    #region Eventos
    private void SlotInteraccionRespuesta(TipoDeInteraccion tipo, int index)
    {
        switch (tipo)
        {
            case TipoDeInteraccion.Usar:
                UsarItem(index);
                break;
            case TipoDeInteraccion.Equipar:
                EquiparItem(index);
                break;
            case TipoDeInteraccion.Remover:
                RemoverItem(index);
                break;
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
