using UnityEngine;

// Centralizar únicamente los datos persistentes del jugador
public class StatManager : MonoBehaviour
{
    public static StatManager Instance;

    [Header("Atributos Maestros")]
    public int maxHealth = 10;
    public float moveSpeed = 5f;
    public int attackDamage = 1;

    private void Awake()
    {
        // Implementar patrón Singleton para persistencia pura de datos
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- Métodos para incrementar atributos (Solo Datos) ---

    public void IncreaseMaxHealth()
    {
        maxHealth += 2;
        // Localizar al jugador para aplicar curación al subir de nivel
        GameObject.FindWithTag("Player")?.GetComponent<PlayerHealth>().changeHealth(2);
    }

    public void IncreaseSpeed()
    {
        moveSpeed += 0.5f;
    }

    public void IncreaseDamage()
    {
        attackDamage += 1;
    }
}