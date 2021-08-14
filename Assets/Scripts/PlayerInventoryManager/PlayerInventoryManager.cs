using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public InventoryObject CartInventory;
    public InventoryObject Equipment;
    public GameObject InventoryUI;
    public GameObject EquipmentUI;
    [SerializeField] private LayerMask npcSeller;
    [SerializeField] private float npcSellerInteractionRange = 3f;


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

    public void TryInteractWithSeller()
    {
        var sellerNCP = GetSellerInRange(transform, npcSellerInteractionRange);
        if(sellerNCP.Length > 0)
        {
            sellerNCP[0].GetComponent<SellerNPC>().ToggleUI();
        }
    }

    private Collider[] GetSellerInRange(Transform pointOfOrigin, float range)
    {
        return Physics.OverlapSphere(pointOfOrigin.position, range, npcSeller);
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
        Equipment.Save();
    }

    public void LoadInventory()
    {
        Debug.Log("Load");
        CartInventory.Load();
        Equipment.Load();
    }

    private void OnApplicationQuit()
    {
        CartInventory.Container.Clear();
        Equipment.Container.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, npcSellerInteractionRange);
    }
}
