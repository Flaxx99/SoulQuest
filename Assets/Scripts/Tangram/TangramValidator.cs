using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [SerializeField] private List<PosicionObjetivo> posicionesObjetivo = new List<PosicionObjetivo>();

    [Header("UI")]
    public TextMeshProUGUI mensajeTexto; // Texto para mostrar mensajes

    private void Start()
    {
        AsignarPiezas();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckSolution();
        }
    }

    /// <summary>
    /// Asigna las referencias de `posicionesObjetivo` a cada `TangramPiece`.
    /// </summary>
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

    /// <summary>
    /// Comprueba si cada pieza está bien posicionada llamando a `EstaCorrecta()`.
    /// </summary>
    public void CheckSolution()
    {
        int piezasCorrectas = 0;
        int total = posicionesObjetivo.Count;

        foreach (var posicion in posicionesObjetivo)
        {
            var pieza = posicion.piezaAsignada;
            if (pieza == null)
            {
                Debug.LogWarning("❌ Hay un PosicionObjetivo sin pieza asignada.");
                continue;
            }

            if (pieza.EstaCorrecta())
            {
                Debug.Log($"✅ {pieza.name} está correctamente posicionada.");
                piezasCorrectas++;

                // Bloquear la pieza después de validarla
                pieza.GetComponent<PieceController>().LockPiece();
            }
            else
            {
                Debug.LogWarning($"❌ {pieza.name} no está bien posicionada.");
            }
        }

        Debug.Log($"🔎 Resultado: {piezasCorrectas}/{total} piezas correctas.");
    }


    /// <summary>
    /// Muestra un mensaje en la UI (si está asignada).
    /// </summary>
    private void MostrarMensaje(string mensaje, Color color)
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
