using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(GameObject spawnedObject, Vector3 position)
    {
        Transform transform = Instantiate(spawnedObject.transform, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        //Item item = spawnedObject.GetComponent<Item>();
        //itemWorld.SetItem(item);

        return itemWorld;
    }
    public Item item;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        //TODO: set mesh reder
    }

    internal void DestroySelf()
    {
        gameObject.SetActive(false);
    }

    public Item GetItem()
    {
        return item;
    }
}
