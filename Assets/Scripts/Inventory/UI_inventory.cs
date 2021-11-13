using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    public float itemSlotCellSize = 30f;
    public int slotsPerRow = 4;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate"); ;
    }

    public void SetInventory(Inventory inv)
    {
        inventory = inv;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        //destory old ui elements to avoid duplicates
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate)
            {//dont destory the template
                continue;
            }
            Destroy(child.gameObject);
        }


        int x = 0;
        int y = 0;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();//instantiate template
            itemSlotRectTransform.gameObject.SetActive(true);
            //find and set item image
            Image image = itemSlotRectTransform.Find("itemImage").GetComponent<Image>();
            image.sprite = item.GetSprite();
            //find and set text to display amount of items
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }
            //item slots in grid array
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
            x++;//go to next position
            //reached end of row, return to start
            if (x >= slotsPerRow)
            {
                x = 0;
                y++;
            }

        }

    }
}
