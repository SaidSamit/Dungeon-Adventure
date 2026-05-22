using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    // Asignar el daño base que hará este enemigo
    public int damage = 1;
    // Definir la fuerza de empuje propia de esta entidad
    public float knockbackForce = 10f; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto impactado es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtener el script de salud para aplicar el daño
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // Validar que el script exista antes de intentar usarlo
            if (playerHealth != null)
            {
                // Enviar el daño negativo, la posición exacta y la fuerza de empuje del enemigo
                playerHealth.TakeDamage(-damage, transform, knockbackForce);
            }
        }
    }
}