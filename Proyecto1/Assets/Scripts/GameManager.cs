using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button quitButton;

    [Header("Pause UI")]
    public GameObject pausePanel;
    public Slider volumeSlider; // <-- El control deslizante del volumen
    private bool isPaused = false;

    private bool isGameOver = false;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Configuración inicial Game Over
        if(gameOverPanel != null) gameOverPanel.SetActive(false);
        if(restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if(quitButton != null) quitButton.onClick.AddListener(QuitGame);
        
        // Configuración inicial Menú de Pausa
        if(pausePanel != null) pausePanel.SetActive(false);

        // Si tenemos un Slider y el AudioManager existe, sincronizamos el valor inicial
        if (volumeSlider != null && AudioManager.Instance != null && AudioManager.Instance.bgmSource != null)
        {
            volumeSlider.value = AudioManager.Instance.bgmSource.volume;
            // Le decimos al Slider que ejecute ChangeVolume cada vez que lo movamos
            volumeSlider.onValueChanged.AddListener(ChangeVolume); 
        }
    }

    void Update()
    {
        // Detectar si presionamos "Escape" (y que no estemos muertos)
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        // Cambiamos el estado (si estaba en false pasa a true, y viceversa)
        isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f; // Congela el juego
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f; // Reanuda el juego
        }
    }

    public void ChangeVolume(float value)
    {
        // Enviamos el valor del Slider al AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(value);
        }
    }

    // --- MÉTODOS DE GAME OVER (Iguales a los que ya tenías) ---
    public void GameOver()
    {
        if(isGameOver) return;
        isGameOver = true;
        if(gameOverPanel != null) gameOverPanel.SetActive(true);
        if(gameOverText != null) gameOverText.text = "Game Over";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Aseguramos que el tiempo fluya al reiniciar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo de juego...");
        Application.Quit();
    }

    // Nuevo botón para usar en el menú de pausa (para volver al juego)
    public void ResumeGame()
    {
        TogglePause();
    }
}