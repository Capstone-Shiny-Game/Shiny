using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    public int maxItemCount = 8;
    private double weight;


    public Inventory()
    {
        itemList = new List<Item>();
        weight = 0;
    }

    public bool AddItem(Item item)
    {
        weight += item.getStackWeight();
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
            if (itemList.Count == maxItemCount)
            {
                return false;
            }
            itemList.Add(item);
        }
        // Update UI
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public bool DropItem(Vector3 dropPosition, Item item, int dropAmount = 1)
    {
        if (!item.IsStackable())
        {
            weight -= item.getStackWeight(); 
            itemList.Remove(item);
            ItemWorld.SpawnItemWorld(dropPosition, new Item(item.itemType, dropAmount));
        }
        else
        {
            Item inventoryItem = GetItemFromList(item);
            if (inventoryItem is null)
            {
                Debug.LogWarning("Tried to drop object that doesn't exist in DropItem within Inventory.cs");
                return false;
            }
            if (inventoryItem.amount < dropAmount)
            {
                Debug.LogWarning($"Unable to drop {item.amount} items");
                return false;
            }
            weight -= item.getStackWeight();
            ItemWorld.SpawnItemWorld(dropPosition, new Item(item.itemType, dropAmount));
            inventoryItem.amount -= dropAmount;
            if (inventoryItem.amount <= 0)
            {
                itemList.Remove(item);
            }
        }
        // Update UI
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public Item GetItemFromList(Item item)
    {
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == item.itemType)
            {
                if (inventoryItem.amount > 0)
                {
                    return inventoryItem;
                }
            }
        }
        return null;
    }

    public double GetWeight()
    {
        return weight;
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
