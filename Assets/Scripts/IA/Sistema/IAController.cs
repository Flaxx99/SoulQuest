using UnityEngine;
using UnityEngine.UIElements.Experimental;

public enum TiposDeAtaque
{
    Melee,
    Embestida
}
public class IAController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    [Header("Estados")]
    [SerializeField] private IAEstado EstadoInicial;
    [SerializeField] private IAEstado EstadoDefault;

    [Header("Config")]
    [SerializeField] private float rangoDeteccion;
    [SerializeField] private float rangoDeAtaque;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private LayerMask personajeLayerMask;

    [Header("Ataque")]
    [SerializeField] private float damage;
    [SerializeField] private float tiempoEntreAtaques; 
    [SerializeField] private TiposDeAtaque tipoAtaque;

    [Header("Debug")]
    [SerializeField] private bool mostrarDeteccion;
    [SerializeField] private bool mostrarRangoAtaque;

    private float tiempoParaSiguenteAtaque;
    public Transform PersonajeReferencia { get; set; }
    public IAEstado EstadoActal { get; set; }
    public EnemigoMovimiento EnemigoMovimiento { get; set; }
    public float RangoDeteccion => rangoDeteccion;
    public float RangoDeAtaque => rangoDeAtaque;
    public float Damage => damage;
    public TiposDeAtaque TipoAtaque => tipoAtaque; 
    public float VelocidadMovimiento => velocidadMovimiento;
    public LayerMask PersonajeLayerMask => personajeLayerMask;

    private void Start()
    {
        EstadoActal = EstadoInicial;
        EnemigoMovimiento = GetComponent<EnemigoMovimiento>();
    }

    private void Update()
    {
        EstadoActal.EjecutarEstado(this);
    }

    public void CambiarEstado(IAEstado NuevoEstado)
    {
        if (NuevoEstado != EstadoDefault)
        {
            EstadoActal = NuevoEstado;
        }
    }

    public void AtaqueMelee(float cantidad)
    {
        if (PersonajeReferencia != null)
        {
            AplicarDamageAlPersonaje(cantidad); 
        }
    }

    public void AplicarDamageAlPersonaje(float cantidad)
    {
        float damagePorRealizar = 0;
        if (Random.value < stats.PorcentajeBloqueo / 100)
        {
            return;
        }

        damagePorRealizar = Mathf.Max(cantidad - stats.Defensa, 1f);
        PersonajeReferencia.GetComponent<PersonajeVida>().RecibirDano(damagePorRealizar);
    }

    public bool PersonajeEnRangoDeAtaque(float rango)
    {
        float distanciaHaciaPersonaje = (PersonajeReferencia.position - transform.position).sqrMagnitude;
        if (distanciaHaciaPersonaje < Mathf.Pow(rango, 2))
        {
            return true;
        }

        return false;
    }

    public bool EsTiempoDeAtacar()
    {
        if (Time.time > tiempoParaSiguenteAtaque)
        {
            return true;
        }

        return false;
    }

    public void ActualizarTiempoEntreAtaques()
    {
        tiempoParaSiguenteAtaque = Time.time + tiempoEntreAtaques;
    }
    private void OnDrawGizmos()
    {
        if (mostrarDeteccion)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        }

        if (mostrarRangoAtaque)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, rangoDeAtaque);
        } 
    }
}
