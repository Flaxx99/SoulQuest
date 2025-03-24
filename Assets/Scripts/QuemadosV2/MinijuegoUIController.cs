using UnityEngine;
using TMPro;  // Usamos TextMeshProUGUI para los textos
using UnityEngine.SceneManagement;
using System.Collections;

public class MinijuegoUIController : MonoBehaviour
{
    // Textos de la UI que muestran las vidas
    public TextMeshProUGUI vidaJugadorText;  // Referencia al texto de vida del jugador
    public TextMeshProUGUI vidaEnemigoText;  // Referencia al texto de vida del enemigo

    // Paneles de la UI que serán mostrados u ocultados
    public GameObject panelVictoria;  // Panel de victoria
    public GameObject panelDerrota;   // Panel de derrota

    // Paneles de la UI que vamos a ocultar durante el minijuego
    public GameObject playerUI;  // Referencia al PlayerUI
    public GameObject panelBotones;  // Referencia al PanelBotones
    public GameObject panelArmaEquipada;  // Referencia al PanelArmaEquipada

    // Referencias a los jugadores y enemigos en la escena
    public Jugador jugador;
    public Enemigo enemigo;
    public GameObject canvasMinijuego;
    public GameObject efectoTerror;
    public FinalMessageController mensajeFinalController; // Asignar en el inspector


    // public GameObject PanelFinal;
    //public float delayAntesDeTransicion = 5f;

    void Start()
    {

        // Asegurarnos de que las vidas iniciales sean positivas
        jugador.vidas = 3;
        enemigo.vidas = 10;

        // Inicializamos todo
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);
        MostrarPanelesUI(); // Asegurarse de que los paneles estén activos al inicio
        ActualizarTextosDeVida();  // Asegurarnos de que los textos se actualicen con los valores iniciales
    }

    public void ActivarTextos()
    {
        // Aseguramos que los textos estén activados
        vidaJugadorText.gameObject.SetActive(true);
        vidaEnemigoText.gameObject.SetActive(true);
        ActualizarTextosDeVida();  // Llamamos a la actualización del texto
    }

    public void ActualizarTextosDeVida()
    {
        // Actualizamos los textos de vida en la UI con los valores iniciales
        if (vidaJugadorText != null)
        {
            vidaJugadorText.text = "Jugador: " + jugador.vidas; // Mostrar las vidas iniciales del jugador
        }

        if (vidaEnemigoText != null)
        {
            vidaEnemigoText.text = "Enemigo: " + enemigo.vidas; // Mostrar las vidas iniciales del enemigo
        }
    }

    // Método para actualizar las vidas del jugador
    public void ActualizarVidasJugador(int vidasRestantes)
    {
        jugador.vidas = vidasRestantes;  // Actualizar las vidas del jugador
        ActualizarTextosDeVida();  // Llamamos a la función para actualizar el texto
    }

    // Método para actualizar las vidas del enemigo
    public void ActualizarVidasEnemigo(int vidasRestantes)
    {
        enemigo.vidas = vidasRestantes;  // Actualizar las vidas del enemigo
        ActualizarTextosDeVida();  // Llamamos a la función para actualizar el texto
    }

    // Método para mostrar el panel de derrota cuando el jugador pierde
    public void MostrarPanelDerrota(bool mostrar)
    {
        panelDerrota.SetActive(mostrar);  // Activa o desactiva el panel de derrota
        if (mostrar)
        {
            panelVictoria.SetActive(false);  // Aseguramos que el panel de victoria se desactive
        }
    }

    // Método para mostrar el panel de victoria cuando el enemigo pierde
    public void MostrarPanelVictoria(bool mostrar)
    {
        panelVictoria.SetActive(mostrar);  // Activa o desactiva el panel de victoria
        if (mostrar)
        {
            panelDerrota.SetActive(false);  // Aseguramos que el panel de derrota se desactive
        }
    }

    // Método para restar vida al jugador
    public void RecibirDanoJugador()
    {
        jugador.vidas--;  // Restar vida al jugador

        // Actualizamos los textos de vida siempre que se reciba daño
        ActualizarTextosDeVida();

        if (jugador.vidas <= 0)  // Si la vida del jugador llega a 0
        {
            MostrarPanelDerrota(true);  // Mostrar el panel de derrota
        }
    }

    // Método para restar vida al enemigo
    public void RecibirDanoEnemigo()
    {
        enemigo.vidas--;  // Restar vida al enemigo

        // Actualizamos los textos de vida siempre que el enemigo reciba daño
        ActualizarTextosDeVida();

        if (enemigo.vidas <= 0)  // Si la vida del enemigo llega a 0
        {
            MostrarPanelVictoria(true);  // Mostrar el panel de victoria
        }
    }
    private bool minijuegoTerminado = false;
    // Finaliza el minijuego, desactiva el script del jugador y muestra el panel de victoria
    /*JONATHANpublic void FinalizarMinijuego(bool jugadorGano)
    {
        if (minijuegoTerminado) return;  // Si ya se finalizó, no hagas nada más
        minijuegoTerminado = true;

        Debug.Log("Finalizando minijuego de quemados...");

        // Desactivar el script del jugador, pero no el GameObject
        if (jugador != null)
        {
            jugador.GetComponent<Jugador>().enabled = false;  // Desactivamos el script del jugador
            Debug.Log("Script del jugador desactivado");
        }

        // Desactivar al enemigo
        if (enemigo != null)
            enemigo.gameObject.SetActive(false);

        // Llamar al método de MinijuegoUIController para mostrar el panel de victoria
        MostrarPanelVictoria(jugadorGano);

        // Desactivar el Canvas del minijuego cuando termine el juego
        if (canvasMinijuego != null)
        {
            canvasMinijuego.SetActive(false);  // Desactivamos el Canvas del minijuego al finalizar
        }

        if (efectoTerror != null)
        {
            efectoTerror.SetActive(false);
        }

        // Restaurar los paneles de la UI
        MostrarPanelesUI();
    }
    */
    //PRUEBA CON PANEL Y SECUENCIA
    public void FinalizarMinijuego(bool jugadorGano)
    {
        if (minijuegoTerminado) return;
        minijuegoTerminado = true;

        Debug.Log("Finalizando minijuego de quemados...");

        // Desactivar script del jugador
        if (jugador != null)
        {
            jugador.GetComponent<Jugador>().enabled = false;
            Debug.Log("Script del jugador desactivado");
        }

        // Desactivar al enemigo
        if (enemigo != null)
            enemigo.gameObject.SetActive(false);

        /* Mostrar el panel de victoria (o panel final) antes de apagar el efecto terror
        if (PanelFinal != null)
        {
            Debug.Log("Activando panel final antes de apagar efecto terror");
            PanelFinal.SetActive(true);
            // Si tienes fade, aquí podrías llamar a UIManager.Instance.FadeIn();
        }*/

        // Luego, desactivar el canvas del minijuego, efecto terror, etc.
        if (canvasMinijuego != null)
            canvasMinijuego.SetActive(false);

        /*if (efectoTerror != null)
            efectoTerror.SetActive(false);*/

        // Restaurar paneles de UI, etc.
        MostrarPanelesUI();

        Debug.Log("Fin de la secuencia de FinalizarMinijuego.");
    }

    // Método para ocultar los paneles de UI (PlayerUI, PanelBotones, PanelArmaEquipada) durante el minijuego
    public void OcultarPanelesUI()
    {
        if (playerUI != null)
            playerUI.SetActive(false);  // Ocultar el PlayerUI

        if (panelBotones != null)
            panelBotones.SetActive(false);  // Ocultar el PanelBotones

        if (panelArmaEquipada != null)
            panelArmaEquipada.SetActive(false);  // Ocultar el PanelArmaEquipada
    }

    // Método para mostrar nuevamente los paneles ocultos después del minijuego
    public void MostrarPanelesUI()
    {
        if (playerUI != null)
            playerUI.SetActive(true);  // Mostrar el PlayerUI

        if (panelBotones != null)
            panelBotones.SetActive(true);  // Mostrar el PanelBotones

        if (panelArmaEquipada != null)
            panelArmaEquipada.SetActive(true);  // Mostrar el PanelArmaEquipada
    }

    // Método que se llama desde el botón "Continuar" en el juego Quemados
    public void OnContinuarClicked()
    {
        Debug.Log("Botón 'Continuar' presionado → Mostrando mensaje final.");
        if (mensajeFinalController != null)
        {
            mensajeFinalController.ShowFinalMessage();
        }
        else
        {
            Debug.LogWarning("No se asignó el FinalMessageController en el inspector.");
        }
    }

    /*
    // Coroutine para esperar, apagar el efecto y cargar los créditos
    IEnumerator ContinuarSecuencia()
    {
        Debug.Log("ContinuarSecuencia() → Esperando " + delayAntesDeTransicion + " segundos. Tiempo inicial: " + Time.time);

        yield return new WaitForSeconds(delayAntesDeTransicion);

        Debug.Log("ContinuarSecuencia() → Tiempo tras la espera: " + Time.time);

        // Apaga el efecto de terror
        if (efectoTerror != null)
        {
            Debug.Log("Desactivando efectoTerror: " + efectoTerror.name);
            efectoTerror.SetActive(false);
        }
        else
        {
            Debug.LogWarning("efectoTerror es null. No se desactiva nada.");
        }

        Debug.Log("Cargando escena de créditos...");
        SceneManager.LoadScene("Creditos");
    }*/

}
