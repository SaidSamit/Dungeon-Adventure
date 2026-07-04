using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Configuración de Audio")]
    // Asignar el componente que emite el sonido en el juego
    public AudioSource bgmSource;

    private void Awake()
    {
        // Implementar el patrón Singleton para asegurar una única instancia global
        if (Instance == null)
        {
            Instance = this;
            // Evitar que la música se destruya al cambiar entre escenas (ej. del Menú al Juego)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destruir copias duplicadas si se vuelve a cargar la escena inicial
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        // Validar si el clip actual es diferente al nuevo para evitar reiniciar la misma pista
        if (bgmSource.clip == clip) return;

        // Asignar la nueva pista musical y comenzar la reproducción
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBackgroundMusic()
    {
        // Detener la reproducción de la música actual
        bgmSource.Stop();
    }

    public void SetVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }
}