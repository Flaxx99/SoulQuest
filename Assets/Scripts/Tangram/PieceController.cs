using UnityEngine;

/// <summary>
/// Controla el movimiento y rotación de las piezas del Tangram.
/// </summary>
public class PieceController : MonoBehaviour
{
    //public float velocidadMovimiento = 1f;
    public float anguloRotacion = 2f;

    void Update()
    {
        // Movimiento con flechas
        //float moveX = Input.GetAxis("Horizontal") * velocidadMovimiento * Time.deltaTime;
        //float moveY = Input.GetAxis("Vertical") * velocidadMovimiento * Time.deltaTime;
        //transform.position += new Vector3(moveX, moveY, 0);

        // Rotación con teclas J y L
        if (Input.GetKeyDown(KeyCode.J))
        {
            transform.Rotate(Vector3.forward, anguloRotacion);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            transform.Rotate(Vector3.forward, -anguloRotacion);
        }
    }
}
