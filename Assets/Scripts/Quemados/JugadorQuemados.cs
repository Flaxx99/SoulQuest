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
    public float tiempoEntreLanzamientos = 1.5f;

    public bool TienePelota { get; set; } = false;

    private float tiempoUltimoLanzamiento;
    private Animator anim;
    private Rigidbody2D rb;
    private MinijuegoManager minijuegoManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        minijuegoManager = Object.FindFirstObjectByType<MinijuegoManager>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        MoverJugador();

        if (TienePelota && Input.GetKeyDown(KeyCode.Space) && Time.time > tiempoUltimoLanzamiento + tiempoEntreLanzamientos)
        {
            LanzarPelota();
            tiempoUltimoLanzamiento = Time.time;
        }
    }

    void MoverJugador()
    {
        float movimiento = Input.GetAxis("Horizontal");

        float nuevaPosX = Mathf.Clamp(transform.position.x + (movimiento * velocidad * Time.deltaTime), limiteIzquierdo.position.x, limiteDerecho.position.x);
        transform.position = new Vector2(nuevaPosX, transform.position.y);

        if (anim != null)
        {
            if (HasParameter(anim, "isMoving"))
                anim.SetBool("isMoving", Mathf.Abs(movimiento) > 0.1f);
        }
    }

    void LanzarPelota()
    {
        if (pelotaPrefab != null && puntoDeLanzamiento != null)
        {
            GameObject pelota = Instantiate(pelotaPrefab, puntoDeLanzamiento.position, Quaternion.identity);
            Rigidbody2D rbPelota = pelota.GetComponent<Rigidbody2D>();

            if (rbPelota != null)
            {
                rbPelota.linearVelocity = new Vector2(0f, Mathf.Abs(fuerzaLanzamiento));
            }

            TienePelota = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PelotaJuego")) // Detección unificada con el enemigo
        {
            RecibirGolpe();
            Destroy(other.gameObject);
        }
    }

    public void RecibirGolpe()
    {
        vidas--;
        Debug.Log("¡El jugador fue golpeado! Vidas restantes: " + vidas);
        minijuegoManager.ActualizarVidas(false, vidas);

        if (vidas <= 0)
        {
            Debug.Log("¡El jugador ha perdido!");
            minijuegoManager.VerificarFinDelJuego(false);
            gameObject.SetActive(false);
        }
    }

    private bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
}
