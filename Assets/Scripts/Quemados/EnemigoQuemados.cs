using UnityEngine;

public class EnemigoQuemados : MonoBehaviour
{
    public float velocidad = 3f;
    public Transform limiteIzquierdo;
    public Transform limiteDerecho;
    public GameObject pelotaPrefab;
    public Transform puntoDeLanzamiento;
    public float fuerzaLanzamiento = -10f;
    public float tiempoEntreLanzamientos = 2f;
    public int vidas = 3;

    public bool TienePelota { get; set; } = false;

    private float tiempoSiguienteLanzamiento;
    private bool moviendoDerecha = true;
    private Animator anim;
    private MinijuegoManager minijuegoManager;
    private Rigidbody2D rb;

    void Start()
    {
        tiempoSiguienteLanzamiento = Time.time + tiempoEntreLanzamientos;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        minijuegoManager = Object.FindFirstObjectByType<MinijuegoManager>();

        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        MoverEnemigo();

        if (TienePelota && Time.time >= tiempoSiguienteLanzamiento)
        {
            LanzarPelota();
            tiempoSiguienteLanzamiento = Time.time + tiempoEntreLanzamientos;
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

        if (anim != null)
        {
            if (HasParameter(anim, "isMoving"))
                anim.SetBool("isMoving", true);

            if (HasParameter(anim, "MoveX"))
                anim.SetFloat("MoveX", moviendoDerecha ? 1 : -1);
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
                rbPelota.linearVelocity = new Vector2(0f, fuerzaLanzamiento);
            }

            TienePelota = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PelotaJuego")) // Ahora solo detecta "PelotaJuego"
        {
            RecibirGolpe();
            Destroy(other.gameObject);
        }
    }

    public void RecibirGolpe()
    {
        vidas--;
        Debug.Log("¡El enemigo fue golpeado! Vidas restantes: " + vidas);
        minijuegoManager.ActualizarVidas(true, vidas);

        if (vidas <= 0)
        {
            Debug.Log("¡El enemigo ha perdido!");
            minijuegoManager.VerificarFinDelJuego(true);
            gameObject.SetActive(false);
        }
    }

    // Método para verificar si el Animator tiene el parámetro antes de usarlo
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
