using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instancia;
    private AudioSource audioSource;

    [SerializeField] private AudioClip musicaPasillos;
    [SerializeField] private AudioClip musicaMinijuegos;
    [SerializeField] private AudioClip musicaMainMenu;
    [SerializeField] private AudioClip musicaCreditos;
    [SerializeField] private AudioClip musicaQuemados;

    void Awake()
    {
        Debug.Log("Awake de AudioManager");
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // Asegúrate de no destruir este objeto entre escenas
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, destrúyelo
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("🎵 No se encontró el componente AudioSource.");
        }
        else
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Suscribe al evento cuando se cargue una nueva escena
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Desuscribe cuando se desactive
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource != null)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);

            string nombreEscena = scene.name;

            if (nombreEscena == "MainMenu")
            {
                AudioManager.instancia.CambiarMusica("MainMenu");
            }
            else if (nombreEscena == "OriginalYoshy" || nombreEscena == "Pasillos" || nombreEscena == "EscenaPrincipal")
            {
                AudioManager.instancia.CambiarMusica("Pasillos");
            }
            else if (nombreEscena.Contains("Minijuego") || nombreEscena.Contains("Salon"))
            {
                AudioManager.instancia.CambiarMusica("Minijuego");
            }
            else if (nombreEscena == "Creditos")  // Nueva condición para la escena de Créditos
            {
                AudioManager.instancia.CambiarMusica("Creditos");
            }
            else
            {
                Debug.LogWarning("🎵 Escena no reconocida: " + nombreEscena + ". No se cambiará la música.");
            }
        }
        else
        {
            Debug.LogError("🎵 El AudioSource no está disponible en la escena cargada.");
        }
    }



    // Método para cambiar la música según la escena
    public void CambiarMusica(string tipo)
    {
        if (audioSource != null)
        {
            AudioClip nuevaMusica = null;
            Debug.Log("Entrando a CambiarMusica con tipo = " + tipo);

            if (tipo == "Pasillos")
                nuevaMusica = musicaPasillos;
            else if (tipo == "Minijuego")
                nuevaMusica = musicaMinijuegos;
            else if (tipo == "MainMenu")
                nuevaMusica = musicaMainMenu;
            else if (tipo == "Creditos") // Nueva opción para los créditos
                nuevaMusica = musicaCreditos;

            if (nuevaMusica != null)
            {
                if (audioSource.clip != nuevaMusica)
                {
                    audioSource.clip = nuevaMusica;
                    audioSource.Play();
                }
            }
            else
            {
                Debug.LogError("🎵 No se encontró música para: " + tipo);
            }
        }
        else
        {
            Debug.LogError("🎵 El AudioSource no está disponible.");
        }
    }



    // Cuando la transición de audio se termine, reanudar la música
    private void ReanudarMusicaDeFondo()
    {
        if (audioSource != null)
        {
            // Cambia la música de fondo después de la transición
            AudioManager.instancia.CambiarMusica("Pasillos"); // Asegúrate de pasar el argumento necesario
        }
    }

    public AudioSource GetAudioSource()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource no encontrado, podría haberse destruido.");
        }
        return audioSource;
    }
}

