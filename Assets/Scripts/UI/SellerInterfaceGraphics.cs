using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SellerInterfaceGraphics : DynamicInterfaceGraphics
{
    [SerializeField] private List<ProductMarketPrice> SellersCatalog;
    [SerializeField] private TextMeshProUGUI AmountOnCartTExt;
    [SerializeField] private Button sellButton;

    private int _amountOnCart;
    private int amountOnCart 
    { 
        get => _amountOnCart;
        set
        {
            _amountOnCart = value;
            AmountOnCartTExt.text = _amountOnCart.ToString();
        }
    }

    private void Awake()
    {
        sellButton.onClick.AddListener(SellItems);
    }

    public IEnumerator EvaluateMyGoods()
    {
        while (gameObject.activeSelf)
        {
            amountOnCart = EvaluateGoods();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SellItems()
    {
        int amount = amountOnCart;
        RemoveItems();
        amountOnCart = EvaluateGoods();
        PlayerManager.instance.EarnMoney(amount);
    }

    private int EvaluateGoods()
    {
        int goodValue = 0;

        for (int i = 0; i < slotsOnInterface.Count; i++)
        {
            int id = slotsOnInterface.ElementAt(i).Value.Item.Id;
            goodValue += GetItemPrice(id) * slotsOnInterface.ElementAt(i).Value.Amount;
        }

        return goodValue;
    }

    private int GetItemPrice(int id)
    {
        foreach (ProductMarketPrice item in SellersCatalog)
        {
            if(id == item.Item.Data.Id)
            {
                return item.Price;
            }
        }

        return 0;
    }

    private void RemoveItems()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in slotsOnInterface)
        {
            slot.Value.RemoveItem();
        }
    }
}

[System.Serializable]
public class ProductMarketPrice
{
    public ItemObject Item;
    public int Price;
}
