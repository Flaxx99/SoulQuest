using System;
using UnityEngine;

[Serializable]
public class IATransicion
{
    public IADecision Desicion;
    public IAEstado EstadoVerdadero;
    public IAEstado EstadoFalso;
}
