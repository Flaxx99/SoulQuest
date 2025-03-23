using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float velocidad = 3f;
    public GameObject pelotaPrefab;
    public Transform puntoDeLanzamiento;
    public float fuerzaLanzamiento = 10f;
    public int vidas = 5;  // Vidas del enemigo

    public Transform limiteIzquierdo;  // Referencia al límite izquierdo
    public Transform limiteDerecho;    // Referencia al límite derecho

    private float tiempoRestante = 2f;  // Tiempo entre lanzamientos
    private float tiempoDeMovimiento = 3f;  // Tiempo de cambio de dirección
    private float tiempoDeCambio;
    private Rigidbody2D rb;

    private float velocidadObjetivo;

    private MinijuegoUIController minijuegoUIController;  // Referencia al MinijuegoUIController

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

    // Método para restar vida al enemigo
    public void RecibirGolpe(float dano)
    {
        vidas -= Mathf.CeilToInt(dano);  // Restar vida al enemigo (utilizamos Mathf.CeilToInt para asegurar que el daño se redondee correctamente)
        vidas = Mathf.Max(vidas, 0);  // Asegurarnos de que las vidas no bajen de 0

        // Actualizamos la UI y las vidas del enemigo
        if (minijuegoUIController != null)
        {
            minijuegoUIController.ActualizarVidasEnemigo(vidas);  // Actualizamos el texto en la UI
        }

        // Si el enemigo ha sido derrotado
        if (vidas <= 0)
        {
            Debug.Log("El enemigo ha sido derrotado.");
            // Mostrar el panel de victoria cuando el enemigo muere
            if (minijuegoUIController != null)
            {
                minijuegoUIController.MostrarPanelVictoria(true);
            }

            DesactivarEnemigo();  // Desactivamos al enemigo cuando sus vidas lleguen a 0
        }
        else
        {
            Debug.Log("Vidas restantes del enemigo: " + vidas);
        }
    }

    private void DesactivarEnemigo()
    {
        gameObject.SetActive(false);  // Desactivamos el objeto enemigo cuando muere
    }

    // Método para manejar la colisión con la pelota
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("PelotaJuego"))
        {
            // El enemigo recibe daño al colisionar con la pelota
            RecibirGolpe(1f);  // El daño aplicado puede ser ajustado (aquí se le aplica 1 de daño por cada golpe)
        }
    }
}
