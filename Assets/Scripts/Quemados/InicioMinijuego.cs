using UnityEngine;

public class InicioMinijuego : MonoBehaviour
{
    public Transform spawnJugadorMinijuego;
    public Transform spawnEnemigoMinijuego;
    public Transform spawnPelotaMinijuego;
    public GameObject jugador;       // El mismo "Alumno"
    public GameObject enemigo;       // El enemigo
    public GameObject pelotaPrefab;
    public GameObject canvasMinijuego; // UI del minijuego
    //public GameObject salonGym;     // Comentado: si no quieres usarlo

    public Camera mainCamera;

    private Vector3 posicionOriginalJugador;
    private Vector3 posicionOriginalEnemigo;
    private bool minijuegoActivo = false;

    private void Start()
    {
        // Guardamos posiciones originales
        posicionOriginalJugador = jugador.transform.position;
        posicionOriginalEnemigo = enemigo.transform.position;

        // Dejar inactiva la UI del minijuego al principio
        if (canvasMinijuego != null) canvasMinijuego.SetActive(false);
        // if (salonGym != null) salonGym.SetActive(false); // Comentado si no quieres ocultar nada
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!minijuegoActivo && other.CompareTag("Player"))
        {
            minijuegoActivo = true;
            IniciarMinijuego();

            // Destruir este objeto si es la PelotaInicial
            Destroy(gameObject);
        }
    }

    public void IniciarMinijuego()
    {
        minijuegoActivo = true;

        // Inicializar vidas
        jugador.GetComponent<JugadorQuemados>().vidas = 3;
        enemigo.GetComponent<EnemigoQuemados>().vidas = 3;

        // Evitar que se duplique el canvas
        if (canvasMinijuego != null && canvasMinijuego.activeSelf) return;

        // Apagar PersonajeMovimiento y encender JugadorQuemados
        if (jugador != null)
        {
            PersonajeMovimiento pm = jugador.GetComponent<PersonajeMovimiento>();
            if (pm != null) pm.enabled = false;

            JugadorQuemados jq = jugador.GetComponent<JugadorQuemados>();
            if (jq != null) jq.enabled = true;
        }

        // Activar la UI del minijuego
        if (canvasMinijuego != null) canvasMinijuego.SetActive(true);

        // if (salonGym != null) salonGym.SetActive(true); // Comentado para no ocultar nada del mapa

        // Reposicionar jugador y enemigo
        if (spawnJugadorMinijuego != null)
            jugador.transform.position = spawnJugadorMinijuego.position;
        if (spawnEnemigoMinijuego != null)
            enemigo.transform.position = spawnEnemigoMinijuego.position;

        // Destruir pelotas anteriores
        foreach (GameObject pelotaExistente in GameObject.FindGameObjectsWithTag("PelotaJuego"))
        {
            Destroy(pelotaExistente);
        }

        // Crear la nueva pelota
        if (pelotaPrefab != null && spawnPelotaMinijuego != null)
        {
            GameObject nuevaPelota = Instantiate(pelotaPrefab, spawnPelotaMinijuego.position, Quaternion.identity);
            nuevaPelota.tag = "PelotaJuego";
        }

        // Asignar la pelota aleatoriamente
        if (Random.Range(0, 2) == 0)
        {
            jugador.GetComponent<JugadorQuemados>().TienePelota = true;
        }
        else
        {
            enemigo.GetComponent<EnemigoQuemados>().TienePelota = true;
        }

        // Ajustar la cámara al minijuego si quieres
        if (mainCamera != null /* && salonGym != null */)
        {
            // Ejemplo: centrar en spawnJugadorMinijuego
            // mainCamera.transform.position = new Vector3(spawnJugadorMinijuego.position.x, spawnJugadorMinijuego.position.y, mainCamera.transform.position.z);
            // mainCamera.orthographicSize = 5;
        }

        // Llamar al MinijuegoManager
        MinijuegoManager manager = FindFirstObjectByType<MinijuegoManager>();
        if (manager != null)
        {
            manager.IniciarMinijuego();
        }
        else
        {
            Debug.LogError("No se encontró MinijuegoManager en la escena.");
        }
    }

    // Punto final si deseas que aparezca en cierto lugar
    public Transform puntoFinMinijuego;

    public void FinalizarMinijuego()
    {
        Debug.Log("Finalizando minijuego...");

        // Aseguramos que el jugador se vea y se posicione en su lugar final
        if (jugador != null)
        {
            jugador.gameObject.SetActive(true);

            Vector3 nuevaPos;
            if (puntoFinMinijuego != null)
            {
                nuevaPos = puntoFinMinijuego.position;
                Debug.Log("Usando puntoFinMinijuego para la posición final.");
            }
            else
            {
                nuevaPos = posicionOriginalJugador;
                Debug.Log("Usando posicionOriginalJugador para la posición final.");
            }
            nuevaPos.z = 0;
            jugador.transform.position = nuevaPos;
            Debug.Log("Jugador reposicionado en: " + nuevaPos);
        }

        // if (salonGym != null) salonGym.SetActive(false); // Comentado para no ocultar nada

        if (canvasMinijuego != null) canvasMinijuego.SetActive(false);

        // Ajustar la cámara si quieres
        if (mainCamera != null && jugador != null)
        {
            Vector3 posCam = new Vector3(jugador.transform.position.x, jugador.transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = posCam;
            mainCamera.orthographicSize = 7;
        }

        Time.timeScale = 1;
        minijuegoActivo = false;

        // Apagar JugadorQuemados, encender PersonajeMovimiento
        JugadorQuemados jq2 = jugador.GetComponent<JugadorQuemados>();
        if (jq2 != null) jq2.enabled = false;

        PersonajeMovimiento pm2 = jugador.GetComponent<PersonajeMovimiento>();
        if (pm2 != null) pm2.enabled = true;

        Debug.Log("¡Minijuego finalizado sin desactivar el mapa!");
    }
}
