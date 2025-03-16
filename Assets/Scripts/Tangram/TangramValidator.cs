using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Necesario para usar el botón

/// <summary>
/// Asocia cada "PosicionObjetivo" a una pieza concreta,
/// y cada "PosicionObjetivo" puede tener varias "referencias" (RectTransform).
/// </summary>
[System.Serializable]
public class PosicionObjetivo
{
    public TangramPiece piezaAsignada; // La pieza que debe encajar
    public List<RectTransform> referencias;   // Las posiciones/rotaciones válidas para esa pieza
}

/// <summary>
/// Valida si cada pieza está en alguna de sus posiciones correctas.
/// </summary>
public class TangramValidator : MonoBehaviour
{
    [Header("Lista de Objetivos")]
    [SerializeField] public List<PosicionObjetivo> posicionesObjetivo = new List<PosicionObjetivo>(); // Cambiado a público para acceso

    [Header("UI")]
    public TextMeshProUGUI mensajeTexto; // Texto para mostrar mensajes
    public Button botonComprobar; // El botón para comprobar las respuestas

    public int piezasCorrectas = 0; // Para contar las piezas correctamente posicionadas

    private void Start()
    {
        AsignarPiezas();
        botonComprobar.onClick.AddListener(CheckSolution);
    }

    /// Asigna las referencias de `posicionesObjetivo` a cada `TangramPiece`.
    private void AsignarPiezas()
    {
        foreach (var posicion in posicionesObjetivo)
        {
            if (posicion.piezaAsignada == null)
            {
                Debug.LogWarning("❌ Falta asignar una pieza en un PosicionObjetivo.");
                continue;
            }

            // Limpiar la lista previa
            posicion.piezaAsignada.targetPositions.Clear();

            // Añadir referencias
            foreach (RectTransform refT in posicion.referencias)
            {
                if (refT != null)
                {
                    posicion.piezaAsignada.targetPositions.Add(refT);
                }
            }

            Debug.Log($"✅ Asignadas {posicion.referencias.Count} referencias a {posicion.piezaAsignada.name}.");
        }
    }

    /// Comprueba si cada pieza está bien posicionada llamando a `EstaCorrecta()`.
    public void CheckSolution()
    {
        piezasCorrectas = 0; // Reseteamos el contador

        int total = posicionesObjetivo.Count;

        foreach (var posicion in posicionesObjetivo)
        {
            var pieza = posicion.piezaAsignada;
            if (pieza == null)
            {
                Debug.LogWarning("❌ Hay un PosicionObjetivo sin pieza asignada.");
                continue;
            }

            if (pieza.EstaCorrecta()) // Si la pieza está bien posicionada
            {
                piezasCorrectas++;
            }
            else
            {
                Debug.LogWarning($"❌ {pieza.name} no está bien posicionada.");
            }
        }
    }

    /// Muestra un mensaje en la UI (si está asignada).
    public void MostrarMensaje(string mensaje, Color color)
    {
        if (mensajeTexto != null)
        {
            mensajeTexto.text = mensaje;
            mensajeTexto.color = color;
            mensajeTexto.alpha = 1f;
            CancelInvoke(nameof(OcultarMensaje));
            Invoke(nameof(OcultarMensaje), 2f);
        }
    }

    private void OcultarMensaje()
    {
        if (mensajeTexto != null)
        {
            mensajeTexto.alpha = 0f;
        }
    }
}
