using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public TMP_Text healthText;

    private void Start()
    {
        // Vida actual es la vida máxima definida en el StatManager
        currentHealth = StatManager.Instance.maxHealth;
        UpdateUI();
    }

    public void changeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > StatManager.Instance.maxHealth)
        {
            currentHealth = StatManager.Instance.maxHealth;
        }

        UpdateUI();

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth + "/" + StatManager.Instance.maxHealth;
        }
    }
}