using UnityEngine;

public class InicioMinijuego : MonoBehaviour
{
    public Transform SpawnJugador; // Punto de inicio del jugador
    public Transform SpawnEnemigo; // Punto de inicio del enemigo
    public GameObject jugador;
    public GameObject enemigo;
    public GameObject pelotaInicial;  // Pelota inicial que aparece en la cancha

    public MinijuegoUIController minijuegoUIController;  // Referencia al script de UI
    public GameObject canvasMinijuego;  // Referencia al Canvas del minijuego

    void Start()
    {
        // Desactivar al enemigo antes de comenzar el minijuego
       
        if (enemigo != null)
            enemigo.SetActive(false);

        // Activar el Canvas del minijuego
        if (canvasMinijuego != null)
        {
            canvasMinijuego.SetActive(true);  // Activamos el Canvas
        }

        // Activar la pelota inicial
        if (pelotaInicial != null)
            pelotaInicial.SetActive(true);
    }

    // Cuando el jugador entra en el área de inicio, comienza el minijuego
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Minijuego de Quemados iniciado!");
            AudioManager.instancia.CambiarMusica("Quemados");
            // Activar al jugador y enemigo en el minijuego
            if (jugador != null)
            {
                jugador.SetActive(true);
                jugador.GetComponent<Jugador>().enabled = true;  // Activar el script Jugador
            }
            if (enemigo != null)
            {
                enemigo.SetActive(true);
            }

            // Mover a los personajes a sus posiciones iniciales
            if (SpawnJugador != null)
                jugador.transform.position = SpawnJugador.position;
            if (SpawnEnemigo != null)
                enemigo.transform.position = SpawnEnemigo.position;

            // Desactivar la pelota inicial
            if (pelotaInicial != null)
                pelotaInicial.SetActive(false);

            // Iniciar la UI del minijuego (por si fuera necesario resetear o mostrar algún valor)
            if (minijuegoUIController != null)
            {
                minijuegoUIController.OcultarPanelesUI(); // Ocultar los paneles de la UI
            }
        }
    }

    // Método para finalizar el minijuego y regresar al mapa principal (o al estado deseado)
    public void FinalizarMinijuego(bool jugadorGano)
    {
        if (minijuegoUIController != null)
        {
            minijuegoUIController.FinalizarMinijuego(jugadorGano);  // Llamar al método FinalizarMinijuego en MinijuegoUIController
        }

        // Restaurar los paneles cuando el minijuego termine
        if (minijuegoUIController != null)
        {
            minijuegoUIController.MostrarPanelesUI();
        }
    }
}
