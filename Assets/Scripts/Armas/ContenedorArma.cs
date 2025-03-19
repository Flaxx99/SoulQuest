using UnityEngine;
using UnityEngine.UI;

public class ContenedorArma : Singleton<ContenedorArma>
{
    [SerializeField] private Image armaIcono;
    [SerializeField] private Image armaSkillIcono;

    public ItemArma ArmaEquipada { get; set; }
    public void EquiparArma(ItemArma itemArma)
    {
        if (Inventario.Instance == null)
        {
            Debug.LogError("EquiparArma: Inventario.Instance es null. Asegúrate de que el Inventario está en la escena.");
            return;
        }

        if (Inventario.Instance.Personaje == null)
        {
            Debug.LogError("EquiparArma: Inventario.Instance.Personaje es null.");
            return;
        }

        if (Inventario.Instance.Personaje.PersonajeAtaque == null)
        {
            Debug.LogError("EquiparArma: Inventario.Instance.Personaje.PersonajeAtaque es null.");
            return;
        }

        if (itemArma == null || itemArma.Arma == null)
        {
            Debug.LogError("EquiparArma: itemArma o itemArma.Arma es null.");
            return;
        }

        ArmaEquipada = itemArma;

        if (armaIcono == null)
        {
            Debug.LogError("EquiparArma: armaIcono no está asignado.");
            return;
        }

        armaIcono.sprite = itemArma.Arma.ArmaIcono;
        armaIcono.gameObject.SetActive(true);

        if (itemArma.Arma.Tipo == TipoArma.Magia)
        {
            if (armaSkillIcono == null)
            {
                Debug.LogError("EquiparArma: armaSkillIcono no está asignado.");
                return;
            }

            armaSkillIcono.sprite = itemArma.Arma.IconoSkill;
            armaSkillIcono.gameObject.SetActive(true);
        }

        if (Inventario.Instance == null || Inventario.Instance.Personaje == null || Inventario.Instance.Personaje.PersonajeAtaque == null)
        {
            Debug.LogError("EquiparArma: Inventario.Instance o sus propiedades son null.");
            return;
        }

        Inventario.Instance.Personaje.PersonajeAtaque.EquiparArma(itemArma);
        Debug.Log("ArmaEquipada: " + (ArmaEquipada != null ? "No es null" : "Es null"));
        Debug.Log("armaIcono: " + (armaIcono != null ? "No es null" : "Es null"));
        Debug.Log("Inventario.Instance: " + (Inventario.Instance != null ? "No es null" : "Es null"));

    }

    public void RemoverArma()
    {
        armaIcono.gameObject.SetActive(false);
        armaSkillIcono.gameObject.SetActive(false);
        ArmaEquipada = null;
        Inventario.Instance.Personaje.PersonajeAtaque.RemoverArma();
    }
}
