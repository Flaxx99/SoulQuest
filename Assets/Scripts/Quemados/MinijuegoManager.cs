using UnityEngine;
using TMPro;
using System.Collections;

public class MinijuegoManager : MonoBehaviour
{
    // --- Referencias a la UI ---
    public TextMeshProUGUI textoVidasJugador;
    public TextMeshProUGUI textoVidasEnemigo;
    public GameObject panelVictoria;
    public GameObject panelDerrota;
    public GameObject canvasMinijuego;

    // --- Referencias a los participantes ---
    public JugadorQuemados jugador;  // O usa public GameObject jugador; si prefieres
    public EnemigoQuemados enemigo;

    // --- Referencia a la cámara (opcional) ---
    public Camera mainCamera;

    // --- Opcional: si creas pelotas al inicio ---
    public Transform spawnJugadorMinijuego;
    public Transform spawnEnemigoMinijuego;
    public Transform spawnPelotaMinijuego;
    public GameObject pelotaPrefab;

    private bool turnoJugador = true;
    private bool juegoTerminado = false;

    void Start()
    {
        juegoTerminado = false;

        // Desactivar al enemigo para que no lance antes de tiempo
        if (enemigo != null)
            enemigo.gameObject.SetActive(false);

        // Ocultar paneles de victoria/derrota
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);
    }

    // Llamado cuando quieras iniciar el minijuego
    public void IniciarMinijuego()
    {
        Debug.Log("🎮 Iniciando Minijuego...");

        // Activar al enemigo
        if (enemigo != null)
            enemigo.gameObject.SetActive(true);

        // Reiniciar vidas
        jugador.vidas = 3;
        enemigo.vidas = 3;
        juegoTerminado = false;

        DeterminarPrimerTurno();
        ActualizarVidas(true, jugador.vidas);
        ActualizarVidas(false, enemigo.vidas);

        // Activar la UI del minijuego
        if (canvasMinijuego != null)
        {
            canvasMinijuego.SetActive(true);
            Debug.Log("✅ Canvas del minijuego activado.");
        }

        // Opcional: reposicionar jugador y enemigo si quieres
        if (spawnJugadorMinijuego != null)
            jugador.transform.position = spawnJugadorMinijuego.position;
        if (spawnEnemigoMinijuego != null)
            enemigo.transform.position = spawnEnemigoMinijuego.position;

        // Destruir pelotas anteriores
        foreach (GameObject pelotaExistente in GameObject.FindGameObjectsWithTag("PelotaJuego"))
        {
            Destroy(pelotaExistente);
        }

        // Crear nueva pelota (opcional)
        if (pelotaPrefab != null && spawnPelotaMinijuego != null)
        {
            GameObject nuevaPelota = Instantiate(pelotaPrefab, spawnPelotaMinijuego.position, Quaternion.identity);
            nuevaPelota.tag = "PelotaJuego";
        }

        // Asignar pelota aleatoriamente
        if (Random.Range(0, 2) == 0)
            jugador.TienePelota = true;
        else
            enemigo.TienePelota = true;

        // Si el enemigo inicia, retrasar un segundo para lanzar la pelota
        if (!turnoJugador)
        {
            Debug.Log("⚽ [Inicio] El enemigo tiene la pelota y la lanzará ahora.");
            StartCoroutine(EsperarYLanzarPelota());
        }
    }

    private void DeterminarPrimerTurno()
    {
        turnoJugador = (Random.Range(0, 2) == 0);
        AsignarPelotaInicial();
    }

    private void AsignarPelotaInicial()
    {
        if (turnoJugador)
        {
            jugador.TienePelota = true;
        }
        else
        {
            enemigo.TienePelota = true;
            enemigo.LanzarPelota();
        }
    }

    private IEnumerator EsperarYLanzarPelota()
    {
        yield return new WaitForSeconds(1.0f);
        enemigo.LanzarPelota();
    }

    public void CambiarTurno()
    {
        if (juegoTerminado) return;

        turnoJugador = !turnoJugador;

        if (turnoJugador)
        {
            jugador.TienePelota = true;
            enemigo.yaLanzoPelota = false;
        }
        else
        {
            enemigo.TienePelota = true;
            jugador.yaLanzoPelota = false;
        }
    }

    public void VerificarFinDelJuego(bool ganoJugador)
    {
        if (juegoTerminado) return;
        if (jugador.vidas > 0 && enemigo.vidas > 0) return;

        juegoTerminado = true;

        if (ganoJugador)
        {
            Debug.Log("🎉 ¡El jugador ganó!");
            if (panelVictoria != null) panelVictoria.SetActive(true);
        }
        else
        {
            Debug.Log("💀 El jugador perdió...");
            if (panelDerrota != null) panelDerrota.SetActive(true);
        }

        // Deshabilitar control de minijuego
        jugador.enabled = false;
        enemigo.enabled = false;
    }

    // Método unificado para cerrar el minijuego SIN ocultar el mapa
    public void CerrarMinijuego()
    {
        Debug.Log("Cerrando el minijuego sin desactivar nada del mapa...");

        Time.timeScale = 1;

        // Desactivar solo la UI del minijuego
        if (canvasMinijuego != null)
            canvasMinijuego.SetActive(false);

        // Apagar JugadorQuemados, encender PersonajeMovimiento
        if (jugador != null)
        {
            // Apagar JugadorQuemados
            JugadorQuemados jq = jugador.GetComponent<JugadorQuemados>();
            if (jq != null)
            {
                jq.enabled = false;
                Debug.Log("JugadorQuemados desactivado.");
            }

            // Encender PersonajeMovimiento
            PersonajeMovimiento pm = jugador.GetComponent<PersonajeMovimiento>();
            if (pm != null)
            {
                pm.enabled = true;
                Debug.Log("PersonajeMovimiento activado.");
            }
            else
            {
                Debug.LogError("No se encontró PersonajeMovimiento en el jugador.");
            }
        }

        Debug.Log("¡Minijuego cerrado sin desactivar el mapa!");
    }

    public void ActualizarVidas(bool esJugador, int vidasRestantes)
    {
        Debug.Log($"Actualizando vidas - Jugador: {esJugador}, Vidas: {vidasRestantes}");
        if (esJugador)
        {
            if (textoVidasJugador != null)
                textoVidasJugador.text = "Jugador: " + vidasRestantes;
        }
        else
        {
            if (textoVidasEnemigo != null)
                textoVidasEnemigo.text = "Enemigo: " + vidasRestantes;
        }
    }
}
