using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotbar : MonoBehaviour
{
    private Inventory inventory;
    public RectTransform currentItemRectTransform;
    public RectTransform nextItemRectTransform;
    public void SetInventory(Inventory inv)
    {
        if (!(inventory is null))
        {
            inventory.onItemListChanged -= Inventory_OnItemListChanged;
        }
        inventory = inv;

        inventory.onItemListChanged += Inventory_OnItemListChanged;
        if (this.enabled == true)
        {
            RefreshHotbarItems();
        }
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        if (this.enabled == true)
        {
            RefreshHotbarItems();
        }
    }

    private void RefreshHotbarItems()
    {
        if (inventory.itemList.Count == 0)
        {
            currentItemRectTransform.gameObject.SetActive(false);
            nextItemRectTransform.gameObject.SetActive(false);
            return;
        }
        else
        {
            Item item = inventory.itemList[0];
            UI_inventory.fillTemplate(currentItemRectTransform, item);
            if (inventory.itemList.Count >= 2)
            {
                item = inventory.itemList[1];
                UI_inventory.fillTemplate(nextItemRectTransform, item);
            }
            else
            {
                nextItemRectTransform.gameObject.SetActive(false);
            }
        }
    }
}
