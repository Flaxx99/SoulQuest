using UnityEngine;

[CreateAssetMenu(menuName = "Stats")]
public class PersonajeStats : ScriptableObject
{
    [Header("Stats")]
    public float Dano =5f;
    public float Defensa =2f;
    public float Velocidad= 5f;
    public float Nivel;
    public float ExpActual;
    public float ExpRequeridaSiguienteNivel;
    [Range(0f, 100f)] public float PorcentajeCritico;
    [Range(0f, 100f)] public float PorcentajeBloqueo;

    [Header("Atributos")]
    public int Fuerza;
    public int Inteligencia;
    public int Destreza;
    [HideInInspector] public int PuntosDisponibles;

    public void AnadirBonusPorAtributoFuerza()
    {
        Dano += 2f;
        Defensa += 1f;
        PorcentajeBloqueo += 0.03f;
    }

    public void AnadirBonusPorAtributoInteligencia()
    {
        Dano += 3f;
        PorcentajeCritico += 0.2f;
    }

    public void AnadirBonusPorAtributoDestreza()
    {
        Velocidad += 0.1f;
        PorcentajeBloqueo += 0.05f;
    }

    public void AgregarBonusPorArma(Arma arma)
    {
        Dano += arma.Damage;
        PorcentajeCritico += arma.ChanceCritico;
        PorcentajeBloqueo += arma.ChanceBloqueo;
    }

    public void RemoverBonusPorArma(Arma arma)
    {
        Dano -= arma.Damage;
        PorcentajeCritico -= arma.ChanceCritico;
        PorcentajeBloqueo -= arma.ChanceBloqueo;
    }

    public void ResetearValores()
    {
        Dano = 5f;
        Defensa = 2f;
        Velocidad = 5f;
        Nivel = 1;
        ExpActual = 0f;
        ExpRequeridaSiguienteNivel = 0f;
        PorcentajeBloqueo = 0f;
        PorcentajeCritico = 0f;
        Fuerza = 0;
        Inteligencia = 0;
        Destreza = 0;

        PuntosDisponibles = 0;
    }
}
