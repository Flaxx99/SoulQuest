using UnityEngine;

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
        audioSource.loop = true; // Asegura que la música no se detenga
        audioSource.playOnAwake = false; // Evita que suene sin control
    }

    void Start()
    {
        CambiarMusica("MainMenu"); // Iniciar con la música de los pasillos
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

        Debug.Log($"🎵 Cambiando música a: {tipo}");

        audioSource.Stop();
        audioSource.clip = nuevaMusica;
        audioSource.Play();
    }

}
