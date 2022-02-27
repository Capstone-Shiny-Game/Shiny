using UnityEngine;

public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string itemType { get; set; }
    [field: SerializeField] public float weight { get; set; }
    [field: SerializeField] public bool stackable { get; set; }
    [field: SerializeField] public Sprite sprite { get; set; }
    [field: SerializeField] public GameObject prefab { get; set; }

    public override string ToString()
    {
        return itemType.ToString();
    }

    public override bool Equals(object other)
    {
        return other is ItemSO && ToString().Equals(other.ToString());
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
}
