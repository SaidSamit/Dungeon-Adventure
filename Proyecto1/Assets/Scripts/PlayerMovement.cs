using UnityEngine;
using System.Collections; 

public class PlayerMovement : MonoBehaviour
{
    public int facingDirection = 1; // 1 = Derecha, -1 = Izquierda
    public Rigidbody2D rb;
    public Animator anim;
    public PlayerCombat playerCombat;

    private Vector2 movement;

    [Header("Configuración del Knockback")]
    // Definir la duración de la pérdida de control al recibir un impacto
    [SerializeField] private float knockbackDuration = 0.2f; 
    // Usar bandera para bloquear el movimiento normal durante el empuje
    private bool isKnockingBack = false;

    private void Update()
    {
        // Detectar el input de ataque en cada frame
        if (Input.GetButtonDown("Attack"))
        {
            playerCombat.Attack();
        }

        // Capturar el input de movimiento horizontal y vertical sin aceleración
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Actualizar los parámetros de movimiento en el Animator
        anim.SetFloat("moveX", movement.x);
        anim.SetFloat("moveY", movement.y);

        // Evaluar si el personaje se encuentra en movimiento
        bool isMoving = movement.magnitude > 0;
        anim.SetBool("isMoving", isMoving);

        // Guardar la última dirección registrada al moverse para usarla en el Idle
        if (isMoving)
        {
            anim.SetFloat("lastMoveX", movement.x);
            anim.SetFloat("lastMoveY", movement.y);

            // Actualizar el sentido del personaje según el eje X para el sistema de combate
            if (movement.x > 0)
            {
                facingDirection = 1;
            }
            else if (movement.x < 0)
            {
                facingDirection = -1;
            }
        }
    }

    void FixedUpdate()
    {
        // Ignorar el movimiento por input si el personaje está siendo empujado
        if (isKnockingBack) return;

        Vector2 velocityDir = movement;

        // Normalizar el vector para evitar un aumento de velocidad en movimiento diagonal
        if (velocityDir.magnitude > 1)
        {
            velocityDir = velocityDir.normalized;
        }

        // Aplicar la velocidad física en el Rigidbody2D usando las stats de StatManager
        rb.linearVelocity = velocityDir * StatManager.Instance.moveSpeed;
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        // Cancelar la ejecución si el personaje ya está siendo empujado por otro impacto simultáneo
        if (isKnockingBack) return; 

        // Iniciar la corrutina para gestionar el tiempo y la fuerza del empuje
        StartCoroutine(KnockbackRoutine(direction, force));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float force)
    {
        // Activar el bloqueo de movimiento
        isKnockingBack = true; 

        // Frenar en seco la inercia previa para lograr un empuje limpio
        rb.linearVelocity = Vector2.zero;

        // Aplicar la fuerza física al personaje de forma instantánea
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        // Congelar la corrutina el tiempo configurado antes de recuperar el control
        yield return new WaitForSeconds(knockbackDuration);

        // Limpiar la velocidad residual física y liberar el movimiento
        rb.linearVelocity = Vector2.zero;
        isKnockingBack = false;
    }
}