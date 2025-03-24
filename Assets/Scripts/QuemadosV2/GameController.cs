using UnityEngine;

public class GameController : MonoBehaviour
{
    public Jugador jugador;
    public Enemigo enemigo;

    private bool minijuegoTerminado = false;

    void Update()
    {

        // Controles Q/E
        if (Input.GetKeyDown(KeyCode.Q))
        {
            jugador.ActivarVelocidad();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            jugador.ActivarEscudo();
        }
    }
}
