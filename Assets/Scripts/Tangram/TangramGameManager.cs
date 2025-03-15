using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controla la lógica del minijuego de Tangram.
/// </summary>
public class TangramGameManager : MonoBehaviour
{
    // Lista de figuras objetivo disponibles en el juego
    [SerializeField] private List<GameObject> figurasPosibles = new List<GameObject>();

    // Lista de piezas del Tangram disponibles para usar
    [SerializeField] private List<GameObject> piezasPosibles = new List<GameObject>();

    void Start()
    {
        // Verificar si hay figuras posibles antes de instanciar
        if (figurasPosibles != null && figurasPosibles.Count > 0)
        {
            int index = Random.Range(0, figurasPosibles.Count);
            GameObject figura = Instantiate(figurasPosibles[index], new Vector3(4, 0, 0), Quaternion.identity);
            figura.transform.SetParent(GameObject.Find("TangramCanvas").transform, false);
        }
        else
        {
            Debug.LogError("No hay figuras disponibles en figurasPosibles. Asigna figuras en el Inspector.");
        }

        // Verificar si hay piezas posibles antes de instanciar
        if (piezasPosibles != null && piezasPosibles.Count > 0)
        {
            foreach (GameObject pieza in piezasPosibles)
            {
                GameObject nuevaPieza = Instantiate(pieza, new Vector3(Random.Range(-5, -3), Random.Range(-2, 2), 0), Quaternion.identity);
                nuevaPieza.transform.SetParent(GameObject.Find("TangramCanvas").transform, false);
            }
        }
        else
        {
            Debug.LogError("No hay piezas disponibles en piezasPosibles. Asigna piezas en el Inspector.");
        }
    }
}
