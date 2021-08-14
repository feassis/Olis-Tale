using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : DamagebleCharacter
{
    public MobSpawner MobSpawner;
    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;
        PlayerManager.instance.HealthBar.value = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public override void Die()
    {
        PlayerManager.instance.GameOver.Open();
    }
}
