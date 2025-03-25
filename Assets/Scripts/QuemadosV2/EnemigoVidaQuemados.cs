using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemigoVidaQuemados : VidaBase
{
    [SerializeField] private EnemigoBarraVida barraVidaPrefab;
    [SerializeField] private Transform barraVidaPosicion;
    [SerializeField] private MinijuegoUIController minijuegoUIController; // Referencia al controlador de UI del minijuego

    private EnemigoBarraVida _enemigoBarraVidaCreada;

    private void Awake()
    {
        // Verificamos que minijuegoUIController no esté null
        if (minijuegoUIController == null)
        {
            minijuegoUIController = FindAnyObjectByType<MinijuegoUIController>();
        }
    }

    protected override void Start()
    {
        base.Start();
        CrearBarraVida();
    }

    private void CrearBarraVida()
    {
        _enemigoBarraVidaCreada = Instantiate(barraVidaPrefab, barraVidaPosicion);
        ActualizarBarraVida(Salud, saludMax);
    }

    protected override void ActualizarBarraVida(float vidaActual, float vidaMax)
    {
        _enemigoBarraVidaCreada.ModificarSalud(vidaActual, vidaMax);
    }

    // Este método se llama cuando la salud del enemigo llega a 0
    protected override void PersonajeDerrotado()
    {
        // Aquí no llamamos a PersonajeDerrotado para la victoria, sino que se maneja con DesactivarEnemigo
        DesactivarEnemigo();
    }

    // Método para desactivar el enemigo y mostrar el panel de victoria
    private void DesactivarEnemigo()
    {
        if (_enemigoBarraVidaCreada != null)
        {
            _enemigoBarraVidaCreada.gameObject.SetActive(false);  // Desactiva la barra de vida del enemigo
        }

        // Llamamos a MinijuegoUIController para mostrar el panel de victoria
        if (minijuegoUIController != null)
        {
            minijuegoUIController.MostrarPanelVictoria(true);  // Muestra el panel de victoria
        }

        // Desactivar el enemigo
        gameObject.SetActive(false);
    }
}
