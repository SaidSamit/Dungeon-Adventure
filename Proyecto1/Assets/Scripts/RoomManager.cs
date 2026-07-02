using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [Header("Configuración de Oleada")]
    // Asignar el prefab del enemigo principal
    public GameObject enemyPrefab;
    
    // NUEVO: Segundo prefab opcional (se puede dejar vacío en el Inspector)
    public GameObject secondaryEnemyPrefab;
    
    // Definir la cantidad total de enemigos para la oleada local
    public int totalEnemies = 1;

    [Header("Límites de Sala y Puerta")]
    public BoxCollider2D roomBounds;
    public float minDistanceFromPlayer = 4f;
    public DungeonDoor exitDoor;

    [Header("Interfaz de la Sala (BÚSQUEDA AUTOMÁTICA)")]
    public UpgradePanelUI upgradeUIController;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool playerEntered = false;
    private Transform playerTransform;

    private void Start()
    {
        if (upgradeUIController == null)
        {
            upgradeUIController = Object.FindAnyObjectByType<UpgradePanelUI>(FindObjectsInactive.Include);
        }

        if (exitDoor != null)
        {
            RandomTeleporter teleporter = exitDoor.GetComponentInChildren<RandomTeleporter>(true);
            if (teleporter != null)
            {
                teleporter.currentRoom = this;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerEntered)
        {
            playerEntered = true;
            playerTransform = collision.transform;
            SpawnEnemies();
        }
    }

    public void ResetRoomState()
    {
        playerEntered = false;
        activeEnemies.Clear();
        if (exitDoor != null)
        {
            exitDoor.CloseDoor();
        }
    }

    // --- MÉTODO MODIFICADO CON EL SISTEMA DE FALLBACK ---
    private void SpawnEnemies()
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            
            // Por defecto, asumimos que usaremos el enemigo principal
            GameObject prefabToSpawn = enemyPrefab;

            // Si pusiste un segundo enemigo, el script elige al azar entre los dos
            if (secondaryEnemyPrefab != null)
            {
                // Random.value genera un float entre 0.0 y 1.0
                if (Random.value > 0.5f)
                {
                    prefabToSpawn = secondaryEnemyPrefab;
                }
            }

            GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 position = Vector2.zero;
        bool validPosition = false;
        int attempts = 0;

        while (!validPosition && attempts < 50)
        {
            float randomX = Random.Range(roomBounds.bounds.min.x, roomBounds.bounds.max.x);
            float randomY = Random.Range(roomBounds.bounds.min.y, roomBounds.bounds.max.y);
            position = new Vector2(randomX, randomY);

            if (playerTransform != null)
            {
                float distance = Vector2.Distance(position, playerTransform.position);
                if (distance >= minDistanceFromPlayer)
                {
                    validPosition = true;
                }
            }
            else
            {
                validPosition = true;
            }
            attempts++;
        }
        return position;
    }

    private void Update()
    {
        if (playerEntered && activeEnemies.Count > 0)
        {
            activeEnemies.RemoveAll(item => item == null);

            if (activeEnemies.Count == 0)
            {
                ShowUpgradeMenu();
            }
        }
    }

    private void ShowUpgradeMenu()
    {
        if (upgradeUIController != null)
        {
            upgradeUIController.gameObject.SetActive(true);
            Time.timeScale = 0f;
            upgradeUIController.SetupMenu(ResumeGameAndOpenDoor);
        }
    }

    public void ResumeGameAndOpenDoor()
    {
        if (upgradeUIController != null)
        {
            upgradeUIController.gameObject.SetActive(false);
        }
        Time.timeScale = 1f;
        if (exitDoor != null)
        {
            exitDoor.OpenDoor();
        }
    }
}