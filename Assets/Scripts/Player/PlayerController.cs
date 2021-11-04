using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private FlightController flightController;
    private WalkingController walkingController;
    private CameraController cameraController;

    private Inventory inventory;
    [SerializeField] private UI_inventory uiInventory; //this variable holds the ui_inventory object from the scene

    private void Start()
    {
        flightController = GetComponent<FlightController>();
        walkingController = GetComponent<WalkingController>();
        cameraController = GetComponent<CameraController>();
        StartFlight();
        StopWalk();
        //inventory initialization
        inventory = new Inventory();
        // TODO : replace with GameObjects in the scene that have the attached scripts
        if (SceneManager.GetActiveScene().name == "Gym") 
        {
            uiInventory.SetInventory(inventory);

            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 253.2f), new Item { itemType = Item.ItemType.shiny, amount = 1 });
            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 301.4f), new Item { itemType = Item.ItemType.food, amount = 1 });
            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 339.4f), new Item { itemType = Item.ItemType.potion, amount = 1 });
            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 359.4f), new Item { itemType = Item.ItemType.potion, amount = 1 });
            ItemWorld.SpawnItemWorld(new Vector3(-104.9f, 4, 379.4f), new Item { itemType = Item.ItemType.potion, amount = 1 });
        }
    }

    private void Update()
    {
        if (walkingController.enabled && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F)))
        {
            Vector3 pos = transform.position;
            pos.y += 10;
            transform.position = pos;
            StartFlight();
            StopWalk();
            cameraController.isWalking = false;
        }
        else if (flightController.enabled && Input.GetKeyDown(KeyCode.F))
        {
            StopFlight();
            StartWalk();
            cameraController.isWalking = true;
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
            if (SceneManager.GetActiveScene().name == "WalkingTest"||SceneManager.GetActiveScene().name.Contains("Gym"))
            {
                StopFlight();
                StartWalk();
                cameraController.isWalking = true;
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

            TryPlaceOnGround();
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

    private void TryPlaceOnGround()
    {
        Vector3? groundPos = this.gameObject.AddComponent<GroundDetector>().FindGround();
        if (groundPos != null)
        {
            SetFixedPosition((Vector3)groundPos);
        }
        Destroy(this.gameObject.GetComponent<GroundDetector>());
    }
}
