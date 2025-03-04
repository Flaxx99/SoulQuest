using UnityEngine;

[CreateAssetMenu(menuName = "IA/Acciones/Atacar Personaje")]
public class AccionAtacarPersonaje : IAAccion
{
    public override void Ejecutar(IAController controller)
    {
        Atacar(controller);
    }

    private void Atacar(IAController controller)
    {
        if (controller.PersonajeReferencia == null)
        {
            return;
        }

        if (controller.EsTiempoDeAtacar() == false)
        {
            return;
        }

        if (controller.PersonajeEnRangoDeAtaque(controller.RangoDeAtaqueDeterminado))
        {
            //Atacarlo
            if (controller.TipoAtaque == TiposDeAtaque.Embestida)
            {
                controller.AtaqueEmbestida(controller.Damage);
            }
            else
            {
                controller.AtaqueMelee(controller.Damage);
            }
            
            controller.ActualizarTiempoEntreAtaques();
        }
    }
}
