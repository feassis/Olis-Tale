using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    public int RestoreHealthValue;

    private void Awake()
    {
        Type = ItemType.Food;
    }
}
