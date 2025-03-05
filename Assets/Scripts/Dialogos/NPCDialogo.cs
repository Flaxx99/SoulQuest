using UnityEngine;


public enum InteraccionExtraNPC
{
    Quests,
    Tienda,
    Crafting
}

[CreateAssetMenu]
public class NPCDialogo : ScriptableObject
{
    [Header("Info")]
    public string Nombre;
    public Sprite Icono;
    public bool ContieneInteraccionExtra;

    [Header("Saludo")]
    [TextArea] public string Saludo;

    [Header("Chat")]
    [TextArea] public string Conversacion;

    [Header("Despedida")]
    [TextArea] public string Despedida;
}

public class DialogoTexto
{
    [TextArea] public string Oracion;
}
