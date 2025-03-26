using UnityEngine;
using TMPro;  // Usamos TextMeshProUGUI para los textos
using UnityEngine.SceneManagement;
using System.Collections;

public class MinijuegoUIController : MonoBehaviour
{
    // Paneles de la UI que serán mostrados u ocultados
    public GameObject panelVictoria;  // Panel de victoria
    public GameObject panelDerrota;   // Panel de derrota

    // Paneles de la UI que vamos a ocultar durante el minijuego
    public GameObject panelBotones;  // Referencia al PanelBotones
    public GameObject panelArmaEquipada;  // Referencia al PanelArmaEquipada

    // Referencias a los jugadores y enemigos en la escena
    public Jugador jugador;
    public Enemigo enemigo;
    public GameObject canvasMinijuego;
    public GameObject efectoTerror;
    public FinalMessageController mensajeFinalController; // Asignar en el inspector

    public GameObject slideshowRestauracion;

    void Start()
    {

        // Asegurarnos de que las vidas iniciales sean positivas
        jugador.vidas = 3;
        enemigo.vidas = 10;

        // Inicializamos todo
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);
        MostrarPanelesUI(); // Asegurarse de que los paneles estén activos al inicio
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

        if (jugador.vidas <= 0)  // Si la vida del jugador llega a 0
        {
            MostrarPanelDerrota(true);  // Mostrar el panel de derrota
        }
    }

    // Método para restar vida al enemigo
    public void RecibirDanoEnemigo()
    {
        enemigo.vidas--;

        if (enemigo.vidas <= 0)  // Si la vida del enemigo llega a 0
        {
            MostrarPanelVictoria(true);  // Mostrar el panel de victoria
        }
    }
    private bool minijuegoTerminado = false;
 
    public void FinalizarMinijuego(bool jugadorGano)
    {
        if (minijuegoTerminado) return;
        minijuegoTerminado = true;

        AudioManager.instancia.CambiarMusica("Pasillos");
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

        // Llamar al método de MinijuegoUIController para mostrar el panel de victoria
        //MostrarPanelVictoria(jugadorGano);

        // Luego, desactivar el canvas del minijuego
        if (canvasMinijuego != null)
            canvasMinijuego.SetActive(false);

        // Restaurar paneles de UI, etc.
        MostrarPanelesUI();
        Debug.Log("Fin de la secuencia de FinalizarMinijuego.");
    }

    // Método para ocultar los paneles de UI (PlayerUI, PanelBotones, PanelArmaEquipada) durante el minijuego
    public void OcultarPanelesUI()
    {
        if (panelBotones != null)
            panelBotones.SetActive(false);  // Ocultar el PanelBotones

        if (panelArmaEquipada != null)
            panelArmaEquipada.SetActive(false);  // Ocultar el PanelArmaEquipada
    }

    // Método para mostrar nuevamente los paneles ocultos después del minijuego
    public void MostrarPanelesUI()
    {
        if (panelBotones != null)
            panelBotones.SetActive(true);  // Mostrar el PanelBotones

        if (panelArmaEquipada != null)
            panelArmaEquipada.SetActive(true);  // Mostrar el PanelArmaEquipada
    }

    public void OnContinuarClicked()
    {
        Debug.Log("Botón 'Continuar' presionado → Activando mensaje final.");
        MostrarPanelVictoria(false);
        // Activamos el mensaje final primero
        FinalMessageController mensajeFinalScript = mensajeFinalController.GetComponent<FinalMessageController>();
        if (mensajeFinalScript != null)
        {
            mensajeFinalScript.ShowFinalMessage();  // Llamamos al método que gestiona toda la secuencia
        }
        

        // En este punto, el mensaje final ya debería estar mostrando y luego ejecutarse el slideshow.
    }



    private IEnumerator ActivarSlideshowConDelay()
    {
        yield return new WaitForSeconds(0.5f);  // Espera un poco para asegurar que el objeto esté activo
        slideshowRestauracion.GetComponent<SlideshowRestauracion>().SecuenciaSlideshow();
    }

}
