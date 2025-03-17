using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Necesario para usar el botón

[System.Serializable]
public class PosicionObjetivo
{
    public TangramPiece piezaAsignada; // La pieza que debe encajar
    public List<RectTransform> referencias;   // Las posiciones/rotaciones válidas para esa pieza
}

public class TangramValidator : MonoBehaviour
{
    [Header("Lista de Objetivos")]
    [SerializeField] public List<PosicionObjetivo> posicionesObjetivo = new List<PosicionObjetivo>();

    [Header("UI")]
    public TextMeshProUGUI mensajeTexto;
    public Button botonComprobar;

    public int piezasCorrectas = 0;

    // Diccionarios para almacenar las posiciones y rotaciones originales
    private Dictionary<TangramPiece, Vector3> posicionesIniciales = new Dictionary<TangramPiece, Vector3>();
    private Dictionary<TangramPiece, Quaternion> rotacionesIniciales = new Dictionary<TangramPiece, Quaternion>();

    private void Start()
    {
        AsignarPiezas();
        GuardarPosicionesIniciales(); // Guardamos las posiciones iniciales de las piezas
        botonComprobar.onClick.AddListener(CheckSolution);
    }

    private void GuardarPosicionesIniciales()
    {
        foreach (var posicion in posicionesObjetivo)
        {
            if (posicion.piezaAsignada != null)
            {
                posicionesIniciales[posicion.piezaAsignada] = posicion.piezaAsignada.transform.position;
                rotacionesIniciales[posicion.piezaAsignada] = posicion.piezaAsignada.transform.rotation;
            }
        }
    }

    public void ReiniciarTangram()
    {
        foreach (var posicion in posicionesObjetivo)
        {
            if (posicion.piezaAsignada != null)
            {
                posicion.piezaAsignada.transform.position = posicionesIniciales[posicion.piezaAsignada];
                posicion.piezaAsignada.transform.rotation = rotacionesIniciales[posicion.piezaAsignada];
            }
        }
    }

    private void AsignarPiezas()
    {
        foreach (var posicion in posicionesObjetivo)
        {
            if (posicion.piezaAsignada == null)
            {
                Debug.LogWarning("❌ Falta asignar una pieza en un PosicionObjetivo.");
                continue;
            }

            posicion.piezaAsignada.targetPositions.Clear();

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

    public void CheckSolution()
    {
        piezasCorrectas = 0;
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
                piezasCorrectas++;
            }
            else
            {
                Debug.LogWarning($"❌ {pieza.name} no está bien posicionada.");
            }
        }
    }

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
