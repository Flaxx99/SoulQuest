using UnityEngine;

public class PelotaQuemados : MonoBehaviour
{
    private bool puedeColisionar = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 0f; // Asegurar que no tenga gravedad
        rb.linearVelocity = new Vector2(0, 5f); // Velocidad controlada

        // 🔹 Esperar antes de activar colisiones
        Invoke("ActivarColisiones", 0.2f);

        Debug.Log("🎾 Pelota creada en posición: " + transform.position);
        Debug.Log("⚡ Velocidad aplicada: " + rb.linearVelocity);
    }

    private void ActivarColisiones()
    {
        puedeColisionar = true;
        Debug.Log("✅ Colisiones activadas para la pelota.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!puedeColisionar) return; // Evita colisión prematura

        Debug.Log("🎯 Pelota tocó: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("💥 Pelota impactó al jugador");
            other.GetComponent<JugadorQuemados>().RecibirGolpe();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemigo"))
        {
            Debug.Log("💥 Pelota impactó al enemigo");
            other.GetComponent<EnemigoQuemados>().RecibirGolpe();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Limite"))
        {
            Debug.Log("🚫 Pelota eliminada por salir del campo");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("🟡 Pelota colisionó con: " + other.gameObject.name);
        }
    }
}
