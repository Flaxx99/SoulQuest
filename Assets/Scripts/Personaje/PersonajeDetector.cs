using System;
using UnityEngine;

public class PersonajeDetector : MonoBehaviour
{
    public static Action<EnemigoInteraccion> EventoEnemigoDetectado;
    public static Action EventoEnemigoPerdido;
    public EnemigoInteraccion EnemigoDetectado { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            // Intentamos obtener el componente EnemigoInteraccion
            EnemigoDetectado = collision.GetComponent<EnemigoInteraccion>();

            // Verificamos si el componente EnemigoInteraccion no es nulo
            if (EnemigoDetectado != null)
            {
                // Intentamos obtener el componente EnemigoVida de EnemigoDetectado
                EnemigoVida enemigoVida = EnemigoDetectado.GetComponent<EnemigoVida>();

                // Verificamos si EnemigoVida no es nulo antes de acceder a su propiedad Salud
                if (enemigoVida != null && enemigoVida.Salud > 0)
                {
                    // Invocamos el evento si la salud es mayor que 0
                    EventoEnemigoDetectado?.Invoke(EnemigoDetectado);
                }
                else
                {
                    Debug.LogWarning("El enemigo detectado no tiene vida o tiene 0 de salud.");
                }
            }
            else
            {
                // Si EnemigoDetectado es nulo, mostramos un mensaje de advertencia
                Debug.LogWarning("El objeto con el que colisionamos no tiene el componente EnemigoInteraccion.");
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            EventoEnemigoPerdido?.Invoke();
        }
    }
}
