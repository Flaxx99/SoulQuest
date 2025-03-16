using UnityEngine;

public class AlinearPosiciones : MonoBehaviour
{
    void Start()
    {
        foreach (Transform posicion in GameObject.Find("FiguraObjetivo1").transform)
        {
            Transform validatorPos = GameObject.Find($"Validator/{posicion.name}_posicion")?.transform;

            if (validatorPos != null)
            {
                // Ajustar posición globalmente
                posicion.position = validatorPos.position;
                Debug.Log($"📌 {posicion.name} alineada con {validatorPos.name}");
            }
            else
            {
                Debug.LogWarning($"⚠️ No se encontró la referencia en Validator para {posicion.name}");
            }
        }
    }
}
