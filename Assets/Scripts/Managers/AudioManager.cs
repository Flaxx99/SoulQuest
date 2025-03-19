using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instancia;
    private AudioSource audioSource;

    [SerializeField] private AudioClip musicaPasillos;
    [SerializeField] private AudioClip musicaMinijuegos;
    [SerializeField] private AudioClip musicaMainMenu;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        CambiarMusicaSegunEscena();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 🔹 Método que detecta cambios de escena y cambia la música correctamente
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CambiarMusicaSegunEscena();
    }

    public void CambiarMusicaSegunEscena()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        Debug.Log($"🎵 Detectando escena: {escenaActual}");

        if (escenaActual == "MainMenu")
        {
            CambiarMusica("MainMenu");
        }
        else if (escenaActual == "Pasillos" || escenaActual == "EscenaPrincipal")
        {
            CambiarMusica("Pasillos");
        }
        else if (escenaActual.Contains("Minijuego"))
        {
            CambiarMusica("Minijuego");
        }
    }

    public void CambiarMusica(string tipo)
    {
        if (audioSource == null) return;

        AudioClip nuevaMusica = null;

        if (tipo == "Pasillos") nuevaMusica = musicaPasillos;
        else if (tipo == "Minijuego") nuevaMusica = musicaMinijuegos;
        else if (tipo == "MainMenu") nuevaMusica = musicaMainMenu;

        if (nuevaMusica == null)
        {
            Debug.LogError($"❌ No se encontró música para {tipo}");
            return;
        }

        if (audioSource.clip == nuevaMusica) return; // Evitar repetir la misma música

        Debug.Log($"🎵 Cambiando música a: {tipo}");

        audioSource.Stop();
        audioSource.clip = nuevaMusica;
        audioSource.Play();
    }
}
