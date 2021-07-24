using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : DamagebleCharacter
{
    [SerializeField] private LootManager myLoot;

    public override void Die()
    {
        base.Die();
        myLoot.InstantiateLoot();
    }
}
