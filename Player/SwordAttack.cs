using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [Header("Configura��es de Ataque")]
    
    [SerializeField] private Transform attackPointR;
    [SerializeField] private Transform attackPointL;
    [SerializeField] private Transform attackPointU;
    [SerializeField] private Transform attackPointD;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask enemyLayer;

    private void ApplyDamage(Collider2D hit)
    {
        if (hit != null)
        {
            Debug.Log("Acertou o inimigo/boss: " + hit.name);

            BossAnimationControl bossAnimControl = hit.GetComponentInChildren<BossAnimationControl>();
            if (bossAnimControl != null)
            {
                bossAnimControl.BossOnHit(AttackManager.DanoPlayer);
                return;
            }

            AnimationControl regularEnemyAnimControl = hit.GetComponentInChildren<AnimationControl>();
            if (regularEnemyAnimControl != null)
            {
                regularEnemyAnimControl.OnHit(AttackManager.DanoPlayer);
                return;
            }
        }
    }

    public void AttackRight()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPointR.position, radius, enemyLayer);
        ApplyDamage(hit);
    }

    public void AttackLeft()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPointL.position, radius, enemyLayer);
        ApplyDamage(hit);
    }

    public void AttackUp()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPointU.position, radius, enemyLayer);
        ApplyDamage(hit);
    }

    public void AttackDown()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPointD.position, radius, enemyLayer);
        ApplyDamage(hit);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (attackPointR != null) Gizmos.DrawWireSphere(attackPointR.position, radius);
        if (attackPointL != null) Gizmos.DrawWireSphere(attackPointL.position, radius);
        if (attackPointU != null) Gizmos.DrawWireSphere(attackPointU.position, radius);
        if (attackPointD != null) Gizmos.DrawWireSphere(attackPointD.position, radius);
    }
}
