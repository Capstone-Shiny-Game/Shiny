using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    

    public Inventory()
    {
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            Item inventoryItem = GetItemFromList(item);
            if (inventoryItem is null)
            {
                itemList.Add(item);
            }
            else
            {
                inventoryItem.amount += item.amount;
            }
        }
        else
        {
            itemList.Add(item);
        }
        // Update UI
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DropItem(Vector3 dropPosition, Item item)
    {
        ItemWorld.SpawnItemWorld(dropPosition, item);
        if (!item.IsStackable())
        {
            itemList.Remove(item);
        }
        else
        {
            Item inventoryItem = GetItemFromList(item);
            if (inventoryItem is null)
            {
                Debug.LogWarning("Tried to drop object that doesn't exist in DropItem within Inventory.cs");
                return;
            }
            inventoryItem.amount -= item.amount;
            if (inventoryItem.amount <= 0)
            {
                itemList.Remove(item);
            }
        }
        // Update UI
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public Item GetItemFromList(Item item)
    {
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == item.itemType)
            {
                if (inventoryItem.amount <= 0)
                {
                    return inventoryItem;
                }
            }
        }
        return null;
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
