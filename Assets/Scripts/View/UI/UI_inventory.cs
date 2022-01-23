using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_inventory : MonoBehaviour
{
    private Inventory inventory;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public float itemSlotCellSize = 30f;
    public float selectedItemSize = 60f;
    public float nextItemSize = 30f;
    public int slotsPerRow = 4;

    public void SetInventory(Inventory inv)
    {
        if (!(inventory is null)) {
            inventory.onItemListChanged -= Inventory_OnItemListChanged;
        }
        inventory = inv;

        inventory.onItemListChanged += Inventory_OnItemListChanged;
        if (this.enabled == true)
        {
            RefreshInventoryItems();
        }
    }



    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        if (this.enabled == true)
        {
            RefreshInventoryItems();
        }
    }

    public static void fillTemplate(RectTransform templateRectTransform, Item item) {
        templateRectTransform.gameObject.SetActive(true);
        // find and set item image
        Image image = templateRectTransform.Find("itemImage").GetComponent<Image>();
        image.sprite = item.GetSprite();
        // find and set text to display amount of items
        TextMeshProUGUI uiText = templateRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
        if (item.amount > 1)
        {
            uiText.SetText(item.amount.ToString());
        }
        else
        {
            uiText.SetText("");
        }
    }

    private void RefreshInventoryItems()
    {
        ClearInventory();

        int x = 0;
        int y = 0;
        foreach (Item item in inventory.itemList)
        {
            // instantiate template
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();

            fillTemplate(itemSlotRectTransform, item);
            // item slots in grid array
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
            // go to next position
            x++;
            // reached end of row, return to start
            if (x >= slotsPerRow)
            {
                x = 0;
                y++;
            }
        }
    }

    /// <summary>
    /// Clears out all instantiated sprites
    /// </summary>
    private void ClearInventory()
    {
        // destroy old ui elements to avoid duplicates
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate)
            {
                // don't destroy the template
                continue;
            }
            Destroy(child.gameObject);
        }
    }
}
