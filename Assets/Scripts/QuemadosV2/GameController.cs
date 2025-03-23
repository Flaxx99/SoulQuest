using UnityEngine;

public class GameController : MonoBehaviour
{
    public Jugador jugador;
    public Enemigo enemigo;

    void Update()
    {
        if (jugador.vidas <= 0)
        {
            // Lógica para cuando el jugador pierde
            Debug.Log("El juego ha terminado. El jugador ha perdido.");
        }

        if (enemigo.vidas <= 0)
        {
            // Lógica para cuando el enemigo pierde
            Debug.Log("¡El jugador ha ganado!");
        }

        if (Input.GetKeyDown(KeyCode.Q))  // Activar velocidad aumentada
        {
            jugador.ActivarVelocidad();
        }

        if (Input.GetKeyDown(KeyCode.E))  // Activar escudo
        {
            jugador.ActivarEscudo();
        }
    }
}
