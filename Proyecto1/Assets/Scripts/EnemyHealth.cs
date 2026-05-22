using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public Animator anim;

    // Bandera para evitar ejecutar la función de muerte múltiples veces
    private bool isDead = false;

    private void Start()
    {
        // Establecer la vida inicial al máximo configurado
        currentHealth = maxHealth;
        
        // Asignar el componente automáticamente si no está definido
        if (anim == null) 
        {
            anim = GetComponent<Animator>();
        }
    }

    public void changeHealth(int amount)
    {
        // Ignorar el cálculo de daño si la entidad ya se encuentra derrotada
        if (isDead) return;

        // Sumar la cantidad recibida (el daño se recibe en valores negativos)
        currentHealth += amount;

        // Limitar la salud al tope máximo
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        // Procesar impacto y reproducir animación de dolor si sigue con vida
        else if(currentHealth > 0 && amount < 0)
        {
            anim.SetTrigger("hurt");
        }
        // Ejecutar protocolo de derrota al agotar los puntos de vida
        else if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Marcar la entidad como derrotada
        isDead = true;

        // Reproducir la animación final
        anim.SetTrigger("die");

        // Comunicar al script de movimiento que debe detener la persecución
        EnemyMovement movementScript = GetComponent<EnemyMovement>();
        if (movementScript != null)
        {
            movementScript.isDead = true;
        }

        // Desactivar el colisionador físico para permitir transitar sobre los restos
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Retrasar la destrucción del objeto en memoria para permitir que la animación se observe completamente
        // (Nota: Ajustar el valor "0.5f" si la animación de muerte dura más o menos tiempo)
        Destroy(gameObject, 0.5f); 
    }
}