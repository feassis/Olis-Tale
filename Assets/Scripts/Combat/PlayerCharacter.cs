using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : DamagebleCharacter
{
    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public override void Die()
    {
        Debug.Log("Player Died");
    }
}
