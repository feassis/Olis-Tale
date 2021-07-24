using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UserInterfaceGraphics : MonoBehaviour
{
    public InventoryObject Inventory;
    public PlayerInventoryManager player;

    protected Vector2 prebapSizeDelta;

    protected Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        for (int i = 0; i < Inventory.Container.Itens.Length; i++)
        {
            Inventory.Container.Itens[i].Parent = this;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnPointerEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnPointerExitInterface(gameObject); });
    }

    private void Update()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in slotsOnInterface)
        {
            if (slot.Value.Item.Id >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = slot.Value.ItemObject.UIDisplay;
                slot.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = slot.Value.Amount <= 1 ? "" : slot.Value.Amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public abstract void CreateSlots();

    public void OnPointerEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }

    public void OnPointerEnterInterface(GameObject obj)
    {
        MouseData.InterfaceMouseIsOver = obj.GetComponent<UserInterfaceGraphics>();
    }

    public void OnPointerExitInterface(GameObject obj)
    {
        MouseData.InterfaceMouseIsOver = null;
    }

    public void OnPointerExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnBeginDrag(GameObject obj)
    {
       MouseData.temItemBeingDrag = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].Item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = prebapSizeDelta;
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.UIDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.temItemBeingDrag != null)
        {
            MouseData.temItemBeingDrag.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void OnEndDrag(GameObject obj)
    {
        Destroy(MouseData.temItemBeingDrag);
        Debug.Log(MouseData.InterfaceMouseIsOver);
        if(MouseData.InterfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.InterfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            Inventory.SwapItem(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
}

public static class MouseData
{
    public static GameObject temItemBeingDrag;
    public static GameObject slotHoveredOver;
    public static UserInterfaceGraphics InterfaceMouseIsOver;
}
