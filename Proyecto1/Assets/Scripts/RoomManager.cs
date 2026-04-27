using UnityEngine;

// Gestionar los enemigos, la interfaz local y los eventos de la sala
public class RoomManager : MonoBehaviour
{
    [Header("Configuración de Oleada")]
    public GameObject enemyPrefab;
    public int totalEnemies = 10;
    
    [Header("Límites de Sala")]
    public BoxCollider2D roomBounds; 
    public float minDistanceFromPlayer = 4f;

    [Header("Interfaz de la Sala")]
    public GameObject upgradePanel; 

    private Transform player;
    private bool roomCleared = false;

    void Start()
    {
        // Obtener la referencia del jugador
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (roomBounds == null) return;
        SpawnWave();
    }

    void Update()
    {
        // Monitorear la limpieza de la sala en tiempo real
        if (!roomCleared)
        {
            CheckEnemiesCleared();
        }
    }

    void SpawnWave()
    {
        // Instanciar entidades enemigas en posiciones calculadas
        for (int i = 0; i < totalEnemies; i++)
        {
            Vector3 spawnPos = GetValidEdgePosition();
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    Vector3 GetValidEdgePosition()
    {
        Bounds b = roomBounds.bounds;
        Vector3 randomPos = Vector3.zero;
        bool valid = false;
        int safetyNet = 0;

        // Calcular punto de aparición validando colisiones
        while (!valid && safetyNet < 50)
        {
            int side = Random.Range(0, 4); 
            float x = 0, y = 0;

            switch (side)
            {
                case 0: x = Random.Range(b.min.x, b.max.x); y = b.max.y; break;
                case 1: x = Random.Range(b.min.x, b.max.x); y = b.min.y; break;
                case 2: x = b.min.x; y = Random.Range(b.min.y, b.max.y); break;
                case 3: x = b.max.x; y = Random.Range(b.min.y, b.max.y); break;
            }

            randomPos = new Vector3(x, y, 0);

            if (player != null && Vector2.Distance(randomPos, player.position) > minDistanceFromPlayer)
            {
                valid = true;
            }
            safetyNet++;
        }
        return randomPos;
    }

    void CheckEnemiesCleared()
    {
        // Verificar condición de victoria contando objetos activos
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            roomCleared = true; 
            OnRoomCleared();
        }
    }

    void OnRoomCleared()
    {
        // Activar interfaz gráfica local y detener el flujo lógico
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResetRoom()
    {
        // Desactivar interfaz gráfica, reanudar tiempo e instanciar nuevos retos
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
        
        Time.timeScale = 1f; 
        roomCleared = false;
        SpawnWave();
    }

    // --- Eventos de Botones de la Interfaz ---

    public void OnUpgradeHealthClicked()
    {
        // Delegar aumento al Singleton y reiniciar sala
        StatManager.Instance.IncreaseMaxHealth();
        ResetRoom();
    }

    public void OnUpgradeSpeedClicked()
    {
        // Delegar aumento al Singleton y reiniciar sala
        StatManager.Instance.IncreaseSpeed();
        ResetRoom();
    }

    public void OnUpgradeDamageClicked()
    {
        // Delegar aumento al Singleton y reiniciar sala
        StatManager.Instance.IncreaseDamage();
        ResetRoom();
    }
}