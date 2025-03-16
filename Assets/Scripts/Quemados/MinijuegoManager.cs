using UnityEngine;
using TMPro;

public class MinijuegoManager : MonoBehaviour
{
    public TextMeshProUGUI textoVidasJugador;
    public TextMeshProUGUI textoVidasEnemigo;
    public GameObject panelVictoria;
    public GameObject panelDerrota;
    public GameObject canvasMinijuego;
    public InicioMinijuego inicioMinijuego;

    private bool minijuegoFinalizado = false;

    void Start()
    {
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);
        ActualizarVidas(false, 3);
        ActualizarVidas(true, 3);
    }

    public void ActualizarVidas(bool esEnemigo, int vidas)
    {
        if (esEnemigo)
        {
            textoVidasEnemigo.text = "Enemigo: " + vidas;
        }
        else
        {
            textoVidasJugador.text = "Jugador: " + vidas;
        }
    }

    public void VerificarFinDelJuego(bool jugadorGana)
    {
        if (minijuegoFinalizado) return;
        minijuegoFinalizado = true;

        if (jugadorGana)
        {
            panelVictoria.SetActive(true);
        }
        else
        {
            panelDerrota.SetActive(true);
        }
    }

    public void Continuar()
    {
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);
        canvasMinijuego.SetActive(false);
        inicioMinijuego.FinalizarMinijuego();
    }
}
