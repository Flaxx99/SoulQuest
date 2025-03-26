using UnityEngine;

public class ValidarNivel : MonoBehaviour
{
    public int nivelRequerido = 3;
    private PersonajeExperiencia personajeExperiencia;

    void Start()
    {
        personajeExperiencia = Object.FindFirstObjectByType<PersonajeExperiencia>();

        if (personajeExperiencia == null)
        {
            Debug.LogError("No se encontró PersonajeExperiencia en la escena.");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int nivelJugador = personajeExperiencia.ObtenerNivel();
            Debug.Log($"Nivel del jugador: {nivelJugador}");

            if (nivelJugador >= nivelRequerido)
            {
                // Si tiene el nivel, no pasa nada
                Debug.Log("Nivel suficiente. El jugador puede pasar.");
            }
            else
            {
                // Mostrar mensaje cada vez que el jugador intente entrar
                Debug.Log($"Nivel insuficiente. El jugador necesita nivel {nivelRequerido} para pasar.");
                UIManager.Instance.MostrarMensaje($"Necesitas tener nivel {nivelRequerido} para jugar este minijuego.");
            }
        }
    }
}

