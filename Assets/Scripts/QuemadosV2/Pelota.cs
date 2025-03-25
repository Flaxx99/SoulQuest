using UnityEngine;

public class Pelota : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float velocidad;
    [SerializeField] private float dano;  // El daño que causará la pelota al enemigo

    private Rigidbody2D rb;
    private Enemigo enemigoObjetivo;
    private Jugador jugadorObjetivo;  // Referencia al jugador

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (enemigoObjetivo == null && jugadorObjetivo == null)
        {
            return;
        }

        MoverPelota();
    }

    void MoverPelota()
    {
        // Si la pelota tiene objetivo, la moveremos hacia él
        if (enemigoObjetivo != null)
        {
            Vector2 direccion = enemigoObjetivo.transform.position - transform.position;
            direccion.Normalize();
            rb.linearVelocity = direccion * velocidad;  // Mover la pelota hacia el enemigo
        }
        else if (jugadorObjetivo != null)
        {
            Vector2 direccion = jugadorObjetivo.transform.position - transform.position;
            direccion.Normalize();
            rb.linearVelocity = direccion * velocidad;  // Mover la pelota hacia el jugador
        }
    }

    // Inicializar la pelota con el enemigo objetivo
    public void InicializarPelota(Enemigo enemigo)
    {
        enemigoObjetivo = enemigo;  // Asignamos el enemigo como objetivo
        jugadorObjetivo = null;  // Nos aseguramos que no haya un jugador objetivo en caso de que haya un cambio
    }

    // Inicializar la pelota con el jugador objetivo
    public void InicializarPelotaParaJugador(Jugador jugador)
    {
        jugadorObjetivo = jugador;  // Asignamos el jugador como objetivo
        enemigoObjetivo = null;  // Nos aseguramos que no haya un enemigo objetivo en caso de que haya un cambio
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Verificamos si la pelota colisiona con el enemigo
        if (col.CompareTag("Enemigo"))
        {
            // Si colisiona con el enemigo, aplicamos el daño
            Enemigo enemigo = col.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                float porcentajeDano = 10f;
                // Aplicamos el daño
                enemigo.RecibirDano(porcentajeDano, true);  // Llamamos al método que reduce la salud del enemigo
                gameObject.SetActive(false);  // Desactivamos la pelota después de colisionar
            }
        }

        // Verificamos si la pelota colisiona con el jugador
        if (col.CompareTag("Player"))
        {
            Jugador jugador = col.GetComponent<Jugador>();
            if (jugador != null)
            {
                // Aplica daño basado en porcentaje (por ejemplo, 10% de la vida del jugador)
                float porcentajeDeDano = 10f; // El daño es un 10% de la vida del jugador
                jugador.RecibirGolpe(porcentajeDeDano, true); // Se pasa "true" para indicar que es porcentaje

                // Desactivamos la pelota después de colisionar
                gameObject.SetActive(false);
            }
        }
    }

    // Detecta las colisiones de la pelota con los límites
    private void OnCollisionEnter2D(Collision2D col)
    {
        // Si la pelota toca un límite, la destruye
        if (col.gameObject.CompareTag("Limite"))  // Asumimos que los límites tienen el tag "Limite"
        {
            Destroy(gameObject);  // Destruye la pelota
        }
    }
}