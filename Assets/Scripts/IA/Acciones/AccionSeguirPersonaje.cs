using UnityEngine;

public class AccionSeguirPersonaje : IAAccion
{
    public override void Ejecutar(IAController controller)
    {
        
    }

    private void SeguirPersonaje(IAController controller) 
    {
        if (controller.PersonajeReferencia == null)
        {
            return;
        }

        Vector3 dirHaciaPersonaje = 
            (controller.PersonajeReferencia.position - controller.transform.position).normalized;
        float distancia = dirHaciaPersonaje.magnitude;

        if (distancia >= 1.15f)
        {
            controller.transform.Translate(dirHaciaPersonaje * controller.VelocidadMovimiento * Time.deltaTime);
        }
    }
}
