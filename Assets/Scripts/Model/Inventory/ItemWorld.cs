using System;
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
        transform.position = transform.FindGround(1);
    }

    public static ItemWorld SpawnItemWorld(Item itemSpawned, Vector3 position, bool SpawnOnGround = false)
    {
        if (itemSpawned is null) {
            Debug.Log("tried to drop item that was null");
            return null;
        }
        GameObject spawnedItem = Instantiate(itemSpawned.GetPrefab(), position, Quaternion.identity);
        spawnedItem.SetActive(false);
        Transform transform = spawnedItem.transform;
        if (SpawnOnGround)
        {
            PutItemOnGround(transform, position);
        }
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.item = itemSpawned;
        spawnedItem.SetActive(true);
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
        if (item is null) 
        {
            Debug.Log("tried to pickup item that was null");
            return;
        }
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().inventory.AddItem(item))
            {
                DestroySelf();
            }
        }
        
    }
}
