using UnityEngine;

public class ActivarMinijuegoSpanish : MonoBehaviour
{
    public MiniJuegoSpanish miniJuego;
    private bool minijuegoActivo = false; // Para evitar doble activación

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !minijuegoActivo) // Solo activa si no está ya activo
        {
            Debug.Log("Jugador activó el minijuego.");
            miniJuego.ActivarMiniJuego();
            minijuegoActivo = true; // Evita que se vuelva a activar antes de cerrar

            // Cambiar la música a la del minijuego
            AudioManager.instancia.CambiarMusica("Minijuego");
        }
    }

    public void ResetearMinijuego() // Llamar esta función cuando el minijuego se cierre
    {
        minijuegoActivo = false;

        // Regresar la música de los pasillos
        AudioManager.instancia.CambiarMusica("Pasillos");
    }
}
