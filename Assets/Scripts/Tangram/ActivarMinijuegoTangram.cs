using UnityEngine;

public class ActivarMinijuegoTangram : MonoBehaviour
{
    public MinijuegoTangram miniJuego;
    public int nivelRequerido = 2;
    private bool minijuegoActivo = false;
    private PersonajeExperiencia personajeExperiencia;

    private void Start()
    {
        personajeExperiencia = Object.FindFirstObjectByType<PersonajeExperiencia>();

        if (personajeExperiencia == null)
        {
            Debug.LogError("No se encontró PersonajeExperiencia en la escena.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !minijuegoActivo)
        {
            int nivelJugador = personajeExperiencia.ObtenerNivel(); // Obtener el nivel actual del jugador

            if (nivelJugador >= nivelRequerido)
            {
                Debug.Log("Jugador activó el minijuego de Tangram.");
                miniJuego.ActivarMiniJuego();
                minijuegoActivo = true;
                AudioManager.instancia.CambiarMusica("Minijuego");
            }
            else
            {
                Debug.Log("¡No tienes el nivel suficiente para jugar este minijuego!");
                UIManager.Instance.MostrarMensaje($"Necesitas tener nivel {nivelRequerido} para jugar este minijuego.");
            }
        }
    }

    public void ResetearMinijuego()
    {
        minijuegoActivo = false;
        AudioManager.instancia.CambiarMusica("Pasillos");
    }
}
