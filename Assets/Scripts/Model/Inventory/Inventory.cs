using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [field: NonSerialized] public event EventHandler onItemListChanged;

    [field: SerializeField, HideInInspector] public List<Item> itemList { get; private set; }
    public int maxItemCount = 20;
    [field: SerializeField, HideInInspector] public double weight { get; private set; }
    [field: SerializeField, HideInInspector] public int selectionIndex { get; private set; }


    /// <summary>
    /// Constructor for inventory
    /// </summary>
    public Inventory()
    {
        itemList = new List<Item>();
        weight = 0;
        selectionIndex = 0;
    }
    /// <summary>
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="item"> The item to add to the inventory </param>
    /// <returns> True when the item is successfully added and false otherwise </returns>
    public bool AddItem(Item item)
    {
        //Debug.Log(item.itemType);
        //Debug.Log(item.amount);
        if (item.IsStackable())
        {
            Item inventoryItem = GetItemFromList(item);
            if (inventoryItem is null)
            {
                //itemList.Add(item);
                itemList.Insert(0, item);
            }
            else
            {
                itemList.Remove(inventoryItem);
                inventoryItem.amount += item.amount;
                itemList.Insert(0, inventoryItem);
            }
        }
        else
        {
            if (itemList.Count == maxItemCount)
            {
                return false;
            }
            //itemList.Add(item);
            itemList.Insert(0, item);

        }
        weight += item.getStackWeight();
        // Update UI
        onItemListChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Rotates the list of items by one to the left. Allows the player to choose
    /// which item they want to drop.
    /// </summary>
    public void RotateItems()
    {
        Item first = itemList[0];
        itemList.RemoveAt(0);
        itemList.Add(first);
        onItemListChanged?.Invoke(this, EventArgs.Empty);
    }


    /// <summary>
    /// changes the currently selected item 1 slot to the right, wraps around if out of bounds
    /// </summary>
    public void MoveSelectionRight()
    {
        selectionIndex++;
        NormalizeSelection();
        // Update UI (can be optimized if needed by adding another event for updating only selection)
        onItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// makes sure the selecion is within bounds of the inventory
    /// </summary>
    private void NormalizeSelection()
    {
        if (itemList.Count == 0)
        {
            selectionIndex = 0;
            return;
        }
        selectionIndex %= itemList.Count;
    }

    /// <summary>
    /// returns the currently selected item
    /// </summary>
    public Item GetSelectedItem()
    {
        return itemList[selectionIndex];
    }

    /// <summary>
    /// Drops a specific item from the inventory.
    /// </summary>
    /// <param name="item"> The item to be dropped </param>
    /// <param name="dropAmount"></param>
    /// <param name="dropPosition"> The position at which to drop the item</param>
    /// <returns> True when the item is successfully dropped and false otherwise </returns>
    public bool DropItem(Vector3 dropPosition, Item item, int dropAmount = 1)
    {
        return RemoveItem(item, dropAmount, true, dropPosition);
    }

    /// <summary>
    /// Removes an item from the inventory. Optionally drops the item in space.
    /// </summary>
    /// <param name="item"> The item to be removed </param>
    /// <param name="removeAmount"> The amount to be removed (for stackable items) </param>
    /// <param name="dropItem"> If true, drops the item in space </param>
    /// <param name="dropPosition"> Where the item should be dropped if `dropItem` is true </param>
    /// <returns></returns>
    public bool RemoveItem(Item item,
                           int removeAmount = 1,
                           bool dropItem = false,
                           Vector3 dropPosition = new Vector3())
    {
        //Debug.Log("drop : "+item.amount);
        Item inventoryItem = GetItemFromList(item);
        if (inventoryItem is null)
        {
            Debug.LogWarning("Tried to drop object that doesn't exist in DropItem within Inventory.cs");
            return false;
        }
        if (removeAmount == 0)
        {
            Debug.LogWarning("Tried to remove zero objects from inventory within Inventory.cs");
            return false;
        }
        Item itemToSpawn = new Item(item.itemType, removeAmount);
        if (!item.IsStackable())
        {
            weight -= item.getStackWeight();
            itemList.Remove(item);
            if (dropItem)
            {
                
                ItemWorld.SpawnItemWorld(itemToSpawn, dropPosition);
            }
        }
        else
        {
            if (inventoryItem.amount < removeAmount)
            {
                Debug.LogWarning($"Unable to drop {item.amount} items");
                return false;
            }
            weight -= item.getStackWeight();
            if (dropItem)
            {
                ItemWorld.SpawnItemWorld(itemToSpawn, dropPosition);
            }
            inventoryItem.amount -= removeAmount;
            if (inventoryItem.amount <= 0)
            {
                itemList.Remove(inventoryItem);
            }
        }
        NormalizeSelection();
        // Update UI
        onItemListChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Searches the inventory's item list and returns a reference to the item
    /// if it is found.
    /// </summary>
    /// <param name="item"> The item being searched for. </param>
    /// <returns>
    /// The item being searched for in the list, if it is found. Returns
    /// null otherwise.
    /// </returns>
    private Item GetItemFromList(Item item)
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

}
