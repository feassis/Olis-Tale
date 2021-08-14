using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : DamagebleCharacter
{
    public Mobs type;
    public MobSpawner spawner;
    [SerializeField] private LootManager myLoot;

    public override void Die()
    {
        base.Die();

        if(spawner != null)
        {
            foreach (MobHandler mob in spawner.MobList)
            {
                if (mob.Entry.Mob.type == type)
                {
                    mob.CurrentMobs--;
                }
            }
        }
        
        myLoot.InstantiateLoot();
        StartCoroutine(DestroyEnemy());
    }

    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(180f);
        
        Destroy(this.gameObject);
    }
}
