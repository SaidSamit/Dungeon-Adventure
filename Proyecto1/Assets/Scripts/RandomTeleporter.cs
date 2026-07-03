using UnityEngine;

public class RandomTeleporter : MonoBehaviour
{
    [Header("Referencias Locales")]
    public Transform localRoomSpawn;
    [HideInInspector] public RoomManager currentRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (DungeonManager.Instance != null)
            {
                // Llamamos a la nueva función que incluye el contador de progreso
                Transform targetSpawn = DungeonManager.Instance.GetNextSpawnPoint(localRoomSpawn);

                if (targetSpawn != null)
                {
                    collision.transform.position = targetSpawn.position;

                    if (currentRoom != null)
                    {
                        currentRoom.ResetRoomState();
                    }
                }
            }
        }
    }
}