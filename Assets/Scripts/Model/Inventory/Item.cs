using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    [field: NonSerialized] public static ItemDB itemDB;
    //other item variables are in itemSO 
    //see itemTypeDropdown.cs for why this is hidden
    [HideInInspector] public string itemType;
    [Range(1, 6)] public int amount;
    public static void SetItemDB() {
        //TODO maybe change this to it's own load function
        itemDB = CreateItemWindow.LoadAsset<ItemDB>(CreateItemWindow.path, CreateItemWindow.databaseName);
        Debug.Log(itemDB.items["apple"].sprite.name);
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
        return 0;//TODO re-add this
        //return itemDB.items[itemType].weight * this.amount;
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
        return false; //TODO add stackability to items in create item window
        //return itemDB.items[itemType].stackable;
    }

}

