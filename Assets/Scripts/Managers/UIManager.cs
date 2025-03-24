using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
    [Header("Stats")]
    [SerializeField] private PersonajeStats stats;

    [Header("Paneles")]
    [SerializeField] private GameObject panelStats;
    [SerializeField] private GameObject panelInventario;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject PlayerUI; // Para ocultar el HUD cuando el jugador muera

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Barra")]
    [SerializeField] private Image vidaPlayer;
    [SerializeField] private Image manaPlayer;
    [SerializeField] private Image expPlayer;

    [Header("Texto")]
    [SerializeField] private TextMeshProUGUI vidaTMP;
    [SerializeField] private TextMeshProUGUI manaTMP;
    [SerializeField] private TextMeshProUGUI expTMP;
    [SerializeField] private TextMeshProUGUI nivelTMP;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI statDanoTMP;
    [SerializeField] private TextMeshProUGUI statDefensaTMP;
    [SerializeField] private TextMeshProUGUI statCriticoTMP;
    [SerializeField] private TextMeshProUGUI statBloqueoTMP;
    [SerializeField] private TextMeshProUGUI statVelocidadTMP;
    [SerializeField] private TextMeshProUGUI statNivelTMP;
    [SerializeField] private TextMeshProUGUI statExpTMP;
    [SerializeField] private TextMeshProUGUI statExpRequeridaTMP;
    [SerializeField] private TextMeshProUGUI atributoFuerzaTMP;
    [SerializeField] private TextMeshProUGUI atributoInteligenciaTMP;
    [SerializeField] private TextMeshProUGUI atributoDestrezaTMP;
    [SerializeField] private TextMeshProUGUI atributosDisponiblesTMP;

    [Header("Mensajes UI")]
    [SerializeField] private GameObject mensajePanel;
    [SerializeField] private TextMeshProUGUI mensajeTexto;


    private float vidaActual;
    private float vidaMax;
    private float manaActual;
    private float manaMax;
    private float expActual;
    private float expRequeridaNuevoNivel;
    private PersonajeVida personajeVida;

    [Header("Paneles adicionales de UI")]
    [SerializeField] private GameObject panelBotones;
    [SerializeField] private GameObject panelArmaEquipada;
    [SerializeField] private GameObject panelInstruccion;

    [Header("Referencias")]
    [SerializeField] private CanvasGroup panelCanvasGroup;

    // Propiedades para acceder a la vida del jugador
    public float VidaActual => vidaActual;
    public float VidaMax => vidaMax;


    // Update is called once per frame
    private void LateUpdate()
    {
        if (panelGameOver.activeSelf) return; // Si el panel Game Over est? activo, no actualizar UI
        ActualizarUIPersonaje();
        ActualizarPanelStats();
    }

    private void Start()
    {
        personajeVida = Object.FindFirstObjectByType<PersonajeVida>(); // Nueva forma recomendada
    }


    protected override void Awake()
    {
        base.Awake(); // Llama al Awake() de la clase base Singleton
        //DontDestroyOnLoad(gameObject);
    }
    private void ActualizarUIPersonaje()
    {
        if (vidaPlayer == null || manaPlayer == null || expPlayer == null)
        {
            Debug.LogWarning("Una de las barras de UI fue destruida o no est? asignada. Evitando actualizaci?n.");
            return; // Salir del m?todo si falta alguna barra
        }

        // Verificar que el objeto no haya sido destruido
        if (!vidaPlayer.gameObject.activeInHierarchy || !manaPlayer.gameObject.activeInHierarchy || !expPlayer.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Una de las barras de UI fue destruida. No se actualizar?.");
            return;
        }

        vidaPlayer.fillAmount = Mathf.Lerp(vidaPlayer.fillAmount, vidaActual / vidaMax, 10f * Time.unscaledDeltaTime);
        manaPlayer.fillAmount = Mathf.Lerp(manaPlayer.fillAmount, manaActual / manaMax, 10f * Time.unscaledDeltaTime);
        expPlayer.fillAmount = Mathf.Lerp(expPlayer.fillAmount, expActual / expRequeridaNuevoNivel, 10f * Time.unscaledDeltaTime);

        vidaTMP.text = $"{vidaActual}/{vidaMax}";
        manaTMP.text = $"{manaActual}/{manaMax}";
        expTMP.text = $"{((expActual / expRequeridaNuevoNivel) * 100):F2}%";
        nivelTMP.text = $"Nivel {stats.Nivel}";
    }

    private void ActualizarPanelStats()
    {
        if (panelStats.activeSelf == false)
        {
            return;
        }
        statDanoTMP.text = stats.Dano.ToString();
        statDefensaTMP.text = stats.Defensa.ToString();
        statCriticoTMP.text = $"{stats.PorcentajeCritico}%";
        statBloqueoTMP.text = $"{stats.PorcentajeBloqueo}%";
        statVelocidadTMP.text = stats.Velocidad.ToString();
        statNivelTMP.text = stats.Nivel.ToString();
        statExpTMP.text = stats.ExpActual.ToString();
        statExpRequeridaTMP.text = stats.ExpRequeridaSiguienteNivel.ToString();

        atributoFuerzaTMP.text = stats.Fuerza.ToString();
        atributoInteligenciaTMP.text = stats.Inteligencia.ToString();
        atributoDestrezaTMP.text = stats.Destreza.ToString();
        atributosDisponiblesTMP.text = $"Puntos: {stats.PuntosDisponibles}";
    }

    public void ActualizarVidaPersonaje(float pVidaActual, float pVidaMax)
    {
        vidaActual = pVidaActual;
        vidaMax = pVidaMax;
    }
    public void ActualizarManaPersonaje(float pManaActual, float pManaMax)
    {
        manaActual = pManaActual;
        manaMax = pManaMax;
    }
    public void ActualizarExpPersonaje(float pExpActual, float pExpRequerida)
    {
        expActual = pExpActual;
        expRequeridaNuevoNivel = pExpRequerida;

        expPlayer.fillAmount = expActual / expRequeridaNuevoNivel;
        expTMP.text = $"{((expActual / expRequeridaNuevoNivel) * 100):F2}%";
        // nivelTMP.text = $"Nivel {Resources.Load<PersonajeStats>("Stats").Nivel}"; // Asegurar que se muestre correctamente
        PersonajeExperiencia personajeExp = Object.FindFirstObjectByType<PersonajeExperiencia>();
        if (personajeExp != null)
        {
            nivelTMP.text = $"Nivel {personajeExp.ObtenerNivel()}";
        }
    }

    #region Paneles

    public void AbrirCerrarPanelStats()
    {
        panelStats.SetActive(!panelStats.activeSelf);
    }

    public void AbrirCerrarPanelInventario()
    {
        panelInventario.SetActive(!panelInventario.activeSelf);
    }
    private bool armaEquipadaWasActiveBefore;

    public void AbrirCerrarPanelInstruccion()
    {
        Debug.Log("Botón pulsado: intentando abrir/cerrar panelInstruccion.");

        bool estabaActivo = panelInstruccion.activeSelf;
        panelInstruccion.SetActive(!estabaActivo);

        // Si acabamos de activar el panel de instrucciones...
        if (panelInstruccion.activeSelf)
        {
            // Guardamos el estado actual de panelArmaEquipada
            armaEquipadaWasActiveBefore = panelArmaEquipada.activeSelf;

            // Lo desactivamos
            panelArmaEquipada.SetActive(false);

            Debug.Log("Desactivando panelArmaEquipada.");
        }
        else
        {
            // Si acabamos de cerrar el panel de instrucciones,
            // restauramos el estado previo de panelArmaEquipada
            panelArmaEquipada.SetActive(armaEquipadaWasActiveBefore);
            Debug.Log("Restaurando estado previo de panelArmaEquipada.");
        }

        Debug.Log($"panelInstruccion: {panelInstruccion.activeSelf}, " +
                  $"panelArmaEquipada: {panelArmaEquipada.activeSelf}");
    }


    #endregion

    public void MostrarPantallaGameOver()
    {
        Debug.Log("Mostrando pantalla de Game Over");

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true); // Activar pantalla de Game Over
        }

        // Ocultar UI del jugador SOLO si su salud es 0
        if (PlayerUI != null && personajeVida.Salud <= 0)
        {
            Debug.Log("Ocultando PlayerUI");
            PlayerUI.SetActive(false);
        }

        // 🔹 Ocultar panel de botones
        if (panelBotones != null)
        {
            Debug.Log("Ocultando PanelBotones");
            panelBotones.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠ panelBotones no asignado en UIManager");
        }

        // 🔹 Ocultar panel de arma equipada
        if (panelArmaEquipada != null)
        {
            Debug.Log("Ocultando PanelArmaEquipada");
            panelArmaEquipada.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠ panelArmaEquipada no asignado en UIManager");
        }
    }


    public void ReiniciarJuego()
    {
        Debug.Log("🔄 Reiniciando el juego...");

        // 🔥 Asegurar que la música de los pasillos suene después de un reinicio
        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.CambiarMusica("Pasillos");
        }

        // 🚀 Solo recargamos la escena, sin cambiar el estado del `GameManager`
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void SalirAlMenu()
    {
        Debug.Log("🔄 Saliendo al Menú Principal...");

        // 🔥 Asegurar que la música del menú suene al regresar
        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.CambiarMusica("MainMenu");
        }

        SceneManager.LoadScene("MainMenu"); // Carga la escena del Menú Principal
    }
    public void MostrarMensaje(string mensaje)
    {
        if (mensajeTexto != null && mensajePanel != null)
        {
            mensajeTexto.text = mensaje;
            mensajePanel.SetActive(true);

            CancelInvoke("EsconderMensaje");
            Invoke("EsconderMensaje", 2.5f); // El mensaje desaparece en 2.5 segundos
        }
        else
        {
            Debug.LogWarning("UIManager: mensajeTexto o mensajePanel no están asignados en el Inspector.");
        }
    }

    private void EsconderMensaje()
    {
        mensajePanel.SetActive(false);
    }

    public float fadeDuration = 2f;  // Duración de la transición de fade

    // Método para realizar el fade (desvanecimiento)
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    // Coroutine para manejar el fade out
    private IEnumerator FadeOutCoroutine()
    {
        float time = 0;

        // Asegura que el CanvasGroup está visible al iniciar
        panelCanvasGroup.alpha = 1f;

        // Realiza el desvanecimiento a un alpha de 0
        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            panelCanvasGroup.alpha = alpha;
            time += Time.deltaTime;
            yield return null;
        }

        panelCanvasGroup.alpha = 0f; // Asegura que al final sea completamente transparente
    }

    // Si necesitas un FadeIn
    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float time = 0;

        // Asegura que el CanvasGroup comienza invisible
        panelCanvasGroup.alpha = 0f;

        // Realiza el desvanecimiento a un alpha de 1
        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            panelCanvasGroup.alpha = alpha;
            time += Time.deltaTime;
            yield return null;
        }

        panelCanvasGroup.alpha = 1f; // Asegura que al final sea completamente visible
    }

}
