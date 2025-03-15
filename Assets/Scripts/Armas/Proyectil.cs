using UnityEngine;
public class Proyectil : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float velocidad;

    public PersonajeAtaque personajeAtaque {  get; private set; }

    private Rigidbody2D rigidbody2;
    private Vector2 direccion;
    private EnemigoInteraccion enemigoObjetivo;

    private void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (enemigoObjetivo == null)
        {
            return;
        }

        MoverProyectil();
    }

    private void MoverProyectil()
    {
        direccion = enemigoObjetivo.transform.position - transform.position;
        float angulo = Mathf.Atan2(direccion.x, direccion.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
        rigidbody2.MovePosition(rigidbody2.position + direccion.normalized * velocidad * Time.fixedDeltaTime);
    }

    public void InicializarProyectil(PersonajeAtaque ataque)
    {
        personajeAtaque = ataque;
        enemigoObjetivo = ataque.EnemigoObjetivo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            enemigoObjetivo.GetComponent<EnemigoVida>().RecibirDano(personajeAtaque.ObtenerDamage());
            gameObject.SetActive(false);
        }
    }
}
