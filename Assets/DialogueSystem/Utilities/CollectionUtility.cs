using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionUtility
{
    public static void AddItem<K, V>(this SerializableDictionary<K, List<V>> dict, K key, V value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key].Add(value);
            return;
        }

        dict.Add(key, new List<V>() { value });

    }
}
