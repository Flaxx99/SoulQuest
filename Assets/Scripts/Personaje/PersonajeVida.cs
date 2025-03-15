using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PersonajeVida : VidaBase 
{
    
    public static Action EventoPersonajeDerrotado;
    public bool Derrotado { get; private set; }
    public bool PuedeSerCurado => Salud < saludMax;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
        ActualizarBarraVida(Salud, saludMax);
    }

    private void Update()
    {
       /* if(Input.GetKeyDown(KeyCode.T)) 
        {
            RecibirDano(10);
        } 
        if(Input.GetKeyDown(KeyCode.Y)) 
        {
            RestaurarSalud(10);
        }*/
    }
    public void RestaurarSalud(float cantidad)
    {
        if (Derrotado)
        {
            return;
        }
        if (PuedeSerCurado)
        {
            Salud += cantidad;
            if (Salud > saludMax)
            {
                Salud = saludMax;
            }
            ActualizarBarraVida(Salud, saludMax);
        }
    }
   protected override void PersonajeDerrotado()
    {
        if (Derrotado) return; // Evita que se ejecute más de una vez

        _boxCollider2D.enabled = false; // Desactiva colisiones
        Derrotado = true;

        // Asegurar que la UI refleja la muerte antes de ocultarla
        UIManager.Instance.ActualizarVidaPersonaje(0, saludMax);

        EventoPersonajeDerrotado?.Invoke();

        // Detener el movimiento del personaje
        PersonajeMovimiento movimiento = GetComponent<PersonajeMovimiento>();
        if (movimiento != null)
        {
            movimiento.enabled = false;
        }

        // Mostrar la pantalla de Game Over
        UIManager.Instance.MostrarPantallaGameOver();
    }


    public void RestaurarPersonaje()
    {
        _boxCollider2D.enabled = true;
        Derrotado = false;
        Salud = saludMax; // Restaura la vida al máximo
        ActualizarBarraVida(Salud, saludMax);

        // Reactivar movimiento
        PersonajeMovimiento movimiento = GetComponent<PersonajeMovimiento>();
        if (movimiento != null)
        {
            movimiento.enabled = true;
        }
        PersonajeMana personajeMana = GetComponent<PersonajeMana>();
        if (personajeMana != null)
        {
            personajeMana.RestablecerMana(); //Restaurar mana al reiniciar el personaje
        }


        // Restablecer animación a estado normal
        Animator anim = GetComponent<Animator>();
        if (anim != null && TieneParametro(anim, "Derrotado"))
        {
            anim.ResetTrigger("Derrotado"); // Asegura que el personaje no siga en estado de muerte
        }

        if (anim != null && TieneParametro(anim, "Revivir"))
        {
            anim.SetTrigger("Revivir");
        }
    }


    protected override void ActualizarBarraVida(float vidaActual, float vidaMax)
    {
        UIManager.Instance.ActualizarVidaPersonaje(vidaActual, vidaMax);
    }
    // Método para verificar si un parámetro existe en el Animator
    private bool TieneParametro(Animator animator, string parametro)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parametro)
            {
                return true;
            }
        }
        return false;
    }

}
