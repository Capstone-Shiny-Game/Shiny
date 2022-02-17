using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour, Savable
{
    public enum CrowState { Flying, Gliding, Walking, Idle, Talking };

    public CrowState state { get; private set; }

    [System.Serializable]
    public struct Offset
    {
        public float forward;
        public float up;
    }
    public Animator birdAnimator;
    private FlightController flightController;
    private WalkingController walkingController;
    private GameObject flightCam;
    private GameObject walkCam;
    private Crow crow;

    // public GameObject NPCUI;
    public GameObject ControllerUI;

    private InputAction walkAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/F");
    [System.NonSerialized]
    public Inventory inventory;


    // this variable holds the ui_inventory object from the scene
    [Header("inventory references")]
    [SerializeField]
    UI_inventory uiInventory;
    [SerializeField]
    UIHotbar uiHotbar;
    [SerializeField]
    ItemDB itemDB;

    // specifies where inventory item should appear when dropped by player
    [Header("inventory drop items")]
    public Offset walkingOffset = new Offset { forward = 0, up = 0 };
    public Offset flyingOffset = new Offset { forward = 0, up = 0 };
    public double maxCarryWeight = 0;

    public static event Action AttemptedGrabOrRelease;

    private void Start()
    {
        Camera.main.useOcclusionCulling = false;

        flightCam = GameObject.Find("CM Flying");
        walkCam = GameObject.Find("CM Walking");

        flightController = GetComponent<FlightController>();
        walkingController = GetComponent<WalkingController>();
        crow = GetComponent<Crow>();

        walkingController.WalkedOffEdge += () => SetState(CrowState.Flying, 0.5f);
        walkingController.SubstateChanged += s => SetState(s);
        flightController.Landed += AttemptToLand;
        flightController.FlightTypeChanged += glide => SetState(glide ? CrowState.Gliding : CrowState.Flying);

        // inventory initialization
        Item.SetItemDB(itemDB);
        SetInventory(new Inventory());
        //Save initialization
        AddSelfToSavablesList();

        SetState(CrowState.Flying);
    }

    private void SetState(CrowState next, float addYForTakeoff = 0)
    {
        CrowState previous = state;
        state = next;


        if ((state == CrowState.Flying || state == CrowState.Gliding) && addYForTakeoff != 0)
        {
            Vector3 pos = transform.position;
            pos.y += addYForTakeoff;
            transform.position = pos;
        }
        flightController.enabled = state == CrowState.Flying ||  state == CrowState.Gliding;
        flightCam.SetActive(flightController.enabled);
        walkCam.SetActive(!flightController.enabled);
        walkingController.enabled = state == CrowState.Walking  || state == CrowState.Idle;
        if(CrowState.Walking == state)
        {
            crow.resetModelRotation();
        }
        birdAnimator.SetBool("isFlying", state == CrowState.Flying);
        birdAnimator.SetBool("isWalking", state == CrowState.Walking);
        birdAnimator.SetBool("isIdle", state == CrowState.Idle);
        birdAnimator.SetBool("isGliding", state == CrowState.Gliding);
        
        if ((previous == CrowState.Walking || previous == CrowState.Idle) && state == CrowState.Flying)
        {
            // pitch up on takeoff
            transform.RotateAround(transform.position, transform.right, -30);
            birdAnimator.SetBool("WalktoFly", true);
        }

        if ((previous == CrowState.Flying || previous == CrowState.Gliding))
        {
            //flightController.speed = 10.0f;
            birdAnimator.SetBool("WalktoFly", false);
        }
    }

    private void AttemptToLand(bool raycastNeeded)
    {
        if (raycastNeeded)
        {
            if (transform.CastGround(out Vector3 ground, transform.localScale.y / 2))
            {
                crow.resetModelRotation();
                SetFixedPosition(ground);
                SetState(CrowState.Walking);
            }
        }
        else
        {
            crow.resetModelRotation();
            SetFixedPosition(transform.FindGround(transform.localScale.y / 2));
            SetState(CrowState.Walking);

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
    /// TODO (Ella) : Consider simplification as part of #110
    /// </summary>
    public void AttemptPickup()
    {
        AttemptedGrabOrRelease?.Invoke();
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
            if (state == CrowState.Walking || state == CrowState.Idle)
                SetState(CrowState.Flying, 2.0f);
            else if (state == CrowState.Flying || state == CrowState.Gliding)
                AttemptToLand(true);
        };
        walkAction.Enable();
    }

    private void OnDisable()
    {
        NPCInteraction.OnNPCInteractEvent -= EnterNPCDialogue;
        NPCInteraction.OnNPCInteractEndEvent -= ExitNPCDialogue;

        walkAction.Disable();
    }

    private void SetFixedPosition(Vector3 position) => transform.position = position;

    private void SetFixedRotation(Vector3 lookPosition)
    {
        Vector3 lookPos = lookPosition - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
    }

    private Vector3 positionBeforeDialogue;
    private Quaternion rotationBeforeDialogue;

    private void EnterNPCDialogue(Transform npcTransform)
    {
        SetState(CrowState.Talking);
        positionBeforeDialogue = transform.position;
        rotationBeforeDialogue = transform.rotation;
        // position player in front of NPC
        Vector3 ground = transform.FindGround(transform.localScale.y / 2);
        Vector3 npcFront = npcTransform.position + npcTransform.forward * 4.0f;
        SetFixedPosition(new Vector3(npcFront.x, ground.y, npcFront.z));
        SetFixedRotation(npcTransform.position);

        ControllerUI.SetActive(false);
    }

    private void ExitNPCDialogue()
    {
        ControllerUI.SetActive(true);
        transform.position = positionBeforeDialogue;
        transform.rotation = rotationBeforeDialogue;
        SetState(CrowState.Walking);
    }

    public void AddSelfToSavablesList()
    {
        Save.AddSelfToSavablesList(this);
    }

    public void GetSaveData(ref Save.SaveData saveData)
    {
        saveData.playerinventory = this.inventory;
    }

    public void LoadData(ref Save.SaveData saveData)
    {
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
