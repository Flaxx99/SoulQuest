using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorQuemados : MonoBehaviour
{
    public float velocidad = 5f;
    public Transform limiteIzquierdo;
    public Transform limiteDerecho;
    public GameObject pelotaPrefab;
    public Transform puntoDeLanzamiento;
    public float fuerzaLanzamiento = 10f;
    public int vidas = 3;
    public bool yaLanzoPelota = false; // Evita que el jugador lance varias veces en un turno
    public bool TienePelota { get; set; } = false;

    private Rigidbody2D rb;
    private Animator anim;
    private MinijuegoManager minijuegoManager;

    [Range(0f, 1f)] public float probabilidadEsquivar = 0.2f; // 20% de esquivar

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Asigna MinijuegoManager automáticamente si no está en el Inspector
        if (minijuegoManager == null)
        {
            minijuegoManager = FindFirstObjectByType<MinijuegoManager>();
            if (minijuegoManager == null)
            {
                Debug.LogError("⚠️ MinijuegoManager no encontrado en la escena.");
            }
        }

        rb.freezeRotation = true;
    }

    void Update()
    {
        MoverJugador();

        if (TienePelota && Input.GetKeyDown(KeyCode.Space))
        {
            LanzarPelota();
        }
    }

    void MoverJugador()
    {
        float movimiento = Input.GetAxis("Horizontal");
        float nuevaPosX = Mathf.Clamp(
            transform.position.x + (movimiento * velocidad * Time.deltaTime),
            limiteIzquierdo.position.x,
            limiteDerecho.position.x
        );
        transform.position = new Vector2(nuevaPosX, transform.position.y);

        if (anim != null)
        {
            anim.SetBool("isMoving", Mathf.Abs(movimiento) > 0.1f);
        }
    }

    public void LanzarPelota()
    {
        if (TienePelota && pelotaPrefab != null && puntoDeLanzamiento != null)
        {
            GameObject pelota = Instantiate(pelotaPrefab, puntoDeLanzamiento.position, Quaternion.identity);
            PelotaQuemados pelotaScript = pelota.GetComponent<PelotaQuemados>();

            if (pelotaScript != null)
            {
                Physics2D.IgnoreCollision(pelota.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

                pelotaScript.Lanzar(GameObject.FindWithTag("EnemigoQuemados").transform, Vector2.up, fuerzaLanzamiento);

                // Después de 0.5 segundos, reactiva la colisión para evitar problemas
                StartCoroutine(ReactivarColision(pelota.GetComponent<Collider2D>()));
            }

            TienePelota = false;
        }
    }

    private IEnumerator ReactivarColision(Collider2D pelotaCollider)
    {
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(pelotaCollider, GetComponent<Collider2D>(), false);
    }

    public void RecibirGolpe()
    {
        if (minijuegoManager == null)
        {
            Debug.LogError("⚠️ MinijuegoManager no está asignado en JugadorQuemados.");
            return;
        }

        float chance = Random.value; // Número aleatorio entre 0 y 1

        if (chance < probabilidadEsquivar)
        {
            Debug.Log("🌀 ¡El jugador esquivó la pelota!");
            // Cambiamos el turno
            minijuegoManager.CambiarTurno();
            return;
        }

        // Si no esquiva, recibe el golpe
        vidas--;
        Debug.Log("💥 ¡El jugador fue golpeado! Vidas restantes: " + vidas);

        minijuegoManager.ActualizarVidas(true, vidas);

        if (vidas <= 0)
        {
            minijuegoManager.VerificarFinDelJuego(false);
            gameObject.SetActive(false);
        }
        else
        {
            TienePelota = true;
            minijuegoManager.CambiarTurno();
        }
    }
}
