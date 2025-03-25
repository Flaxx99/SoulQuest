using UnityEngine;

public class Jugador : MonoBehaviour
{
    public int vidas = 5;  // Vidas del jugador
    public GameObject pelotaPrefab;
    public Transform puntoDeLanzamiento;
    public float fuerzaLanzamiento = 10f;
    public float velocidadAumentada = 7f;  // Para el poder de velocidad
    public float tiempoEscudo = 5f;  // Duración del escudo
    public float velocidad = 5f;  // Velocidad normal del jugador

    private Rigidbody2D rb;
    private bool poderVelocidadActivo = false;
    private bool escudoActivo = false;
    public float tiempoEscudoRestante = 0f;
    public float tiempoPoderVelocidadRestante = 0f;

    // Variable para mantener la referencia al MinijuegoUIController
    private MinijuegoUIController minijuegoUIController;
    private PersonajeVida personajeVida;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Obtener la referencia del UIController desde la escena
        minijuegoUIController = FindAnyObjectByType<MinijuegoUIController>();  // Usamos FindObjectOfType para encontrarlo

        personajeVida = GetComponent<PersonajeVida>();
    }

    void Update()
    {
        MoverJugador();
        LanzarPelota();

        // Actualizar los poderes
        if (escudoActivo)
        {
            tiempoEscudoRestante -= Time.deltaTime;
            if (tiempoEscudoRestante <= 0)
            {
                DesactivarEscudo();
            }
        }

        if (poderVelocidadActivo)
        {
            tiempoPoderVelocidadRestante -= Time.deltaTime;
            if (tiempoPoderVelocidadRestante <= 0)
            {
                DesactivarVelocidad();
            }
        }
    }

    void MoverJugador()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movimiento = new Vector2(horizontal, vertical).normalized * (poderVelocidadActivo ? velocidadAumentada : velocidad);
        rb.linearVelocity = movimiento;
    }

    void LanzarPelota()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject pelota = Instantiate(pelotaPrefab, puntoDeLanzamiento.position, Quaternion.identity);
            Rigidbody2D rbPelota = pelota.GetComponent<Rigidbody2D>();
            Vector2 direccion = Vector2.up;  // Dirección hacia arriba
            rbPelota.AddForce(direccion * fuerzaLanzamiento, ForceMode2D.Impulse);
        }
    }

    public void RecibirGolpe(float cantidad, bool esPorcentaje = false)
    {
        if (esPorcentaje)
        {
            // Si el daño es en porcentaje, calcula el daño basado en el porcentaje de salud máxima
            cantidad = (cantidad / 100f) * personajeVida.SaludMax; // Asegúrate de que PersonajeVida sea una instancia válida
        }

        // Ahora llamamos al método de PersonajeVida para aplicar el daño
        personajeVida.RecibirDano(cantidad); // Asegúrate de que PersonajeVida es accesible correctamente
    }


    // Método para activar la velocidad aumentada
    public void ActivarVelocidad()
    {
        poderVelocidadActivo = true;
        tiempoPoderVelocidadRestante = 5f;  // El poder dura 5 segundos
        Debug.Log("Velocidad aumentada activada");
    }

    // Método para activar el escudo
    public void ActivarEscudo()
    {
        escudoActivo = true;
        tiempoEscudoRestante = tiempoEscudo;  // El escudo dura el tiempo definido
        Debug.Log("Escudo activado");
    }

    // Método para desactivar la velocidad
    void DesactivarVelocidad()
    {
        poderVelocidadActivo = false;
        Debug.Log("Velocidad normalizada");
    }

    // Método para desactivar el escudo
    void DesactivarEscudo()
    {
        escudoActivo = false;
        Debug.Log("Escudo desactivado");
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("PelotaJuego"))
        {
            float porcentajeDeDano = 10f;
            RecibirGolpe(porcentajeDeDano, true);  // El jugador recibe daño de la pelota
        }
    }
}