using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            case ItemType.potion:       return ItemAssets.Instance.potion;
            case ItemType.food:         return ItemAssets.Instance.food;
            case ItemType.shiny:        return ItemAssets.Instance.shiny;
             // syntax is case ItemType.<item name from enum>: return ItemAssets.Instance.<item name from ItemAssets>;
        }
    }
}
