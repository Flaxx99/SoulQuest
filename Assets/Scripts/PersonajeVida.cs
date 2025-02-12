using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PersonajeVida : VidaBase 
{
    public bool PuedeSerCurado => Salud < saludMax;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) 
        {
            RecibirDaño(10);
        } 
        if(Input.GetKeyDown(KeyCode.R)) 
        {
            RestaurarSalud(10);
        }
    }
    public void RestaurarSalud(float cantidad)
    {
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

    protected override void ActualizarBarraVida(float vidaActual, float vidaMax)
    {
        base.ActualizarBarraVida(vidaActual, vidaMax);
    }
}
