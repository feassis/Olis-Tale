using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Invetory/Regular Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabeseObject database;
    public Inventory Container;
    public float WeigthCapacity;
    public InventorySlot[] GetSlots { get { return Container.Itens; } }
    [SerializeField] private string savePath;

    public bool AddItem(Item item, int amount)
    {
        int AmountToAdd = amount;
        if (EmptySlotCount <= 0 && !item.IsStackable)
        {
            Debug.Log("Don't have Space");
            return false;
        }

        List<InventorySlot> slots = FindAllSlotsOfOneItem(item);

        foreach (InventorySlot slot in slots)
        {
            if (slot.Amount + AmountToAdd > database.GetItem[item.Id].StackAmount)
            {
                AmountToAdd -= database.GetItem[item.Id].StackAmount - slot.Amount;
                slot.UpdateSlot(item, database.GetItem[item.Id].StackAmount);

            }
            else
            {
                slot.UpdateSlot(item, slot.Amount + AmountToAdd);
                AmountToAdd = 0;

            }
            if (slot.Amount == 0)
            {
                slot.RemoveItem();
            }
        }

        while (AmountToAdd > 0)
        {
            if (AmountToAdd > database.GetItem[item.Id].StackAmount)
            {
                SetFirstEmptySlot(item, database.GetItem[item.Id].StackAmount);
                AmountToAdd -= database.GetItem[item.Id].StackAmount;
            }
            else
            {
                SetFirstEmptySlot(item, AmountToAdd);
                AmountToAdd = 0;
            }
        }

        return true;
    }

    public List<InventorySlot> FindAllSlotsOfOneItem(Item item)
    {
        List<InventorySlot> list = new List<InventorySlot>();
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item.Id == item.Id)
            {
                list.Add(GetSlots[i]);
            }
        }
        return list;
    }
    public InventorySlot FindItemOnInventory(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item.Id == item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Container.Itens.Length; i++)
            {
                if(Container.Itens[i].Item.Id < 0)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot SetFirstEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < Container.Itens.Length; i++)
        {
            if(Container.Itens[i].Item.Id < 0)
            {
                Container.Itens[i].UpdateSlot(item, amount);
                return Container.Itens[i];
            }
        }

        //setUpFunctionality when inventory is full
        return null;
    }

    public bool SwapItem(InventorySlot Item1, InventorySlot Item2)
    {
        if (Item2.CanPlaceInSlot(Item1.ItemObject) && Item1.CanPlaceInSlot(Item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(Item2.Item, Item2.Amount);

            Item2.UpdateSlot(Item1.Item, Item1.Amount);
            Item1.UpdateSlot(temp.Item, temp.Amount);
            return true;
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < Container.Itens.Length; i++)
        {
            if(Container.Itens[i].Item == item)
            {
                Container.Itens[i].UpdateSlot(new Item(), 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();

        Debug.Log("Saved");
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);

            for (int i = 0; i < Container.Itens.Length; i++)
            {
                Container.Itens[i].UpdateSlot(newContainer.Itens[i].Item, newContainer.Itens[i].Amount);
            }

            stream.Close();

            Debug.Log("Loaded");
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Itens = new InventorySlot[16];

    public void Clear()
    {
        for (int i = 0; i < Itens.Length; i++)
        {
            Itens[i].RemoveItem();

        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedTypes = new ItemType[0];
    [System.NonSerialized]
    public UserInterfaceGraphics Parent;
    public Item Item;
    public int Amount;

    public InventorySlot()
    {
                Item = null;
        Amount = 0;
    }

    public InventorySlot(Item item, int amount)
    {
        UpdateSlot(item, amount);
    }

    public ItemObject ItemObject
    {
        get
        {
            if(Item.Id >= 0)
            {
                return Parent.Inventory.database.GetItem[Item.Id];
            }
            return null;
        }
    }

    public void UpdateSlot(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public void RemoveItem()
    {
        Item = new Item();
        Amount = 0;
    }

    public int AddItem(int amount)
    {
        if(Item.IsStackable && Amount + amount <= Item.StackAmount)
        {
            Amount += amount;
            return 0;
        }

        int amountToReturn = Amount + amount - Item.StackAmount;
        Amount = Item.StackAmount;
        return amountToReturn;
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if(AllowedTypes.Length <= 0 || itemObject == null || itemObject.Data.Id < 0)
        {
            return true;
        }

        for (int i = 0; i < AllowedTypes.Length; i++)
        {
            if (itemObject.Type == AllowedTypes[i])
            {
                return true;
            }
        }

        return false;
    }
}
