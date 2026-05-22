using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1f;
    public float attackOffset = 0.5f; 
    
    // Nueva variable para bajar el centro del ataque desde la cabeza hasta el pecho/cuerpo
    public Vector3 bodyCenterOffset = new Vector3(0f, -0.5f, 0f); 
    
    public LayerMask enemyLayer;
    public Animator anim;
    public float cooldown;
    private float cooldownTimer;

    private bool isHitboxActive = false;
    private List<EnemyHealth> enemiesHitThisSwing = new List<EnemyHealth>();

    private void Update()
    {
        // Bajar el tiempo del cooldown en cada frame
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Chequear impactos en cada frame mientras la hitbox esté activa
        if (isHitboxActive)
        {
            CheckForHitsPerFrame();
        }

        // Mover dinámicamente el área de ataque desde el centro del cuerpo hacia donde mira el personaje
        UpdateAttackPointPosition();
    }

    public void Attack()
    {
        // Iniciar el ataque solo si el cooldown ya terminó
        if (cooldownTimer <= 0)
        {
            // Activar el trigger en el animator para reproducir la animación
            anim.SetTrigger("attack");
            
            // Reiniciar el contador del cooldown
            cooldownTimer = cooldown;

            // Limpiar la lista de enemigos golpeados al iniciar un nuevo barrido
            enemiesHitThisSwing.Clear();
        }
    }

    private void CheckForHitsPerFrame()
    {
        // Detectar a los enemigos que entren en el área del círculo rojo
        Collider2D[] enemiesInRage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        
        foreach (Collider2D enemyCollider in enemiesInRage)
        {
            EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>();

            // Verificar que sea un enemigo válido y que no se haya golpeado en este ataque
            if (enemyHealth != null && !enemiesHitThisSwing.Contains(enemyHealth))
            {
                // Aplicar el daño
                enemyHealth.changeHealth(-StatManager.Instance.attackDamage);

                // Agregar el enemigo a la lista para ignorarlo en los siguientes frames de este barrido
                enemiesHitThisSwing.Add(enemyHealth);
            }
        }
    }

    private void UpdateAttackPointPosition()
    {
        // Leer la última dirección registrada en el Animator
        float lastX = anim.GetFloat("lastMoveX");
        float lastY = anim.GetFloat("lastMoveY");

        // Crear un vector direccional normalizado para la dirección
        Vector3 direction = new Vector3(lastX, lastY, 0).normalized;

        // Posicionar el ataque sumando el centro del cuerpo más la dirección multiplicada por el offset
        attackPoint.localPosition = bodyCenterOffset + (direction * attackOffset);
    }

    public void EnableHitbox()
    {
        // Activar la detección de daño mediante el evento de animación
        isHitboxActive = true;
    }

    public void DisableHitbox()
    {
        // Desactivar la detección de daño al terminar el corte en la animación
        isHitboxActive = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Evitar errores de consola si no hay un attackPoint asignado
        if (attackPoint == null) return;

        // Definir el color rojo para la guía visual
        Gizmos.color = Color.red;

        // Dibujar el círculo de impacto con el rango definido
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}