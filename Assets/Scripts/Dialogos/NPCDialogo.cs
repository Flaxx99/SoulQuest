using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoNPCDialogo", menuName = "NPC/Dialogo")]
public class NPCDialogo : ScriptableObject
{
    [Header("Info")]
    public string Nombre;
    public Sprite Icono;
    public bool ContieneInteraccionExtra;

    [Header("Saludo")]
    [TextArea] public string Saludo;

    [Header("Chat")]
    [TextArea] public string[] Conversacion; // 🔥 Convertido a array para múltiples líneas de diálogo

    [Header("Despedida")]
    [TextArea] public string Despedida;
}
