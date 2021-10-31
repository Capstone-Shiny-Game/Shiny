using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private FlightController flightController;
    private WalkingController walkingController;

    private Inventory inventory;
    [SerializeField] private UI_inventory uiInventory; //this variable holds the ui_inventory object from the scene

    private void Start()
    {
        flightController = GetComponent<FlightController>();
        walkingController = GetComponent<WalkingController>();
        StartFlight();
        StopWalk();
        //inventory initialization
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

        // TODO : replace with GameObjects in the scene that have the attached scripts
        if (SceneManager.GetActiveScene().name == "Gym") 
        {
            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 253.2f), new Item { itemType = Item.ItemType.shiny, amount = 1 });
            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 301.4f), new Item { itemType = Item.ItemType.food, amount = 1 });
            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 339.4f), new Item { itemType = Item.ItemType.potion, amount = 1 });
            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 359.4f), new Item { itemType = Item.ItemType.potion, amount = 1 });
            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 379.4f), new Item { itemType = Item.ItemType.potion, amount = 1 });
        }
    }

    private void OnEnable()
    {
        // register the function for the collision event
        //NPCInteraction.OnPlayerCollided += SetFixedPosition;
    }

    private void OnDisable()
    {
        // degregister the function
        //NPCInteraction.OnPlayerCollided -= SetFixedPosition;
    }
    
    private void StartFlight() => flightController.enabled = true;
    private void StopFlight() => flightController.enabled = false;
    private void StartWalk() => walkingController.enabled = true;
    private void StopWalk() => walkingController.enabled = false;

    private void SetFixedPosition(Vector3 position) => this.transform.position = position;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BOUNCE");
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Vector3 norm = collision.GetContact(0).normal;
            StartCoroutine(flightController.BounceOnCollision(norm));
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ring") && !flightController.isBoost)
        {
            Debug.Log("RING2");
            Transform targetRing = other.gameObject.transform;
            flightController.SetTargetRing(targetRing);
            //transform.LookAt(targetRing);
            StartCoroutine(flightController.Boost());

        }
        else if (other.CompareTag("Terrain"))
        {
            // TODO (Ella) : This is evil. 
            if (SceneManager.GetActiveScene().name == "WalkingTest")
            {
                StopFlight();
                StartWalk();
            }
            else
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y + 5f,
                    transform.position.z);

                StartCoroutine(flightController.Slow());
            }
        }
        else if (other.CompareTag("NPC"))
        {
            StopFlight();
            StopWalk();
            Vector3 npcFront = other.gameObject.transform.position + other.transform.forward * 3.0f;
            SetFixedPosition(npcFront);
        }
        //add items to inventory
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            //touching item
            Debug.Log(itemWorld.GetItem().GetType());
            //TODO : add weights here
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();

        }
    }
}
