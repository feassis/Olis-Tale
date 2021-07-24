using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database",menuName = "Inventory System/Items/Database")]
public class ItemDatabeseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public List<ItemObject> Items;
    public Dictionary<int, ItemObject> GetItem;

    public void OnAfterDeserialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].Data.Id = i;
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}
