using UnityEngine;
using UnityEngine.UI; // <-- Modificado para utilizar componentes visuales de interfaz

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    // Asignar la imagen de relleno de la barra de vida desde el Inspector
    public Image healthBarFill;

    private void Start()
    {
        // Iniciar la vida actual con la vida máxima definida en el StatManager
        currentHealth = StatManager.Instance.maxHealth;
        UpdateUI();
    }

    // --- FUNCIÓN: Solo para recibir daño ---
    public void TakeDamage(int amount, Transform damageSource, float knockbackForce)
    {
        // Sumar el valor recibido (el daño ya viene en negativo)
        currentHealth += amount;

        // Limitar la salud al máximo permitido por las stats actuales
        if (currentHealth > StatManager.Instance.maxHealth)
        {
            currentHealth = StatManager.Instance.maxHealth;
        }

        // Actualizar la barra visual de salud
        UpdateUI();

        // --- Lógica de Animación Direccional ---
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            // Calcular el vector de dirección desde el enemigo hacia la posición del jugador
            Vector2 directionToPlayer = (transform.position - damageSource.position).normalized;
            // Traducir el vector a un índice entero para el Blend Tree
            int directionIndex = CalculateHitDirection(directionToPlayer);
            
            // Enviar los parámetros al Animator para reproducir la animación de golpe
            anim.SetFloat("hitDirection", directionIndex);
            anim.SetTrigger("hurt");
        }

        // --- Activación del Knockback Dinámico ---
        // Evaluar si el personaje sigue vivo para aplicar el empuje físico
        if (currentHealth > 0)
        {
            PlayerMovement movementScript = GetComponent<PlayerMovement>();
            if (movementScript != null)
            {
                // Calcular la dirección opuesta al enemigo para aplicar el desplazamiento
                Vector2 knockbackDirection = (transform.position - damageSource.position).normalized;
                
                // Ejecutar el empuje aplicando la fuerza exacta dictada por el enemigo
                movementScript.ApplyKnockback(knockbackDirection, knockbackForce);
            }
        }

        // --- Muerte ---
        // Desactivar el personaje y notificar al GameManager si la vida llega a cero
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    // --- FUNCIÓN: Solo para curación y mejoras de stat ---
    public void changeHealth(int amount)
    {
        // Sumar el valor recibido (usado para pociones o mejoras del menú)
        currentHealth += amount;

        // Limitar la salud al máximo permitido por las stats actuales
        if (currentHealth > StatManager.Instance.maxHealth)
        {
            currentHealth = StatManager.Instance.maxHealth;
        }

        // Actualizar la barra visual de salud sin reproducir animaciones de dolor
        UpdateUI();
    }

    private int CalculateHitDirection(Vector2 direction)
    {
        // Convertir el vector direccional a grados (0 a 360) usando Atan2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        
        // Retornar 1 (Arriba), 2 (Izquierda), 0 (Abajo) o 3 (Derecha) según el cuadrante del impacto
        if (angle > 45 && angle <= 135) return 1; 
        if (angle > 135 && angle <= 225) return 2; 
        if (angle > 225 && angle <= 315) return 0; 
        
        return 3; 
    }

    void UpdateUI()
    {
        // Calcular el porcentaje de vida actual y aplicarlo al relleno de la barra visual
        if (healthBarFill != null)
        {
            // Convertir las variables a float para obtener valores decimales precisos (ej. 0.5 para la mitad)
            healthBarFill.fillAmount = (float)currentHealth / StatManager.Instance.maxHealth;
        }
    }
}