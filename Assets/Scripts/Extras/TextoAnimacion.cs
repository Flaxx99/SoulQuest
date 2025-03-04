using TMPro;
using UnityEngine;

public class TextoAnimacion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageTexto;
    public void EstablecerTexto(float cantidad)
    {
        damageTexto.text = cantidad.ToString();
    }
}
