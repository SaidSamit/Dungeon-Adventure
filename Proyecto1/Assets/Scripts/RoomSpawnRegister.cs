using UnityEngine;

public class RoomSpawnRegister : MonoBehaviour
{
    private void Start()
    {
        // Comunicar la existencia de este punto de aparición al gestor central al iniciar la escena
        if (DungeonManager.Instance != null)
        {
            DungeonManager.Instance.RegisterSpawnPoint(transform);
        }
    }
}