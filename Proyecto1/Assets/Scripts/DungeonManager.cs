using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    public List<Transform> allRoomSpawnPoints = new List<Transform>();
    
    [Header("Configuración de Jefe")]
    public Transform bossRoomSpawnPoint; // Nodo exclusivo para el jefe
    public int roomsCleared = 0; // Contador de saltos
    public int roomsBeforeBoss = 5; // Regla de enrutamiento (cada 5 salas)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Actualizamos el registro para separar el nodo del jefe de los nodos normales
    public void RegisterSpawnPoint(Transform spawnPoint, bool isBoss)
    {
        if (isBoss)
        {
            bossRoomSpawnPoint = spawnPoint;
        }
        else if (!allRoomSpawnPoints.Contains(spawnPoint))
        {
            allRoomSpawnPoints.Add(spawnPoint);
        }
    }

    // Nueva lógica de enrutamiento
    public Transform GetNextSpawnPoint(Transform currentRoomSpawn)
    {
        // 1. Contabilizamos la sala que acabamos de limpiar
        roomsCleared++;

        // 2. Evaluamos la regla: Si es múltiplo de 5 (5, 10, 15...) y existe la sala
        if (roomsCleared % roomsBeforeBoss == 0 && bossRoomSpawnPoint != null)
        {
            return bossRoomSpawnPoint;
        }

        // 3. Tráfico normal: Retornar sala aleatoria
        if (allRoomSpawnPoints.Count <= 1) return currentRoomSpawn;

        List<Transform> validTargets = new List<Transform>(allRoomSpawnPoints);
        validTargets.Remove(currentRoomSpawn);

        int randomIndex = Random.Range(0, validTargets.Count);
        return validTargets[randomIndex];
    }
}