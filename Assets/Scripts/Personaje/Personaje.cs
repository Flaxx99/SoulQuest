using UnityEngine;
using System.Collections;

public class Personaje : MonoBehaviour
{
    public PersonajeVida PersonajeVida { get; private set; }

    private void Awake()
    {
        PersonajeVida = GetComponent<PersonajeVida>();
    }
    public void RestaurarPersonaje()
    {
        PersonajeVida.RestaurarPersonaje();
    }
}
