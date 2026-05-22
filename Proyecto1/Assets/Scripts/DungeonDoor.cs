using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    [Header("Configuración Visual")]
    // Asignar el componente SpriteRenderer de la puerta
    public SpriteRenderer spriteRenderer;
    // Asignar el sprite correspondiente a la puerta abierta
    public Sprite openDoorSprite;
    // Variable interna para almacenar el diseño original de la puerta cerrada
    private Sprite defaultClosedSprite;

    [Header("Componentes Físicos")]
    // Asignar el colisionador sólido que bloquea el paso inicialmente
    public Collider2D solidCollider;

    [Header("Componentes del Portal")]
    // Asignar el objeto hijo que contiene el colisionador de teletransporte
    public GameObject portalZone;

    private void Start()
    {
        // Asegurar que el portal inicie desactivado al comenzar la sala
        if (portalZone != null)
        {
            portalZone.SetActive(false);
        }

        // Obtener el colisionador automáticamente si no se asignó manualmente en el Inspector
        if (solidCollider == null)
        {
            solidCollider = GetComponent<Collider2D>();
        }

        // Guardar el sprite inicial para poder restaurarlo al cerrar la puerta posteriormente
        if (spriteRenderer != null)
        {
            defaultClosedSprite = spriteRenderer.sprite;
        }
    }

    public void OpenDoor()
    {
        // Cambiar el aspecto visual de la puerta al estado abierto
        if (spriteRenderer != null && openDoorSprite != null)
        {
            spriteRenderer.sprite = openDoorSprite;
        }

        // Desactivar la pared invisible para permitir el cruce físico del personaje
        if (solidCollider != null)
        {
            solidCollider.enabled = false;
        }

        // Habilitar la zona del portal para detectar al jugador y ejecutar el cambio de sala
        if (portalZone != null)
        {
            portalZone.SetActive(true);
        }
    }

    public void CloseDoor()
    {
        // Restaurar el aspecto visual original correspondiente al estado cerrado
        if (spriteRenderer != null && defaultClosedSprite != null)
        {
            spriteRenderer.sprite = defaultClosedSprite;
        }

        // Reactivar la pared física para bloquear el avance del jugador nuevamente
        if (solidCollider != null)
        {
            solidCollider.enabled = true;
        }

        // Desactivar la zona del portal para evitar teletransportes accidentales
        if (portalZone != null)
        {
            portalZone.SetActive(false);
        }
    }
}