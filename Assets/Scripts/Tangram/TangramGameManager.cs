using UnityEngine;
using System.Collections.Generic;

public class TangramGameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> piezasPosibles = new List<GameObject>();

    void Start()
    {
        GameObject tangramPiecesParent = GameObject.Find("TangramPieces"); // Buscamos el contenedor de piezas

        if (piezasPosibles != null && piezasPosibles.Count > 0)
        {
            foreach (GameObject pieza in piezasPosibles)
            {
                // Verificamos si ya existe una pieza con el mismo nombre dentro de TangramPieces
                if (tangramPiecesParent.transform.Find(pieza.name) == null)
                {
                    GameObject nuevaPieza = Instantiate(pieza, new Vector3(Random.Range(-5, -3), Random.Range(-2, 2), 0), Quaternion.identity);
                    nuevaPieza.transform.SetParent(tangramPiecesParent.transform, false);
                }
                else
                {
                    Debug.LogWarning($"La pieza {pieza.name} ya existe en TangramPieces y no será duplicada.");
                }
            }
        }
        else
        {
            Debug.LogError("No hay piezas disponibles en piezasPosibles.");
        }
    }
}
