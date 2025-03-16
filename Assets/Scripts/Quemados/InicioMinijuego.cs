using UnityEngine;

public class InicioMinijuego : MonoBehaviour
{
    public Transform spawnJugadorMinijuego;
    public Transform spawnEnemigoMinijuego;
    public Transform spawnPelotaMinijuego;
    public GameObject jugador;
    public GameObject enemigo;
    public GameObject pelotaPrefab;
    public GameObject salonGym;
    public GameObject salones;
    public GameObject canvasMinijuego;
    public Camera mainCamera;

    private Vector3 posicionOriginalJugador;
    private Vector3 posicionOriginalEnemigo;
    private bool minijuegoActivo = false;
    private bool[] estadoSalones;

    private void Start()
    {
        posicionOriginalJugador = jugador.transform.position;
        posicionOriginalEnemigo = enemigo.transform.position;
        salonGym.SetActive(false);
        canvasMinijuego.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!minijuegoActivo && other.CompareTag("Player"))
        {
            minijuegoActivo = true;
            IniciarMinijuego();
        }
    }

    public void IniciarMinijuego()
    {
        // Desactivar el script de movimiento normal y activar el de minijuego
        jugador.GetComponent<PersonajeMovimiento>().enabled = false;
        jugador.GetComponent<JugadorQuemados>().enabled = true;

        // Guardar el estado de los salones
        estadoSalones = new bool[salones.transform.childCount];
        for (int i = 0; i < salones.transform.childCount; i++)
        {
            estadoSalones[i] = salones.transform.GetChild(i).gameObject.activeSelf;
            salones.transform.GetChild(i).gameObject.SetActive(false);
        }

        salonGym.SetActive(true);
        canvasMinijuego.SetActive(true);

        jugador.transform.position = spawnJugadorMinijuego.position;
        enemigo.transform.position = spawnEnemigoMinijuego.position;

        GameObject pelotaInicial = GameObject.FindWithTag("PelotaInicial");
        if (pelotaInicial != null)
        {
            Destroy(pelotaInicial);
        }

        GameObject pelota = Instantiate(pelotaPrefab, spawnPelotaMinijuego.position, Quaternion.identity);
        pelota.tag = "PelotaJuego";

        if (Random.Range(0, 2) == 0)
        {
            jugador.GetComponent<JugadorQuemados>().TienePelota = true;
        }
        else
        {
            enemigo.GetComponent<EnemigoQuemados>().TienePelota = true;
        }

        mainCamera.transform.position = new Vector3(salonGym.transform.position.x, salonGym.transform.position.y, mainCamera.transform.position.z);
        mainCamera.orthographicSize = 5;
    }

    public void FinalizarMinijuego()
    {
        // Restaurar el script de movimiento normal y desactivar el de minijuego
        jugador.GetComponent<PersonajeMovimiento>().enabled = true;
        jugador.GetComponent<JugadorQuemados>().enabled = false;

        // Restaurar los salones
        for (int i = 0; i < salones.transform.childCount; i++)
        {
            salones.transform.GetChild(i).gameObject.SetActive(estadoSalones[i]);
        }

        salonGym.SetActive(false);
        canvasMinijuego.SetActive(false);

        jugador.transform.position = posicionOriginalJugador;
        enemigo.transform.position = posicionOriginalEnemigo;

        mainCamera.transform.position = new Vector3(posicionOriginalJugador.x, posicionOriginalJugador.y, mainCamera.transform.position.z);
        mainCamera.orthographicSize = 7;

        minijuegoActivo = false;
    }
}
