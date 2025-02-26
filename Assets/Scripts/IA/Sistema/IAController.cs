using UnityEngine;

public class IAController : MonoBehaviour
{
    [Header("Estados")]
    [SerializeField] private IAEstado EstadoInicial;
    [SerializeField] private IAEstado EstadoDefault;

    [Header("Config")]
    [SerializeField] private float rangoDeteccion;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private LayerMask personajeLayerMask;

    [Header("Debug")]
    [SerializeField] private bool mostrarDeteccion;
    public Transform PersonajeReferencia { get; set; }
    public IAEstado EstadoActal { get; set; }
    public EnemigoMovimiento EnemigoMovimiento { get; set; }
    public float RangoDeteccion => rangoDeteccion;
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

    private void OnDrawGizmos()
    {
        if (mostrarDeteccion)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        }
    }
}
