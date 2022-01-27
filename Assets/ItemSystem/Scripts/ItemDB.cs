using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemDB : ScriptableObject
{
    [field: SerializeField] public SerializableDictionary<string, ItemSO> items { get; set; }
    public void Initialize()
    {
        items = new SerializableDictionary<string, ItemSO>();
    }
}