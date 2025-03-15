using System.Collections;
using UnityEngine;

public class VidaBase : MonoBehaviour
{
    [SerializeField] protected float saludInicial;
    [SerializeField] protected float saludMax;
    public float Salud { get; protected set; }

    private SpriteRenderer spriteRenderer; // Para cambiar el color del personaje

    // Start is called once before la primera ejecución de Update
    protected virtual void Start()
    {
        Salud = saludInicial;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtiene el sprite del personaje
    }

    public void RecibirDano(float cantidad)
    {
        if (cantidad <= 0 || Salud <= 0) return; // No recibir daño si ya está en 0

        Salud -= cantidad;
        if (Salud < 0) Salud = 0; // Evita valores negativos

        ActualizarBarraVida(Salud, saludMax);

        //Activa el efecto de parpadeo rojo
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

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

    //Efecto de Parpadeo Rojo
    IEnumerator FlashRed()
    {
        for (int i = 0; i < 3; i++) // Parpadea 3 veces
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
