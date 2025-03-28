using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float velocidad = 3f;
    public GameObject pelotaPrefab;
    public Transform puntoDeLanzamiento;
    public float fuerzaLanzamiento = 20f;
    public int vidas = 5;  // Vidas del enemigo

    public Transform limiteIzquierdo;  // Referencia al límite izquierdo
    public Transform limiteDerecho;    // Referencia al límite derecho

    private float tiempoRestante = 0.5f;  // Tiempo entre lanzamientos
    private float tiempoDeMovimiento = 3f;  // Tiempo de cambio de dirección
    private float tiempoDeCambio;
    private Rigidbody2D rb;

    private float velocidadObjetivo;

    private MinijuegoUIController minijuegoUIController;  // Referencia al MinijuegoUIController
    private EnemigoVidaQuemados enemigoVida;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tiempoDeCambio = tiempoDeMovimiento;
        velocidadObjetivo = velocidad;  // Iniciar con la velocidad original
        vidas = Mathf.Max(vidas, 0);  // Asegurarnos de que las vidas no sean negativas

        // Obtener la referencia al MinijuegoUIController en la escena
        minijuegoUIController = FindFirstObjectByType<MinijuegoUIController>();

        if (minijuegoUIController == null)
        {
            Debug.LogError("MinijuegoUIController no encontrado en la escena.");
        }

        enemigoVida = GetComponent<EnemigoVidaQuemados>();
    }

    void Update()
    {
        MoverEnemigo();
        LanzarPelota();
    }

    void MoverEnemigo()
    {
        // Cambiar dirección cuando el tiempo de movimiento haya terminado
        if (tiempoDeCambio <= 0)
        {
            velocidadObjetivo = -velocidadObjetivo;  // Cambiar la dirección de la velocidad
            tiempoDeCambio = tiempoDeMovimiento;  // Resetear el tiempo
        }

        // Usamos Mathf.Lerp para hacer un movimiento más suave y continuo
        float velocidadSuavizada = Mathf.Lerp(rb.linearVelocity.x, velocidadObjetivo, Time.deltaTime * 3f);
        rb.linearVelocity = new Vector2(velocidadSuavizada, rb.linearVelocity.y);

        // Verificar si el enemigo alcanza los límites del campo y cambiar la dirección
        if (transform.position.x >= limiteDerecho.position.x || transform.position.x <= limiteIzquierdo.position.x)
        {
            velocidadObjetivo = -velocidadObjetivo;  // Cambiar dirección cuando llegue al borde
        }

        tiempoDeCambio -= Time.deltaTime;  // Reducir el tiempo para cambiar de dirección
    }

    void LanzarPelota()
    {
        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            GameObject pelota = Instantiate(pelotaPrefab, puntoDeLanzamiento.position, Quaternion.identity);
            Rigidbody2D rbPelota = pelota.GetComponent<Rigidbody2D>();
            Vector2 direccion = Vector2.down;  // Dirección hacia abajo
            rbPelota.AddForce(direccion * fuerzaLanzamiento, ForceMode2D.Impulse);
            tiempoRestante = 2f;  // Resetear el tiempo de lanzamiento
        }
    }

    // Método para recibir daño
    public void RecibirDano(float cantidad, bool esPorcentaje = false)
    {
        if (enemigoVida != null)
        {
            enemigoVida.RecibirDano(cantidad, esPorcentaje);  // Llama al método RecibirDano de EnemigoVida
        }
    }

    private void DesactivarEnemigo()
    {
        gameObject.SetActive(false);  // Desactivamos el objeto enemigo cuando muere
    }

    // Método de ejemplo que puede ser llamado cuando el enemigo colisiona con algo
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("PelotaJuego"))
        {
            // Aquí aplicamos daño en porcentaje (10% de la vida del enemigo)
            float porcentajeDeDano = 10f;  // Porcentaje de daño
            RecibirDano(porcentajeDeDano, true);  // Llamamos al método para aplicar el daño
        }
    }
}