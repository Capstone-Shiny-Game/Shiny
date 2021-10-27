using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void SetInventory(Inventory inv) {
        inventory = inv;
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems() {
        int x = 0;
        int y = 0;
        
        foreach (Item item in inventory.GetItemList()) {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();//instantiate template
            itemSlotRectTransform.gameObject.SetActive(true);
            //find and set item image
            Image image = itemSlotRectTransform.Find("itemImage").GetComponent<Image>();
            image.sprite = item.GetSprite();
            //item slots in grid array
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            x++;//go to next position
            //reached end of row, return to start
            if (x > slotsPerRow) {
                x = 0;
                y++;
            }
            
        }

    }
}
