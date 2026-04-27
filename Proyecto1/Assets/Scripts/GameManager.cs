using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button quitButton;

    private bool isGameOver = false;

    public void Awake()
    {
        if(Instance == null)
        {Instance = this;
        }
        else
        {Destroy(gameObject);
        }
    }

    void Start()
    {
        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if(restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        if(quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        
    }

    public void GameOver()
    {
        if(isGameOver) return;

        isGameOver = true;

        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        if(gameOverText != null)
        {
            gameOverText.text = "Game Over";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Debug.Log("Saliendo de juego...");
        Application.Quit();
    }


        void Update()
    {
        
    }


}
