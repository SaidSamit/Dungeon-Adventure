using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private bool isChasing;
    private Transform player;

    // Referencia al Animator para controlar las animaciones visuales
    public Animator anim;
    // Bandera para detener la lógica física al ser derrotado
    public bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Obtener el componente automáticamente si no se asignó en el Inspector
        if (anim == null) 
        {
            anim = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Cancelar la lógica de persecución si la entidad ha sido derrotada
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if(isChasing == true && player != null)
        {
            // Calcular la dirección hacia el objetivo
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;

            // Voltear el sprite visualmente para mirar en la dirección del movimiento
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
            }
        }
        else
        {
            // Detener el movimiento si no se está persiguiendo a nadie
            rb.linearVelocity = Vector2.zero;
        }

        // Enviar el estado de movimiento al Animator (verdadero si hay velocidad física)
        bool movingState = rb.linearVelocity.magnitude > 0;
        anim.SetBool("isMoving", movingState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Iniciar la persecución solo si el jugador entra al área y la entidad sigue viva
        if(collision.gameObject.CompareTag("Player") && !isDead)
        {
            if (player == null)
            {
                player = collision.transform;
            }
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Cancelar la persecución al salir el objetivo del área
        if(collision.gameObject.CompareTag("Player"))
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }   
}