using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Asocia cada "PosicionObjetivo" a una pieza concreta,
/// y cada "PosicionObjetivo" puede tener varias "referencias" (Transform).
/// </summary>
[System.Serializable]
public class PosicionObjetivo
{
    public TangramPiece piezaAsignada;    // La pieza que debe encajar
    public List<Transform> referencias;   // Las posiciones/rotaciones válidas para esa pieza
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
        // Como ejemplo, asignamos las referencias a cada pieza
        // para que cada TangramPiece sepa dónde puede encajar.
        AsignarPiezas();
    }

    private void Update()
    {
        // Presiona Enter para validar
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckSolution();
        }
    }

    /// <summary>
    /// Pasa las referencias de 'posicionesObjetivo' a cada TangramPiece
    /// para que sepa cuáles son sus "targetPositions".
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

            // Limpiamos la lista por si tuviera algo previo
            posicion.piezaAsignada.targetPositions.Clear();

            // Añadimos todas las referencias definidas en el inspector
            foreach (Transform refT in posicion.referencias)
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
    /// Comprueba si cada pieza está bien posicionada llamando a 'EstaCorrecta()'.
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
            }
            else
            {
                Debug.LogWarning($"❌ {pieza.name} no está bien posicionada.");
            }
        }

        Debug.Log($"🔎 Resultado: {piezasCorrectas}/{total} piezas correctas.");

        if (piezasCorrectas == total)
        {
            MostrarMensaje("¡Tangram completado!", Color.green);
        }
        else
        {
            MostrarMensaje($"Piezas correctas: {piezasCorrectas}/{total}", Color.yellow);
        }
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
