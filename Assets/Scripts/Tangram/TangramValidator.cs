using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class PosicionObjetivo
{
    public Transform referencia; // Objeto que representa la posición correcta
    public List<TangramPiece> piezasValidas; // Lista de piezas que pueden ocupar esta posición
}

public class TangramValidator : MonoBehaviour
{
    [SerializeField] private List<PosicionObjetivo> posicionesObjetivo = new List<PosicionObjetivo>(); // Evita que se sobrescriba
    public float toleranciaPosicion = 0.7f; // Margen de error en posición (ajustado para mayor precisión)
    public float toleranciaRotacion = 15f;  // Margen de error en rotación (grados)
    public TextMeshProUGUI mensajeTexto; // Texto de UI para mensajes

    public static TangramValidator Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Se encontró un segundo Validator y fue destruido.");
            Destroy(gameObject);
            return;
        }

        if (posicionesObjetivo == null || posicionesObjetivo.Count == 0)
        {
            Debug.LogError("ERROR en Awake(): La lista de posiciones objetivo está vacía.");
        }
        else
        {
            Debug.Log($"En Awake(): Posiciones objetivo correctamente asignadas. Total: {posicionesObjetivo.Count}");
        }
    }


    void Start()
    {
        if (posicionesObjetivo == null || posicionesObjetivo.Count == 0)
        {
            Debug.LogError("ERROR en Start(): La lista de posiciones objetivo está vacía. Verifica que los elementos están asignados en el Inspector.");
        }
        else
        {
            Debug.Log($"En Start(): Posiciones objetivo correctamente asignadas. Total: {posicionesObjetivo.Count}");
        }
        Debug.Log($"En Start(): Lista actual de posiciones objetivo: {posicionesObjetivo.Count}");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Si se presiona "Enter"
        {
            Debug.Log("Enter presionado: Ejecutando CheckSolution()");
            CheckSolution();
        }
    }


    public void CheckSolution()
    {
        Debug.Log("Iniciando validación del Tangram...");

        Debug.Log($"PosicionesObjetivo tiene {posicionesObjetivo.Count} elementos.");

        if (posicionesObjetivo == null || posicionesObjetivo.Count == 0)
        {
            Debug.LogError("ERROR: No hay posiciones objetivo asignadas en el Validator. Asegúrate de asignarlas en el Inspector.");
            return;
        }

        if (posicionesObjetivo.Count == 0)
        {
            Debug.LogError("ERROR: No hay posiciones objetivo asignadas en el Validator. Asegúrate de asignarlas en el Inspector.");
            return;
        }

        int piezasCorrectas = 0; // Contador de piezas bien colocadas

        foreach (PosicionObjetivo posicion in posicionesObjetivo)
        {
            bool posicionCorrecta = false; // Cada posición debe validarse correctamente

            foreach (TangramPiece pieza in posicion.piezasValidas)
            {
                float distancia = Vector2.Distance(pieza.transform.position, posicion.referencia.position);
                float diferenciaRotacion = Mathf.Abs(pieza.transform.eulerAngles.z - posicion.referencia.eulerAngles.z);

                Debug.Log($"Verificando {pieza.name}");
                Debug.Log($"Objetivo: Posición {posicion.referencia.position}, Rotación {posicion.referencia.eulerAngles.z}");
                Debug.Log($"Actual: Posición {pieza.transform.position}, Rotación {pieza.transform.eulerAngles.z}");
                Debug.Log($"Distancia: {distancia}, Diferencia Rotación: {diferenciaRotacion}");

                // Verificar si la pieza está dentro del margen de error
                if (distancia <= toleranciaPosicion && diferenciaRotacion <= toleranciaRotacion)
                {
                    Debug.Log($"{pieza.name} está correctamente posicionada.");
                    posicionCorrecta = true;
                    piezasCorrectas++; // Aumenta el contador de piezas correctas
                    break; // No es necesario revisar más piezas para esta posición
                }
            }

            if (!posicionCorrecta)
            {
                Debug.LogWarning($"{posicion.referencia.name} no tiene ninguna pieza bien colocada.");
                MostrarMensajeEnPantalla("Algunas piezas no están bien colocadas", Color.red);
                return; // Si una pieza está mal, se detiene la validación
            }
        }

        // Ahora mostramos cuántas piezas fueron correctas antes de completar el tangram
        Debug.Log($"¡Tangram completado correctamente! Piezas correctas: {piezasCorrectas}/{posicionesObjetivo.Count}");

        if (piezasCorrectas == posicionesObjetivo.Count) // Asegurar que TODAS las piezas estén bien antes de completar
        {
            MostrarMensajeEnPantalla("¡Tangram completado!", Color.green);
            Invoke("CargarEscenaVictoria", 2f);
        }
        else
        {
            Debug.LogWarning("Todavía hay piezas mal colocadas.");
            MostrarMensajeEnPantalla("Algunas piezas aún no están bien colocadas.", Color.red);
        }
    }




    public void MostrarMensajeEnPantalla(string mensaje, Color color)
    {
        if (mensajeTexto != null)
        {
            mensajeTexto.text = mensaje;
            mensajeTexto.color = color;
            mensajeTexto.alpha = 1;
            CancelInvoke("OcultarMensaje");
            Invoke("OcultarMensaje", 2f);
        }
    }

    private void OcultarMensaje()
    {
        if (mensajeTexto != null)
        {
            mensajeTexto.alpha = 0;
        }
    }

    private void CargarEscenaVictoria()
    {
        Debug.Log("Cargando escena de victoria...");
        SceneManager.LoadScene("VictoryScene");
    }
}
