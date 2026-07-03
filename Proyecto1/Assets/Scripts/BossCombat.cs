using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [Header("Configuración del Golpe")]
    public int attackDamage = 2;
    public float knockbackForce = 8f;

    [Header("Área de Impacto (Hitbox)")]
    // Crea un objeto vacío hijo del jefe y arrástralo aquí
    public Transform attackPoint; 
    public float attackRadius = 1f;
    // Distancia extra hacia adelante para separar el golpe del cuerpo
    public float attackDistance = 1.2f; 
    public Vector3 centerOffset = new Vector3(0, -0.5f, 0); 
    
    // Asigna la capa donde está tu jugador (Ej. "PlayerLayer")
    public LayerMask playerLayer; 

    private Animator anim;
    private bool isHitboxActive = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAttackPointPosition();

        if (isHitboxActive)
        {
            CheckHit();
        }
    }

    private void UpdateAttackPointPosition()
    {
        float lastX = anim.GetFloat("lastMoveX");
        float lastY = anim.GetFloat("lastMoveY");
        
        Vector3 direction = new Vector3(lastX, lastY, 0).normalized;
        attackPoint.localPosition = centerOffset + (direction * attackDistance);
    }

    private void CheckHit()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
        
        foreach (Collider2D hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(-attackDamage, transform, knockbackForce);
                // Apagamos la hitbox tras acertar para evitar daño doble en un mismo frame
                isHitboxActive = false; 
            }
        }
    }

    // --- MÉTODOS PARA LLAMAR DESDE LA ANIMACIÓN ---
    public void EnableBossHitbox()
    {
        isHitboxActive = true;
    }

    public void DisableBossHitbox()
    {
        isHitboxActive = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}