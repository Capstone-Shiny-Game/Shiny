using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public struct Offset
    {
        public float forward;
        public float up;
    }
    private FlightController flightController;
    private WalkingController walkingController;
    private CameraController cameraController;
    //public GameObject NPCUI;
    public GameObject ControllerUI;
    private InputAction walkAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/F");
    private InputAction dropItemAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/G");
    private InputAction rotateInventoryAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/R");

    private Inventory inventory;
    [SerializeField]
    private UI_inventory uiInventory; //this variable holds the ui_inventory object from the scene

    //specifies where inventory item should appear when dropped by player
    public Offset walkingOffset = new Offset { forward = 0, up = 0 };
    public Offset flyingOffset = new Offset { forward = 0, up = 0 };
    public double maxCarryWeight = 0;

    private GroundDetector groundDetector;

    private void Start()
    {
        flightController = GetComponent<FlightController>();
        walkingController = GetComponent<WalkingController>();
        cameraController = GetComponent<CameraController>();
        StartFlight();
        StopWalk();

        groundDetector = GetComponent<GroundDetector>() ?? gameObject.AddComponent<GroundDetector>();

        //inventory initialization
        inventory = new Inventory();
        // TODO : replace with GameObjects in the scene that have the attached scripts
        if (SceneManager.GetActiveScene().name == "GymItems")
        {
            uiInventory.SetInventory(inventory);

            float yPos = 10.3f;
            ItemWorld.SpawnItemWorld(new Vector3(40f, yPos, 50f), new Item(Item.ItemType.shiny));
            ItemWorld.SpawnItemWorld(new Vector3(40f, yPos, 40f), new Item(Item.ItemType.food));
            ItemWorld.SpawnItemWorld(new Vector3(40f, yPos, 30f), new Item(Item.ItemType.potion));
        }
    }

    private void toggleFlight()
    {
        if (walkingController.enabled)
        {
            Vector3 pos = transform.position;
            pos.y += 10;
            transform.position = pos;
            StartFlight();
            StopWalk();
            cameraController.isWalking = false;
        }
        else if (flightController.enabled)
        {
            StopFlight();
            StartWalk();
            cameraController.isWalking = true;
        }
    }

    /// <summary>
    /// Drops the first item in the inventory.
    /// </summary>
    private void dropItLikeItsHot()
    {
        if (inventory.itemList.Count == 0)
        {
            return;
        }
        if (walkingController.enabled)
        {
            Vector3 offset = this.transform.forward * walkingOffset.forward;
            offset.y = walkingOffset.up;
            inventory.DropItem(this.transform.position + offset, inventory.itemList[0]);
        }
        else if (flightController.enabled)
        {
            Vector3 offset = this.transform.forward * flyingOffset.forward;
            offset.y = flyingOffset.up;
            inventory.DropItem(this.transform.position + offset, inventory.itemList[0]);
        }
    }
    /// <summary>
    /// Rotates the inventory to the left by one
    /// </summary>
    private void RotateInventory()
    {
        inventory.RotateItems();
    }

    private void OnEnable()
    {
        NPCInteraction.OnNPCInteractEvent += EnterNPCDialogue;

        walkAction.performed += ctx => toggleFlight();
        walkAction.Enable();

        dropItemAction.performed += ctx => dropItLikeItsHot();
        dropItemAction.Enable();

        rotateInventoryAction.performed += ctx => RotateInventory();
        rotateInventoryAction.Enable();
    }

    private void OnDisable()
    {
        NPCInteraction.OnNPCInteractEvent -= EnterNPCDialogue;

        walkAction.Disable();
        dropItemAction.Disable();
        rotateInventoryAction.Disable();
    }

    private void StartFlight() => flightController.enabled = true;
    private void StopFlight() => flightController.enabled = false;
    private void StartWalk() => walkingController.enabled = true;
    private void StopWalk() => walkingController.enabled = false;

    private void SetFixedPosition(Vector3 position) => this.transform.position = position;

    public void ResetToWalk()
    {
        //StopFlight();
        //StartWalk();
        cameraController.isWalking = true;
    }

    private void EnterNPCDialogue()
    {
        //call ui
        //NPCUI.SetActive(true);
        ControllerUI.SetActive(false);
        //bind "ok" button to start walk
    }

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
            if (SceneManager.GetActiveScene().name == "WalkingTest" || SceneManager.GetActiveScene().name.Contains("Gym"))
            {
                flightController.speed = 10.0f;
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
        // TODO (Jakob) : the NPC also registers this event - figure out how to consolidate
        else if (other.CompareTag("NPC"))
        {
            StopFlight();
            StopWalk();
            Vector3 npcFront = other.gameObject.transform.position + other.transform.forward * 4.0f;
            SetFixedPosition(npcFront);
            TryPlaceOnGround();
        }
        //add items to inventory
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            //touching item
            //Debug.Log(itemWorld.GetItem().GetType());
            Item item = itemWorld.GetItem();
            if (maxCarryWeight >= (inventory.weight + item.getStackWeight()))
            { //check if picking this up would add to much weight
                if (inventory.AddItem(item))
                {
                    itemWorld.DestroySelf();
                }
            }
            //Debug.Log(inventory.GetWeight());
        }
    }

    private void TryPlaceOnGround()
    {
        if (groundDetector.FindGround() is Vector3 groundPos)
        {
            // TODO : cleanup
            groundPos.y += walkingController.HeightOffset;
            SetFixedPosition(groundPos);
        }
    }
}
