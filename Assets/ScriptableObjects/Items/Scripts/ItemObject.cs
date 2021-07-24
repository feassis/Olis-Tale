using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : ScriptableObject
{
    public string Name;
    public Sprite UIDisplay;
    public ItemType Type;
    public float Weight;
    public bool IsStackable;
    public int StackAmount;
    public Item Data = new Item();

    [TextArea(15,20)]
    public string Description;

    public Item CreateItem()
    {
        return new Item(this);
    }
}

public enum ItemType
{
    Helmet,
    Belt,
    Trinket,
    Food,
    MonsterMaterial,
    Potion
}

public enum Atributes
{
    Attack,
    Defense,
    HP
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public bool IsStackable;
    public int StackAmount;
    public List<ItemBuff> Buffs = new List<ItemBuff>();

    public Item()
    {
        Name = "";
        Id = -1;
    }

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Data.Id;
        IsStackable = item.IsStackable;
        StackAmount = item.StackAmount;

        for (int i = 0; i < item.Data.Buffs.Count; i++)
        {
            Buffs.Add(new ItemBuff(item.Data.Buffs[i].Min, item.Data.Buffs[i].Max));
            Buffs[i].Atribute = item.Data.Buffs[i].Atribute;
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Atributes Atribute;
    public int Value;
    public int Min;
    public int Max;

    public ItemBuff(int min, int max)
    {
        Min = min;
        Max = max;
        GenerateValue();
    }

    public void GenerateValue()
    {
        Value = Random.Range(Min, Max);
    }
}