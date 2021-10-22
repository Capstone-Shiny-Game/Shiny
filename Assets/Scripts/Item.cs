using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType { 
        potion,
        food,
        shiney,
    
    }

    public ItemType itemType;
    public int amount;
}
