using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Jugador jugador;
    public Text textoEscudo;
    public Text textoVelocidad;

    void Update()
    {
        // Mostrar tiempo de escudo si está activado
        if (jugador.tiempoEscudoRestante > 0)
        {
            textoEscudo.text = "Escudo: " + Mathf.Ceil(jugador.tiempoEscudoRestante) + "s";
        }
        else
        {
            textoEscudo.text = "Escudo: Inactivo";
        }

        // Mostrar tiempo de velocidad aumentada si está activado
        if (jugador.tiempoPoderVelocidadRestante > 0)
        {
            textoVelocidad.text = "Velocidad: " + Mathf.Ceil(jugador.tiempoPoderVelocidadRestante) + "s";
        }
        else
        {
            textoVelocidad.text = "Velocidad: Normal";
        }
    }
}
