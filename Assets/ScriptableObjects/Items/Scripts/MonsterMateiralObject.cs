using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Material", menuName ="Inventory System/Items/Monster Material")]
public class MonsterMateiralObject : ItemObject
{
    private void Awake()
    {
        Type = ItemType.MonsterMaterial;
    }
}
