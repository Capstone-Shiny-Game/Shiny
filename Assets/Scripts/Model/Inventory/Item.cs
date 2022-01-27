using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        potion,
        food,
        shiny,
        apple,
        honey,
        title,
        acorn,
    }

    public ItemType itemType;
    public int amount;
    public Sprite sprite;
    public GameObject prefab;//TODO fix this when saving, if object doesnt exist or has been destoryed get missing reference
    public Item(ItemType itemType, int amount)
    {
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
        // switch (itemType)
        // {
        //     //stackable
        //     default:
        //     case ItemType.potion:
        //         return 1.0 * this.amount;
        //     //not stackable
        //     case ItemType.food:
        //     case ItemType.shiny:
        //         return 2.0 * this.amount;
        // }

    }

    public Sprite GetSprite()
    {
        return sprite;

        // switch (itemType)
        // {
        //     default:
        //     //returns the correct sprite for the object, take from ItemAssets
        //     case ItemType.potion:
        //         return ItemAssets.Instance.potionSprite;
        //     case ItemType.food:
        //         return ItemAssets.Instance.foodSprite;
        //     case ItemType.shiny:
        //         return ItemAssets.Instance.shinySprite;
        //         // syntax is case ItemType.<item name from enum>: return ItemAssets.Instance.<item name from ItemAssets>;
        // }
    }

    public bool IsStackable()
    {
        return false;
        // switch (itemType)
        // {
        //     //stackable
        //     default:
        //     case ItemType.potion:
        //         return true;
        //     //not stackable
        //     case ItemType.food:
        //     case ItemType.shiny:
        //         return false;
        // }

    }
}
