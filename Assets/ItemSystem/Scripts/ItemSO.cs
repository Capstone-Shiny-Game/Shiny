using UnityEngine;

public class ItemSO : ScriptableObject
{
    [field: SerializeField] public string itemType { get; set; }
    [field: SerializeField] public float weight { get; set; }
    [field: SerializeField] public bool stackable { get; set; }
    [field: SerializeField] public Sprite sprite { get; set; }
    [field: SerializeField] public GameObject prefab { get; set; }
}