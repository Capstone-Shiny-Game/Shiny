using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Savable
{
    private enum CrowState { Flying, Walking, Splashing, Talking };

    private CrowState state;

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

        groundDetector = GetComponent<GroundDetector>();

        walkingController.WalkedOffEdge += () => SetState(CrowState.Flying, 0.5f);
        flightController.Landed += AttemptToLand;

        // inventory initialization
        SetInventory(new Inventory());
        //Save initialization
        AddSelfToSavablesList();

        SetState(CrowState.Flying);
    }

    private void SetState(CrowState next, float addYForTakeoff = 0)
    {
        CrowState previous = state;
        state = next;

        if (state == CrowState.Flying && addYForTakeoff != 0)
        {
            Vector3 pos = transform.position;
            pos.y += addYForTakeoff;
            transform.position = pos;
        }

        flightController.enabled = state == CrowState.Flying;
        walkingController.enabled = state == CrowState.Walking || state == CrowState.Splashing;
        cameraController.isWalking = walkingController.enabled;

        birdAnimator.SetBool("isFlying", state == CrowState.Flying);
        birdAnimator.SetBool("isWalking", state == CrowState.Walking);
        birdAnimator.SetBool("isSwim", state == CrowState.Splashing);

        if ((previous == CrowState.Walking || previous == CrowState.Splashing) && state == CrowState.Flying)
        {
            // pitch up on takeoff
            transform.RotateAround(transform.position, transform.right, -30);
            birdAnimator.SetBool("WalktoFly", true);
        }

        if (previous == CrowState.Flying && walkingController.enabled)
        {
            flightController.speed = 10.0f;
            birdAnimator.SetBool("WalktoFly", false);
        }
    }

    private void AttemptToLand()
    {
        if (state == CrowState.Flying && groundDetector.FindGround(out Vector3 groundPos, out bool isWater))
        {
            flightController.speed = 10.0f;
            SetState(isWater ? CrowState.Splashing : CrowState.Walking);
            SetFixedPosition(groundPos);
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

        walkAction.performed += ctx =>
        {
            if (state == CrowState.Walking || state == CrowState.Splashing)
                SetState(CrowState.Flying, 2.0f);
            else
                AttemptToLand();
        };
        walkAction.Enable();

    }

    private void OnDisable()
    {
        NPCInteraction.OnNPCInteractEvent -= EnterNPCDialogue;
        NPCInteraction.OnNPCInteractEndEvent -= ExitNPCDialogue;

        walkAction.Disable();
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
        SetState(CrowState.Talking);
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
        SetState(CrowState.Walking);
    }

    private void OnTriggerEnter(Collider other)
    {
        //add items to inventory
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            //touching item
            Item item = itemWorld.GetItem();
            if (maxCarryWeight >= (inventory.weight + item.getStackWeight()))
            {
                //check if picking this up would add to much weight
                if (inventory.AddItem(item))
                {
                    itemWorld.DestroySelf();
                }
            }
        }
    }

    private void TryPlaceOnGround()
    {
        if (groundDetector.FindGround(out Vector3 groundPos, out _))
            SetFixedPosition(groundPos);
    }

    public void AddSelfToSavablesList() {
        Save.AddSelfToSavablesList(this);
    }

    public void GetSaveData(ref Save.SaveData saveData) {
        saveData.playerinventory = this.inventory;
    }

    public void LoadData(ref Save.SaveData saveData) {
        SetInventory(saveData.playerinventory);
    }

    /// <summary>
    /// sets the inventory and updates the UI with the new inventory
    /// </summary>
    /// <param name="inventory"></param>
    private void SetInventory(Inventory inventory)
    {
        uiInventory.SetInventory(inventory);
        uiHotbar.SetInventory(inventory);
        this.inventory = inventory;
    }
}
