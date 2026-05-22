using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public int damageAmount = -10;
    public float damageCooldown = 1f;
    private float cooldownTimer;
    private bool isDamageActive = false;
    private PlayerHealth playerInRange;

    private void Update()
    {
        // Reducir el temporizador de daño en cada frame
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Aplicar daño si la trampa está activa y el jugador sigue encima de ella
        if (isDamageActive && playerInRange != null && cooldownTimer <= 0)
        {
            ApplyDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Guardar la referencia si el objeto impactado es el jugador
        if (collision.CompareTag("Player"))
        {
            playerInRange = collision.GetComponent<PlayerHealth>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Limpiar la referencia al salir de la trampa
        if (collision.CompareTag("Player"))
        {
            playerInRange = null;
        }
    }

    private void ApplyDamage()
    {
        // Validar si el jugador está parado sobre los pinchos
        if (playerInRange != null)
        {
            // Aplicar el daño sin fuerza de empuje para evitar que el personaje salga volando de la trampa
            playerInRange.TakeDamage(damageAmount, transform, 0f);
            
            // Reiniciar el temporizador para calcular el siguiente tick de daño
            cooldownTimer = damageCooldown;
        }
    }

    public void EnableTrapDamage()
    {
        // Activar la detección de daño desde el evento de animación
        isDamageActive = true;
        
        // Aplicar daño inmediato si el jugador ya estaba pisando la trampa
        if (cooldownTimer <= 0)
        {
            ApplyDamage();
        }
    }

    public void DisableTrapDamage()
    {
        // Desactivar el daño al ocultarse la trampa en la animación
        isDamageActive = false;
    }
}