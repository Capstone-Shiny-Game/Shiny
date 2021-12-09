using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour, Savable
{
    [System.Serializable]
    public struct Offset
    {
        public float forward;
        public float up;
    }
    public Animator birdAnimator;
    private FlightController flightController;
    private WalkingController walkingController;
    private CameraController cameraController;
    // public GameObject NPCUI;
    public GameObject ControllerUI;

    private InputAction walkAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/F");

    private Inventory inventory;

    // this variable holds the ui_inventory object from the scene
    [SerializeField]
    UI_inventory uiInventory;
    [SerializeField]
    UIHotbar uiHotbar;

    // specifies where inventory item should appear when dropped by player
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

        groundDetector = GetComponent<GroundDetector>();

        walkingController.WalkedOffEdge += () => ToggleFlight(0.5f);

        // inventory initialization
        inventory = new Inventory();
        // TODO : replace with GameObjects in the scene that have the attached scripts
        if (SceneManager.GetActiveScene().name == "GymItems")
        {
            AddSelfToSavablesList();
            uiInventory.SetInventory(inventory);
            uiHotbar.SetInventory(inventory);

            float yPos = 10.3f;
            ItemWorld.SpawnItemWorld(new Vector3(40f, yPos, 50f), new Item(Item.ItemType.shiny));
            ItemWorld.SpawnItemWorld(new Vector3(40f, yPos, 40f), new Item(Item.ItemType.food));
            ItemWorld.SpawnItemWorld(new Vector3(40f, yPos, 30f), new Item(Item.ItemType.potion));
        }
    }

    private void ToggleFlight(float addY = 2)
    {
        if (walkingController.enabled)
        {
            StopWalk();
            Vector3 pos = transform.position;
            pos.y += addY;
            transform.position = pos;
            transform.RotateAround(transform.position, transform.right, -30);
            birdAnimator.SetBool("WalktoFly", true);
            StartFlight();
        }
        else if (flightController.enabled && groundDetector.FindGround(out Vector3 groundPos, out bool isWater))
        {
            flightController.speed = 10.0f;
            StopFlight();
            birdAnimator.SetBool("WalktoFly", false);
            transform.position = groundPos;
            StartWalk();
            walkingController.Splashing = isWater;
        }
    }

    /// <summary>
    /// Drops the first item in the inventory.
    /// </summary>
    public void DropItLikeItsHot()
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
    public void RotateInventory()
    {
        inventory.RotateItems();
    }

    private void OnEnable()
    {
        NPCInteraction.OnNPCInteractEvent += EnterNPCDialogue;
        NPCInteraction.OnNPCInteractEndEvent += ExitNPCDialogue;

        walkAction.performed += ctx => ToggleFlight();
        walkAction.Enable();

    }

    private void OnDisable()
    {
        NPCInteraction.OnNPCInteractEvent -= EnterNPCDialogue;
        NPCInteraction.OnNPCInteractEndEvent -= ExitNPCDialogue;

        walkAction.Disable();
    }

    private void StartFlight()
    {
        flightController.enabled = true;
        cameraController.isWalking = false;
        flightController.ShowTrail();
        birdAnimator.SetBool("isFlying", true);
    }
    private void StopFlight()
    {
        flightController.enabled = false;
        flightController.HideTrail();
        birdAnimator.SetBool("isFlying", false);
    }
    private void StartWalk()
    {
        walkingController.enabled = true;
        cameraController.isWalking = true;
        flightController.HideTrail();
        if (walkingController.Splashing == true)
        {
            birdAnimator.SetBool("isSwim", true);
            birdAnimator.SetBool("isWalking", false);
        }
        else if (walkingController.Splashing == false)
        {
            birdAnimator.SetBool("isSwim", false);
            birdAnimator.SetBool("isWalking", true);
        }

    }
    private void StopWalk()
    {
        walkingController.enabled = false;
        cameraController.isWalking = false;
        flightController.ShowTrail();
        birdAnimator.SetBool("isWalking", false);
        birdAnimator.SetBool("isSwim", false);
    }

    private void SetFixedPosition(Vector3 position) => this.transform.position = position;

    public void ResetToWalk()
    {
        //StopFlight();
        //StartWalk();
        cameraController.isWalking = true;
    }

    private Vector3 positionBeforeDialogue;
    private Quaternion rotationBeforeDialogue;

    private void EnterNPCDialogue(Transform npcTransform)
    {
        StopFlight();
        StopWalk();
        Vector3 npcFront = npcTransform.position + npcTransform.forward * 4.0f;
        positionBeforeDialogue = transform.position;
        rotationBeforeDialogue = transform.rotation;
        SetFixedPosition(npcFront);
        TryPlaceOnGround();
        ControllerUI.SetActive(false);
    }

    private void ExitNPCDialogue()
    {
        ControllerUI.SetActive(true);
        // SetFixedPosition(new Vector3(transform.position.x - 5, transform.position.y, transform.position.z - 5));
        transform.position = positionBeforeDialogue;
        transform.rotation = rotationBeforeDialogue;
        StartWalk();
    }

    // TODO : remove or fold bouncing into method below
    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BOUNCE");
        if (collision.gameObject.CompareTag("Terrain"))
        {
        Vector3 norm = collision.GetContact(0).normal;
        StartCoroutine(flightController.BounceOnCollision(norm));
        }
    }
    */

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
        else if ((other.CompareTag("Terrain") || other.CompareTag("Water")) && flightController.enabled)
        {
            flightController.speed = 10.0f;
            StopFlight();
            StartWalk();
            cameraController.isWalking = true;
        }
        else if (flightController.enabled && !other.isTrigger)
        {
            Vector3 bouncedUp = transform.position + (transform.up * 5);
            Collider[] colliders = Physics.OverlapSphere(bouncedUp, transform.localScale.magnitude);
            bool collided = colliders.Any(collider =>
            {
                if (collider.isTrigger)
                    return false;

                string tag = collider.transform.tag;
                if (tag == "Player" || tag == "Terrain" || tag == "Water")
                    return false;

                return true;
            });

            if (!collided)
                transform.position = bouncedUp;
            else
            {
                transform.position -= transform.forward * 2;
                // TODO (Ella) : make less cursed
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 50))
                {
                    float turn = 45 * Mathf.Sign(Vector3.SignedAngle(
                        Vector3.ProjectOnPlane(transform.forward, Vector3.up),
                        Vector3.ProjectOnPlane(hit.normal, Vector3.up),
                        transform.up));
                    transform.RotateAround(transform.position, Vector3.up, turn);
                }
            }

            StartCoroutine(flightController.Slow());
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
        if (groundDetector.FindGround(out Vector3 groundPos, out _))
            SetFixedPosition(groundPos);
    }

    public void AddSelfToSavablesList() {
        Save.savables.Add(this);
    }

    public void GetSaveData(ref Save.SaveData saveData) {
        saveData.playerinventory = inventory;
    }

    public void LoadData(ref Save.SaveData saveData) {
        inventory = saveData.playerinventory;
    }
}
