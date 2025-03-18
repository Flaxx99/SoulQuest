using UnityEngine;
using System.Collections;

public class EnemigoQuemados : MonoBehaviour
{
    public float velocidad = 3f;
    public Transform limiteIzquierdo;
    public Transform limiteDerecho;
    public GameObject pelotaPrefab;
    public Transform puntoDeLanzamiento;
    public float fuerzaLanzamiento = -10f;
    public float tiempoEntreLanzamientos = 1.5f;
    public int vidas = 3;
    public JugadorQuemados jugador; // Variable para almacenar al jugador
    public bool yaLanzoPelota = false;

    public bool TienePelota { get; set; } = false;

    private bool moviendoDerecha = true;
    private Animator anim;
    private MinijuegoManager minijuegoManager;
    private Rigidbody2D rb;

    [Range(0f, 1f)] public float probabilidadEsquivar = 0.2f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Asignar el MinijuegoManager
        minijuegoManager = FindFirstObjectByType<MinijuegoManager>();
        if (minijuegoManager == null)
        {
            Debug.LogError("❌ MinijuegoManager NO encontrado en la escena.");
        }

        if (rb != null)
            rb.freezeRotation = true;
    }

    void Update()
    {
        MoverEnemigo();

        if (TienePelota)
        {
            StartCoroutine(LanzarPelotaConRetraso());
            TienePelota = false;
        }
    }

    void MoverEnemigo()
    {
        if (moviendoDerecha)
        {
            transform.position += Vector3.right * velocidad * Time.deltaTime;
            if (transform.position.x >= limiteDerecho.position.x)
                moviendoDerecha = false;
        }
        else
        {
            transform.position += Vector3.left * velocidad * Time.deltaTime;
            if (transform.position.x <= limiteIzquierdo.position.x)
                moviendoDerecha = true;
        }
    }

    private IEnumerator LanzarPelotaConRetraso()
    {
        yield return new WaitForSeconds(1.0f);
        if (yaLanzoPelota)
        {
            LanzarPelota();
        }
    }

    public void LanzarPelota()
    {
        if (pelotaPrefab != null && puntoDeLanzamiento != null)
        {
            GameObject pelota = Instantiate(pelotaPrefab, puntoDeLanzamiento.position, Quaternion.identity);
            PelotaQuemados pelotaScript = pelota.GetComponent<PelotaQuemados>();

            if (pelotaScript != null)
            {
                Debug.Log("⚽ El enemigo lanza la pelota al jugador.");
                pelotaScript.Lanzar(jugador.transform, new Vector2(0, -1), Mathf.Abs(fuerzaLanzamiento));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PelotaJuego"))
        {
            RecibirGolpe();
            Destroy(other.gameObject);
        }
    }

    private bool golpeRecibido = false;

    public void RecibirGolpe()
    {
        if (!gameObject.activeInHierarchy) return;
        if (golpeRecibido) return;
        golpeRecibido = true;

        if (minijuegoManager == null)
        {
            Debug.LogError("⚠️ MinijuegoManager no está asignado en EnemigoQuemados.");
            return;
        }

        vidas--;
        Debug.Log("🔥 ¡El enemigo fue golpeado! Vidas restantes: " + vidas);
        minijuegoManager.ActualizarVidas(false, vidas);

        if (vidas <= 0)
        {
            Debug.Log("💀 El enemigo ha perdido...");
            StopAllCoroutines();
            minijuegoManager.VerificarFinDelJuego(true);
            gameObject.SetActive(false);
        }
        else
        {
            LanzarPelota();
            minijuegoManager.CambiarTurno();
            StartCoroutine(ResetGolpe());
        }
    }

    private IEnumerator ResetGolpe()
    {
        yield return new WaitForSeconds(0.5f);
        golpeRecibido = false;
    }

    public void TomarPelota()
    {
        Debug.Log("🔴 El enemigo atrapó la pelota");
        TienePelota = true;
    }
}
