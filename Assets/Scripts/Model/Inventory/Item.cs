using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    [field: NonSerialized] static ItemDB itemDB = new ItemDB();//TODO replace this with a call to find inventory item db
    //TODO tell item to get itemDB
    public enum ItemType//TODO remove this
    {
        potion,
        food,
        shiny,
        apple,
        honey,
        title,
        acorn,
    }

    public ItemType itemType;//TODO change this to string
    public int amount;
    public Sprite sprite; //TODO remove this
    public GameObject prefab;//TODO remove this
    public Item(ItemType itemType, int amount)
    {   //TODO check item db for type
        this.amount = amount;
        this.itemType = itemType;
    }
    public Item(ItemType itemType)
    {
        this.amount = 1;
        this.itemType = itemType;
    }

    //returns the weight of the entire stack of inventory items
    public double getStackWeight()
    {
        return 0.0;
        
        return itemDB.items["itemType"].weight * this.amount;//TODO replace with item type ;

    }

    public Sprite GetSprite()
    {
        return sprite;
        //TODO remove sprite variable
        return itemDB.items["itemType"].sprite;//TODO replace with item type
    }
    public GameObject GetPrefab()
    {
        return prefab;
        //TODO remove prefab variable
        return itemDB.items["itemType"].prefab;//TODO replace with item type
    }

    public bool IsStackable()
    {
        return false;

        return itemDB.items["itemType"].stackable;//TODO replace with item type

    }
}
