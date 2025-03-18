using UnityEngine;

public class PelotaQuemados : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform objetivo;
    private bool enMovimiento = false;
    public float velocidadSeguimiento = 5f;
    private MinijuegoManager minijuegoManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        minijuegoManager = FindFirstObjectByType<MinijuegoManager>();
    }

    void Update()
    {
        if (enMovimiento && rb != null)
        {
            Vector2 direccion = (objetivo.position - transform.position).normalized;
            rb.linearVelocity = direccion * velocidadSeguimiento;
        }
    }

    public void Lanzar(Transform nuevoObjetivo, Vector2 direccion, float fuerza)
    {
        objetivo = nuevoObjetivo;
        enMovimiento = true;

        if (rb != null)
        {
            rb.linearVelocity = direccion * fuerza;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto ya está inactivo, no hacemos nada
        if (!other.gameObject.activeInHierarchy)
        {
            Debug.Log("⚠️ Colisión ignorada porque el objeto está inactivo.");
            return;
        }

        Debug.Log($"⚡ La pelota colisionó con {other.gameObject.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("💥 La pelota golpeó al jugador.");
            other.GetComponent<JugadorQuemados>().RecibirGolpe();
            // Desactivar collider para evitar dobles colisiones
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }
        else if (other.CompareTag("EnemigoQuemados"))
        {
            Debug.Log("💥 La pelota golpeó al enemigo.");
            other.GetComponent<EnemigoQuemados>().RecibirGolpe();
            // Desactivar collider para evitar dobles colisiones
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }

        if (other.CompareTag("Limite"))
        {
            Destroy(gameObject);
        }
    }
}
