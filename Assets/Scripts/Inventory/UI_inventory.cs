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
        inventory = inv;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshHotbarItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        if (this.enabled == true)
        {
            RefreshHotbarItems();
        }
    }

    private void RefreshInventoryItems()
    {
        return;
    }

    private void RefreshHotbarItems()
    {
        ClearInventory();

        for (int i = 0; i < Math.Min(inventory.itemList.Count, 2); i++)
        {
            Item item = inventory.itemList[i];

            // instantiate template
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            // find and set item image
            Image image = itemSlotRectTransform.Find("itemImage").GetComponent<Image>();
            image.sprite = item.GetSprite();
            // find and set text to display amount of items
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }
            // item slots in grid array
            itemSlotRectTransform.anchoredPosition = new Vector2(i * itemSlotCellSize, 0f);
            // go to next position

        }

        UpdateUISelection();

    }

    // private void RefreshInventoryItems()
    // {
    //     ClearInventory();

    //     int x = 0;
    //     int y = 0;
    //     for (int i = 0; i < inventory.itemList.Count; i++)
    //     {
    //         if (i >= 2)
    //         {
    //             break;
    //         }
    //         Item item = inventory.itemList[i];

    //         // instantiate template
    //         RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
    //         itemSlotRectTransform.gameObject.SetActive(true);
    //         // find and set item image
    //         Image image = itemSlotRectTransform.Find("itemImage").GetComponent<Image>();
    //         image.sprite = item.GetSprite();
    //         // find and set text to display amount of items
    //         TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
    //         if (item.amount > 1)
    //         {
    //             uiText.SetText(item.amount.ToString());
    //         }
    //         else
    //         {
    //             uiText.SetText("");
    //         }
    //         // item slots in grid array
    //         itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
    //         // go to next position
    //         x++;
    //         // reached end of row, return to start
    //         if (x >= slotsPerRow)
    //         {
    //             x = 0;
    //             y++;
    //         }
    //     }
    //     UpdateUISelection();
    // }

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
