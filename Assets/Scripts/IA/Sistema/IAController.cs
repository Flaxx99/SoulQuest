using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using RandomUnity = UnityEngine.Random;
using RandomSystem = System.Random;


public enum TiposDeAtaque
{
    Melee,
    Embestida
}
public class IAController : MonoBehaviour
{
    public static Action<float> EventoDamageRealizado;

    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    [Header("Estados")]
    [SerializeField] private IAEstado EstadoInicial;
    [SerializeField] private IAEstado EstadoDefault;

    [Header("Config")]
    [SerializeField] private float rangoDeteccion;
    [SerializeField] private float rangoDeAtaque;
    [SerializeField] private float rangoDeEmbestida;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float velocidadDeEmbestida;
    [SerializeField] private LayerMask personajeLayerMask;

    [Header("Ataque")]
    [SerializeField] private float damage;
    [SerializeField] private float tiempoEntreAtaques; 
    [SerializeField] private TiposDeAtaque tipoAtaque;

    [Header("Debug")]
    [SerializeField] private bool mostrarDeteccion;
    [SerializeField] private bool mostrarRangoAtaque;
    [SerializeField] private bool mostrarRangoEmbestida;

    private float tiempoParaSiguenteAtaque;
    private BoxCollider2D _boxCollider2D;
    public Transform PersonajeReferencia { get; set; }
    public IAEstado EstadoActal { get; set; }
    public EnemigoMovimiento EnemigoMovimiento { get; set; }
    public float RangoDeteccion => rangoDeteccion;
    public float Damage => damage;
    public TiposDeAtaque TipoAtaque => tipoAtaque; 
    public float VelocidadMovimiento => velocidadMovimiento;
    public LayerMask PersonajeLayerMask => personajeLayerMask;
    public float RangoDeAtaqueDeterminado => tipoAtaque == TiposDeAtaque.Embestida ? rangoDeEmbestida : rangoDeAtaque;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
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

    public void AtaqueEmbestida(float cantidad)
    {
        StartCoroutine(IEEmbestida(cantidad));
    }

    private IEnumerator IEEmbestida(float cantidad)
    {
        Vector3 personajePosicion = PersonajeReferencia.position;
        Vector3 posicionInicial = transform.position;
        Vector3 direccionHaciaPersonaje = (personajePosicion - posicionInicial).normalized;
        Vector3 posicionDeAtaque = personajePosicion - direccionHaciaPersonaje * 0.5f;
        _boxCollider2D.enabled = false;

        float trancisionDeAtaque = 0f;
        while (trancisionDeAtaque <= 1f)
        {
            trancisionDeAtaque += Time.deltaTime * velocidadMovimiento;
            float interpolacion = (-Mathf.Pow(trancisionDeAtaque, 2) + trancisionDeAtaque) * 4f;
            transform.position = Vector3.Lerp(posicionInicial, posicionDeAtaque, interpolacion);
            yield return null;
        }

        if (PersonajeReferencia != null)
        {
            AplicarDamageAlPersonaje(cantidad);
        }

        _boxCollider2D.enabled = true;
    }

    public void AplicarDamageAlPersonaje(float cantidad)
    {
        float damagePorRealizar = 0;
        if (RandomUnity.value < stats.PorcentajeBloqueo / 100)
        {
            return;
        }

        damagePorRealizar = Mathf.Max(cantidad - stats.Defensa, 1f);
        PersonajeReferencia.GetComponent<PersonajeVida>().RecibirDano(damagePorRealizar);
        EventoDamageRealizado?.Invoke(damagePorRealizar);
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

        if (mostrarRangoEmbestida)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, rangoDeEmbestida);
        }
    }
}
