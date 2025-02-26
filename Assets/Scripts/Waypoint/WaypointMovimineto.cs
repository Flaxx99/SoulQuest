using UnityEngine;


public enum DireccionMovimineto
{
    Horizontal,
    Vertical,
}
public class WaypointMovimineto : MonoBehaviour
{

    [SerializeField] private DireccionMovimineto direccion;
    [SerializeField] private float velocidad;

    public Vector3 PuntoPorMoverse => _waypoint.ObtenerPosMovimineto(puntoActualIndex);


    private Waypoint _waypoint;
    private int puntoActualIndex;
    private Vector3 ultimaPosicion;
    void Start()
    {
        puntoActualIndex = 0;
        _waypoint = GetComponent<Waypoint>();

        
    }

    // Update is called once per frame
    void Update()
    {
        MoverPersonaje();
        RotarPersonaje();
        if (ComprobarPuntoActAlcanzado())
        {
            ActualizarIndexMov();   

        }

    }

    private void MoverPersonaje()
    {
        transform.position = Vector3.MoveTowards(transform.position, PuntoPorMoverse, velocidad * Time.deltaTime);
    }

    private bool ComprobarPuntoActAlcanzado()
    {
        float distanciaHaciaPuntoAct = (transform.position - PuntoPorMoverse).magnitude;
        if (distanciaHaciaPuntoAct < 0.1f)
        {
            ultimaPosicion = transform.position;
            return true;
        }

        return false;
    }

    private void ActualizarIndexMov()
    {
        if (puntoActualIndex == _waypoint.Puntos.Length - 1)
        {

            puntoActualIndex = 0;
            
        }
        else
        {
            if (puntoActualIndex < _waypoint.Puntos.Length - 1)
            {

                puntoActualIndex++;

            }

        }
    }

    private void RotarPersonaje()
    {
        if (direccion != DireccionMovimineto.Horizontal)
        {
            return;
        }

        if (PuntoPorMoverse.x > ultimaPosicion.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, -1, -1);
        }
    }
}
