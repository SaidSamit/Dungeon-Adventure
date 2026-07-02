using UnityEngine;

public class SpinnerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 3f;
    private bool isChasing;
    private Transform player;

    public Animator anim;
    public bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (anim == null) 
        {
            anim = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Cancelar lógica si está derrotado
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isChasing && player != null)
        {
            // 1. Calcular la diferencia de distancia en ambos ejes
            float diffX = player.position.x - transform.position.x;
            float diffY = player.position.y - transform.position.y;

            Vector2 moveDirection = Vector2.zero;

            // 2. Determinar el eje dominante (el más largo) para moverse estrictamente en cruz
            if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
            {
                // Moverse horizontalmente (usamos Mathf.Sign para saber si es izquierda (-1) o derecha (1))
                moveDirection = new Vector2(Mathf.Sign(diffX), 0);
                
                // Voltear el sprite si es necesario
                transform.localScale = new Vector3(Mathf.Sign(diffX), 1, 1);
            }
            else
            {
                // Moverse verticalmente
                moveDirection = new Vector2(0, Mathf.Sign(diffY));
            }

            // 3. Aplicar la velocidad
            rb.linearVelocity = moveDirection * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        // 4. Actualizar la animación
        bool movingState = rb.linearVelocity.magnitude > 0;
        anim.SetBool("isMoving", movingState);
    }

    // Usar la misma lógica de Aggro que ya tenías
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        if(collision.gameObject.CompareTag("Player"))
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }   
}