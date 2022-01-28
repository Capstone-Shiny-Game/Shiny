using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    [field: NonSerialized] public static ItemDB itemDB;
    //other item variables are in itemSO
    public string itemType;
    public int amount;
    public static void SetItemDB() {
        //TODO maybe change this to it's own load function
        CreateItemWindow.LoadAsset<ItemDB>(CreateItemWindow.path, CreateItemWindow.databaseName);
    }

    public Item(string itemType, int amount)
    {   //TODO check item db for type
        this.amount = amount;
        this.itemType = itemType;
    }
    public Item(string itemType)
    {
        this.amount = 1;
        this.itemType = itemType;
    }

    //returns the weight of the entire stack of inventory items
    public double getStackWeight()
    {
        return itemDB.items[itemType].weight * this.amount;
    }

    public Sprite GetSprite()
    {
        return itemDB.items[itemType].sprite;
    }
    public GameObject GetPrefab()
    {
        return itemDB.items[itemType].prefab;
    }

    public bool IsStackable()
    {
        return itemDB.items[itemType].stackable;
    }
}
