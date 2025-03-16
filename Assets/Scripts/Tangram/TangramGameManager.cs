using UnityEngine;
using System.Collections.Generic;

public class TangramGameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> piezasPosibles = new List<GameObject>();
    public List<GameObject> piezas;

    void Start()
    {
        for (int i = 0; i < piezas.Count; i++)
        {
            if (piezas[i] == null)
            {
                Debug.LogError($"⚠️ Error: La pieza en el índice {i} es NULL en TangramGameManager.");
                continue;
            }

            TangramPiece pieza = piezas[i].GetComponent<TangramPiece>();
            if (pieza == null)
            {
                Debug.LogError($"⚠️ Error: La pieza {piezas[i].name} no tiene el componente TangramPiece.");
                continue;
            }

            pieza.HacerAlgo(); // Solo se ejecuta si la pieza es válida
        }
    }

}
