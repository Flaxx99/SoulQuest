using UnityEngine;
using UnityEngine.SceneManagement;

public class MinijuegoQuemados : MonoBehaviour
{
    public GameObject enemigo;  // Arrastra el enemigo aquí en el Inspector
    public GameObject pelota;   // La pelota del minijuego
    public Transform posicionPelota; // Donde aparecerá la pelota al empezar
    public GameObject barreras; // Colliders invisibles para limitar la zona

    private bool minijuegoIniciado = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!minijuegoIniciado && other.CompareTag("Jugador"))
        {
            minijuegoIniciado = true;
            Debug.Log("¡Minijuego de quemados iniciado!");

            // Aparece el enemigo
            enemigo.SetActive(true);

            // Mueve la pelota al centro de la cancha
            pelota.transform.position = posicionPelota.position;
            pelota.SetActive(true);

            // Activa las barreras invisibles
            barreras.SetActive(true);

            // Eliminar la "PelotaInicial" (este mismo objeto)
            Destroy(gameObject);
        }
    }
}
