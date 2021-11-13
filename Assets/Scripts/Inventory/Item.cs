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

    }

    public ItemType itemType;
    public int amount;

    //returns the weight of the entire stack of inventory items
    public double getStackWeight()
    {
        switch (itemType)
        {
            //stackable
            default:
            case ItemType.potion:
                return 1.0 * this.amount;
            //not stackable
            case ItemType.food:
            case ItemType.shiny:
                return 2.0 * this.amount;
        }

    }

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            //returns the correct sprite for the object, take from ItemAssets
            case ItemType.potion:
                return ItemAssets.Instance.potionSprite;
            case ItemType.food:
                return ItemAssets.Instance.foodSprite;
            case ItemType.shiny:
                return ItemAssets.Instance.shinySprite;
                // syntax is case ItemType.<item name from enum>: return ItemAssets.Instance.<item name from ItemAssets>;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            //stackable
            default:
            case ItemType.potion:
                return true;
            //not stackable
            case ItemType.food:
            case ItemType.shiny:
                return false;
        }

    }
}
