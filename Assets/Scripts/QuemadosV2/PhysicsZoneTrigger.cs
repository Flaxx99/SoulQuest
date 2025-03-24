using UnityEngine;

public class PhysicsZoneTrigger : MonoBehaviour
{
    // Aquí decides si el jugador se vuelve Kinematic dentro de la zona
    public bool activarKinematicDentro = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Algo entró al trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = activarKinematicDentro ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
                Debug.Log("Jugador detectado. Rigidbody ahora es: " + rb.bodyType);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = activarKinematicDentro ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
                Debug.LogWarning("Jugador SALIÓ de la zona. Rigidbody ahora es: " + rb.bodyType);
            }
        }
    }
}