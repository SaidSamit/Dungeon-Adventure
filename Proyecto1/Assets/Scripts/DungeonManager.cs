using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    // Almacenar dinámicamente los puntos de aparición de todas las salas activas
    public List<Transform> allRoomSpawnPoints = new List<Transform>();

    private void Awake()
    {
        // Configurar el patrón Singleton para asegurar un único gestor de mazmorra
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterSpawnPoint(Transform spawnPoint)
    {
        // Añadir el punto de aparición a la lista global si no se encuentra registrado previamente
        if (!allRoomSpawnPoints.Contains(spawnPoint))
        {
            allRoomSpawnPoints.Add(spawnPoint);
        }
    }

    public Transform GetRandomSpawnPoint(Transform currentRoomSpawn)
    {
        // Retornar el punto actual si no existen más salas registradas en la partida
        if (allRoomSpawnPoints.Count <= 1) return currentRoomSpawn;

        // Clonar la lista para realizar un filtrado seguro sin alterar la original
        List<Transform> validTargets = new List<Transform>(allRoomSpawnPoints);
        
        // Remover el punto de la sala actual para evitar teletransportar al jugador al mismo lugar
        validTargets.Remove(currentRoomSpawn);

        // Seleccionar un índice aleatorio de la lista de destinos válidos
        int randomIndex = Random.Range(0, validTargets.Count);
        return validTargets[randomIndex];
    }
}