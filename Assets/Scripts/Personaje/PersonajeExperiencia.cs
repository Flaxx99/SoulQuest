using UnityEngine;

public class PersonajeExperiencia : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    [Header("Config")]
    [SerializeField] private int nivelMax;
    [SerializeField] private int expBase;
    [SerializeField] private int valorIncremental;

    private float expActual;
    private float expActualTemp;
    private float expRequeridaSiguienteNivel;

    private void Start()
    {
        stats.Nivel = 1;
        expRequeridaSiguienteNivel = expBase;
        stats.ExpRequeridaSiguienteNivel = expRequeridaSiguienteNivel;
        ActualizarBarraExp();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AnadirExperiencia(2f);
        }
    }
    public void AnadirExperiencia(float expObtenida)
    {
        if (expObtenida > 0f)
        {
            expActual += expObtenida;
            expActualTemp += expObtenida;

            while (expActualTemp >= expRequeridaSiguienteNivel && stats.Nivel < nivelMax)
            {
                ActualizarNivel();
            }

            stats.ExpActual = expActual;
            ActualizarBarraExp();
        }
    }

    private void ActualizarNivel()
    {
        if (stats.Nivel < nivelMax)
        {
            stats.Nivel++;
            expActualTemp = 0f; // Reiniciar experiencia para el nuevo nivel
            expRequeridaSiguienteNivel *= valorIncremental;
            stats.ExpRequeridaSiguienteNivel = expRequeridaSiguienteNivel;
            stats.PuntosDisponibles += 3;

            Debug.Log($"¡Subiste al nivel {stats.Nivel}!");
        }
    }

    private void ActualizarBarraExp()
    {
        UIManager.Instance.ActualizarExpPersonaje(expActualTemp, expRequeridaSiguienteNivel);
    }
    public int ObtenerNivel()
    {
        return Mathf.FloorToInt(stats.Nivel); // Convierte el nivel a entero
    }


}
