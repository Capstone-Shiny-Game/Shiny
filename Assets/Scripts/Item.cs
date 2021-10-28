using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType { 
        potion,
        food,
        shiny,
    
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite() {
        switch (itemType) {
            default:
                //returns the correct sprite for the object, take from ItemAssets
            case ItemType.potion:       return ItemAssets.Instance.potionSprite;
            case ItemType.food:         return ItemAssets.Instance.foodSprite;
            case ItemType.shiny:        return ItemAssets.Instance.shinySprite;
             // syntax is case ItemType.<item name from enum>: return ItemAssets.Instance.<item name from ItemAssets>;
        }
    }
}
