using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    
    public Item item;
    internal void DestroySelf()
    {
        Destroy(gameObject);
    }

    public static void PutItemOnGround(Transform transform, Vector3 position)
    {

        transform.GetComponent<GroundDetector>().FindGround(out Vector3 groundPos, out bool isWater);
        if (groundPos != null)
        {
            Debug.Log("GroundPos: " + groundPos);
            transform.position = groundPos - new Vector3(0, transform.localScale.y / 2, 0);
            transform.position = transform.position + new Vector3(0, 1, 0);
        }
        Destroy(transform.GetComponent<GroundDetector>());

    }
    //TODO REMOVE this it is depreciated
    public static ItemWorld SpawnItemWorld(GameObject prefab, Vector3 position, bool SpawnOnGround = true)
    {
        Transform transform = Instantiate(prefab, position, Quaternion.identity).transform;
        if (SpawnOnGround)
        {
            PutItemOnGround(transform, position);
        }
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.item = new Item(itemWorld.item.itemType, itemWorld.item.amount);
        return itemWorld;
    }

    public static ItemWorld SpawnItemWorld(Item itemSpawned, Vector3 position, bool SpawnOnGround = false)
    {
        Transform transform = Instantiate(itemSpawned.GetPrefab(), position, Quaternion.identity).transform;
        if (SpawnOnGround)
        {
            PutItemOnGround(transform, position);
        }
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.item = itemSpawned;
        return itemWorld;
    }

    private void OnTriggerEnter(Collider other)
    {
        ////add items to inventory
        //ItemWorld itemWorld = other.GetComponent<ItemWorld>();

        //touching item
        //Item item = itemWorld.GetItem();
        //maxcarryweight is on player
        //    if (maxCarryWeight >= (inventory.weight + item.getStackWeight()))
        //    {
        //check if picking this up would add to much weight
        if (other.CompareTag("Player"))
        {
            Debug.Log("helpful place");
            if (other.GetComponent<PlayerController>().inventory.AddItem(item))
            {
                Debug.Log("happy place");
                DestroySelf();
            }
        }
        
    }
}
