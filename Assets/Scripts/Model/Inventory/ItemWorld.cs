using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(GameObject spawnedObject, Vector3 position)
    {

        Transform transform = Instantiate(spawnedObject.transform, position, Quaternion.identity);

        transform.GetComponent<GroundDetector>().FindGround(out Vector3 groundPos, out bool isWater);
        if (groundPos != null)
        {
            Debug.Log("GroundPos: " + groundPos);
            transform.position = groundPos - new Vector3(0, transform.localScale.y / 2, 0);
            transform.position = transform.position + new Vector3(0, 1, 0);
        }
        Destroy(transform.GetComponent<GroundDetector>());

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
            if (other.GetComponent<PlayerController>().inventory.AddItem(item))
            {
                DestroySelf();
            }
        }
           
         //   }
        
    }
}
