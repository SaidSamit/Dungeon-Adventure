using UnityEngine;

public class RoomSpawnRegister : MonoBehaviour
{
    [Header("Configuración de Sala")]
    // Casilla para marcar ÚNICAMENTE en tu sala especial del jefe
    public bool isBossRoom = false; 

    private void Start()
    {
        if (DungeonManager.Instance != null)
        {
            // Enviamos nuestra identidad (normal o jefe) al gestor
            DungeonManager.Instance.RegisterSpawnPoint(transform, isBossRoom);
        }
    }
}