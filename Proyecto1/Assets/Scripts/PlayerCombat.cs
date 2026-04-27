using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1;
    public LayerMask enemyLayer;
    public Animator anim;
    public float cooldown;
    private float cooldownTimer;

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (cooldownTimer <= 0)
        {
            anim.SetBool("isAtacking", true);

            Collider2D[] enemies = Physics2D.OverlapCircleAll
            (attackPoint.position, attackRange, enemyLayer);
            
            if (enemies.Length > 0)
            {
                enemies[0].GetComponent<EnemyHealth>(

                ).changeHealth(-StatManager.Instance.attackDamage);
            }
            cooldownTimer = cooldown;
        }
    }

    public void StopAttack()
    {
        anim.SetBool("isAtacking", false);
    }
}