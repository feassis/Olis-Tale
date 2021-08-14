using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagebleCharacter : MonoBehaviour
{
    public Animator Animator;
    public bool IsDead = false;
    [SerializeField] protected float maxHealth;
    protected float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Animator.SetTrigger("hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Animator.SetBool("isDead", true);

        IsDead = true;

        GetComponent<CapsuleCollider>().enabled = false;
        this.enabled = false;
    }
}
