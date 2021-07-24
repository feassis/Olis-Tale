using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [HideInInspector]
    public bool IsAttacking = false;

    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider cartCollider;
    [SerializeField] private float heavyAttackRecoveryDelay = 0.5f;

    [Header("Attack Stats")]
    [SerializeField] private float lightAttackDamage;
    [SerializeField] private float heavyAttackDamage;
    [SerializeField] private float dashAttackDamage;

    [Header("Attack Range")]
    public LayerMask enemyLayer;
    [SerializeField] private Transform lightAttackTransform;
    [SerializeField] private float lightAttackRange;
    [SerializeField] private Transform heavyAttackTransform;
    [SerializeField] private float heavyAttackRange;
    [SerializeField] private Transform dashAttackTransform;
    [SerializeField] private float dashAttackRange;

    private const string lightAttackAnim = "SpinAttackAnim";
    private const string heavyAttackAnim = "HeavyAttackAnim";
    private const string heavyAttackRecoveryAnim = "HeavyAttackRecoveryAnim";
    private const string idle = "Idle";

    public void LightAttack()
    {
        if (!IsAttacking)
        {
            StartCoroutine(LightAttackCourotine());
        }
    }

    public void HeavyAttack()
    {
        if (!IsAttacking)
        {
            StartCoroutine(HeavyAttackCourotine());
        }
    }

    private IEnumerator LightAttackCourotine()
    {
        IsAttacking = true;

        animator.Play(lightAttackAnim);
        

        Collider[] enemysInRange = GetEnemiesInRange(lightAttackTransform, lightAttackRange);
        DamageEnemy(enemysInRange, lightAttackDamage);

        yield return WaitAnimationToEnd(lightAttackAnim);

        animator.Play(idle);
        
        IsAttacking = false;
    }

    private IEnumerator HeavyAttackCourotine()
    {
        IsAttacking = true;

        animator.Play(heavyAttackAnim);
        

        yield return WaitAnimationToEnd(heavyAttackAnim);
        cartCollider.enabled = false;
        Collider[] enemiesInRange = GetEnemiesInRange(heavyAttackTransform, heavyAttackRange);
        DamageEnemy(enemiesInRange, heavyAttackDamage);

        yield return new WaitForSeconds(heavyAttackRecoveryDelay);
        
        animator.Play(heavyAttackRecoveryAnim);
        yield return WaitAnimationToEnd(heavyAttackRecoveryAnim);

        IsAttacking = false;
    }

    private Collider[] GetEnemiesInRange(Transform pointOfOrigin, float range)
    {
        return Physics.OverlapSphere(pointOfOrigin.position, range, enemyLayer);
    }

    private void DamageEnemy(Collider[] enemiesInRange, float damage)
    {
        foreach (Collider enemy in enemiesInRange)
        {
            enemy.GetComponent<DamagebleCharacter>().TakeDamage(damage);
        }
    }

    private IEnumerator WaitAnimationToEnd(string animName)
    {
        AnimatorStateInfo state;
        do
        {
            yield return null;
            state = animator.GetCurrentAnimatorStateInfo(0);
        } while (!state.IsName(animName) || state.normalizedTime < 1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(lightAttackTransform.position, lightAttackRange);
        Gizmos.DrawWireSphere(heavyAttackTransform.position, heavyAttackRange);
    }
}
