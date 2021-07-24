using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DynamicInterfaceGraphics : UserInterfaceGraphics
{
    public GameObject InventoryPrefab;

    [SerializeField] protected int xSpaceBetweenItems;
    [SerializeField] protected int ySpaceBetweenItems;
    [SerializeField] protected int xStart;
    [SerializeField] protected int yStart;
    [SerializeField] protected int numberOfColumns;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < Inventory.Container.Itens.Length; i++)
        {
            var obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetSlotPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnPointerEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnPointerExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnBeginDrag(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnEndDrag(obj); });

            slotsOnInterface.Add(obj, Inventory.Container.Itens[i]);
        }
        prebapSizeDelta = InventoryPrefab.GetComponent<RectTransform>().sizeDelta;
    }

    protected Vector3 GetSlotPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItems * (i % numberOfColumns)), yStart + (-ySpaceBetweenItems * (i / numberOfColumns)), 0f);
    }
}
