using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Atributos del Jefe")]
    public float speed = 2.5f;
    // Distancia a la que se detiene para no empujarte mientras ataca
    public float stopDistance = 1.2f; 
    
    [Header("Tiempos y Rangos")]
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    private float cooldownTimer = 0f;

    [Header("Estado")]
    public bool isAttacking = false;
    public bool isDead = false;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // Buscar automáticamente al jugador en la sala
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Solo tomar decisiones si el jugador existe y NO estamos a mitad de un ataque
        if (player != null && !isAttacking)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= attackRange && cooldownTimer <= 0)
            {
                StartAttack();
            }
            else if (distance > stopDistance)
            {
                ChasePlayer();
            }
            else
            {
                // Estamos cerca pero esperando el Cooldown: quedarse quieto e intimidar
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("isMoving", false);
                
                // Actualizar la mirada hacia el jugador mientras espera
                FacePlayer(); 
            }
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        anim.SetFloat("lastMoveX", direction.x);
        anim.SetFloat("lastMoveY", direction.y);
        anim.SetBool("isMoving", true);
    }

    private void StartAttack()
    {
        isAttacking = true;
        // Frenar en seco para ejecutar el ataque
        rb.linearVelocity = Vector2.zero; 
        anim.SetBool("isMoving", false);
        
        FacePlayer();
        
        anim.SetTrigger("attack");
        cooldownTimer = attackCooldown;
    }

    private void FacePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        // Enviar la última dirección para que el Blend Tree sepa a dónde mirar
        anim.SetFloat("lastMoveX", direction.x);
        anim.SetFloat("lastMoveY", direction.y);
    }

    // --- MÉTODOS PARA LLAMAR DESDE LA ANIMACIÓN ---
    public void FinishAttack()
    {
        // Esto le devuelve el control de movimiento al jefe
        isAttacking = false;
    }
}