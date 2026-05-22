using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [Header("Configuración de Oleada")]
    // Asignar el prefab del enemigo a generar en esta sala específica
    public GameObject enemyPrefab;
    // Definir la cantidad total de enemigos para la oleada local
    public int totalEnemies = 1;

    [Header("Límites de Sala y Puerta")]
    // Asignar el colisionador que define el área de aparición
    public BoxCollider2D roomBounds;
    // Definir la distancia mínima para evitar apariciones sobre el jugador
    public float minDistanceFromPlayer = 4f;
    // Asignar la puerta local de esta sala (requiere el componente DungeonDoor)
    public DungeonDoor exitDoor;

    [Header("Interfaz de la Sala (BÚSQUEDA AUTOMÁTICA)")]
    // La variable se completará automáticamente al iniciar el juego
    public UpgradePanelUI upgradeUIController;

    // --- Estado Local de la Sala ---
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool playerEntered = false;
    private Transform playerTransform;

    private void Start()
    {
        // Buscar automáticamente el componente de interfaz en la escena si la casilla se encuentra vacía
        if (upgradeUIController == null)
        {
            upgradeUIController = Object.FindAnyObjectByType<UpgradePanelUI>(FindObjectsInactive.Include);
        }

        // Vincular esta sala al teletransportador local para gestionar el reinicio controlado
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
        // Detectar el ingreso del jugador al área de la sala por primera vez
        if (collision.CompareTag("Player") && !playerEntered)
        {
            playerEntered = true;
            playerTransform = collision.transform;
            
            // Iniciar la generación de la oleada de monstruos
            SpawnEnemies();
        }
    }

    public void ResetRoomState()
    {
        // Restablecer el estado de control de ingreso del personaje
        playerEntered = false;

        // Limpiar por completo el registro de la lista de enemigos activos
        activeEnemies.Clear();

        // Ordenar a la puerta local restablecer sus componentes físicos y visuales
        if (exitDoor != null)
        {
            exitDoor.CloseDoor();
        }
    }

    private void SpawnEnemies()
    {
        // Ejecutar el ciclo de instanciación según el total definido
        for (int i = 0; i < totalEnemies; i++)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
            // Registrar el enemigo en la lista de entidades activas de esta sala específica
            activeEnemies.Add(enemy);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 position = Vector2.zero;
        bool validPosition = false;
        int attempts = 0;

        // Buscar coordenadas aleatorias dentro de los límites hasta encontrar una válida o alcanzar el límite de intentos
        while (!validPosition && attempts < 50)
        {
            float randomX = Random.Range(roomBounds.bounds.min.x, roomBounds.bounds.max.x);
            float randomY = Random.Range(roomBounds.bounds.min.y, roomBounds.bounds.max.y);
            position = new Vector2(randomX, randomY);

            // Validar que la posición calculada mantenga la distancia mínima requerida respecto al jugador
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
        // Monitorear el estado de los enemigos si el combate ya inició
        if (playerEntered && activeEnemies.Count > 0)
        {
            // Eliminar de la lista las referencias a los objetos destruidos (enemigos muertos)
            activeEnemies.RemoveAll(item => item == null);

            // Validar si la lista está vacía para finalizar el evento de la sala
            if (activeEnemies.Count == 0)
            {
                ShowUpgradeMenu();
            }
        }
    }

    private void ShowUpgradeMenu()
    {
        // Validar la existencia del controlador de interfaz genérico
        if (upgradeUIController != null)
        {
            // Activar el panel visualmente
            upgradeUIController.gameObject.SetActive(true);
            // Pausar el flujo del tiempo del juego
            Time.timeScale = 0f;

            // Enviar la orden de reanudación al mensajero de forma segura y sin alterar los botones
            upgradeUIController.SetupMenu(ResumeGameAndOpenDoor);
        }
    }

    public void ResumeGameAndOpenDoor()
    {
        // Ocultar la interfaz visual de mejoras
        if (upgradeUIController != null)
        {
            upgradeUIController.gameObject.SetActive(false);
        }
        
        // Reanudar el flujo del tiempo a su estado normal
        Time.timeScale = 1f;

        // Ejecutar la orden de apertura en la puerta asignada a esta sala local
        if (exitDoor != null)
        {
            exitDoor.OpenDoor();
        }
    }
}