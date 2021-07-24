using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public InventoryObject CartInventory;
    public GameObject InventoryUI;
    public GameObject EquipmentUI;


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GroundItem item = hit.gameObject.GetComponent<GroundItem>();
        if (item)
        {
            bool succed = CartInventory.AddItem(new Item(item.MyItem), 1);
            if (succed)
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void ToggleInventoryInterface()
    {
        InventoryUI.SetActive(!InventoryUI.activeSelf);
        EquipmentUI.SetActive(!EquipmentUI.activeSelf);
    }

    public void SaveInventory()
    {
        Debug.Log("Save");
        CartInventory.Save();
    }

    public void LoadInventory()
    {
        Debug.Log("Load");
        CartInventory.Load();
    }

    private void OnApplicationQuit()
    {
        CartInventory.Container.Clear();
    }
}
