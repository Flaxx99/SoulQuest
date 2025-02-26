using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Personaje/Arma")]
public class Arma : ScriptableObject
{
    [Header("Config")]
    public Sprite ArmaIcono; 
    public Sprite IconoSkill;
    public float Daño;

    [Header("Stats")]
    public float ChanceCritico;
    public float ChanceBloqueo;

}
