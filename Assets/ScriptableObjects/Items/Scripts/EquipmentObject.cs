using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public EquipmentSlotType SlotType;
}

public enum EquipmentSlotType
{
    Head,
    Body,
    Legs,
    Ring
}
