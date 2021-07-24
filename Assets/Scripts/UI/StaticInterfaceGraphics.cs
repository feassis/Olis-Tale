using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterfaceGraphics : UserInterfaceGraphics
{
    public GameObject[] Slots;
    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < Inventory.Container.Itens.Length; i++)
        {
            var obj = Slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnPointerEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnPointerExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnBeginDrag(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnEndDrag(obj); });

            slotsOnInterface.Add(obj, Inventory.Container.Itens[i]);
        }
    }
}
