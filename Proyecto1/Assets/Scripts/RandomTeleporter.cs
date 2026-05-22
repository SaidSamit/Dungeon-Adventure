using UnityEngine;

public class RandomTeleporter : MonoBehaviour
{
    [Header("Referencias Locales")]
    // Asignar el punto de destino de la propia sala en la que se encuentra este portal
    public Transform localRoomSpawn;

    // Referencia asignada automáticamente para controlar el reinicio de la sala correspondiente
    [HideInInspector] public RoomManager currentRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detectar si la entidad que ingresa al portal es el jugador
        if (collision.CompareTag("Player"))
        {
            if (DungeonManager.Instance != null)
            {
                // Solicitar un punto de destino aleatorio excluyendo el origen actual del personaje
                Transform targetSpawn = DungeonManager.Instance.GetRandomSpawnPoint(localRoomSpawn);

                if (targetSpawn != null)
                {
                    // Desplazar la posición física del jugador al destino seleccionado
                    collision.transform.position = targetSpawn.position;

                    // Ordenar a la sala de origen restablecer su estado inmediatamente después del viaje exitoso
                    if (currentRoom != null)
                    {
                        currentRoom.ResetRoomState();
                    }
                }
            }
        }
    }
}