using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellerNPC : MonoBehaviour
{
    [SerializeField] private GameObject SellerUI;
    [SerializeField] private SellerInterfaceGraphics sellScreen;

    public void ToggleUI()
    {
        SellerUI.SetActive(!SellerUI.activeSelf);
        StartCoroutine(sellScreen.EvaluateMyGoods());
    }

    private void OnApplicationQuit()
    {
        sellScreen.Inventory.Clear();
    }
}
