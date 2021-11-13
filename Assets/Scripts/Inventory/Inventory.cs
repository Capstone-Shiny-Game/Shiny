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
            bool itemAlreadyInInv = false;
            foreach (Item invItem in itemList)
            {
                if (invItem.itemType == item.itemType)
                {
                    invItem.amount += 1;
                    itemAlreadyInInv = true;
                }
            }
            if (!itemAlreadyInInv)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
