using UnityEngine;

public class VidaBase : MonoBehaviour
{
    [SerializeField] protected float saludInicial;
    [SerializeField] protected float saludMax;
    public float Salud { get; protected set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Salud = saludInicial;
    }

    public void RecibirDano(float cantidad)
    {
        if (cantidad <= 0 || Salud <= 0) return; // No recibir daño si ya está en 0

        Salud -= cantidad;
        if (Salud < 0) Salud = 0; // Evita valores negativos

        ActualizarBarraVida(Salud, saludMax);

        if (Salud == 0)
        {
            PersonajeDerrotado(); // Se llama solo si la vida llega a 0
        }
    }

    protected virtual void ActualizarBarraVida(float vidaActual, float vidaMax)
    {

    }
    protected virtual void PersonajeDerrotado()
    {

    }

}
