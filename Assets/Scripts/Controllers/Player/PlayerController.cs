using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Cinemachine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour, Savable
{


    public enum CrowState { Flying, Gliding, Walking, Idle, Talking, Perching };

    public CrowState state { get; private set; }
    private Rigidbody rb;
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

    //private InputAction walkAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/F");
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
        rb = GetComponent<Rigidbody>();
        flightCam = GameObject.Find("CM Flying");
        walkCam = GameObject.Find("CM Walking");


        flightController = GetComponent<FlightController>();
        walkingController = GetComponent<WalkingController>();
        crow = GetComponent<Crow>();

        walkingController.WalkedOffEdge += () => SetState(CrowState.Flying, 0.5f);
        walkingController.SubstateChanged += s => SetState(s);
        flightController.Landed += AttemptToLand;
        flightController.LandedPerch += (c, c2)=> AttemptToLandPerch(c, c2);

        flightController.FlightTypeChanged += glide => SetState(glide ? CrowState.Gliding : CrowState.Flying);

        // inventory initialization
        Item.SetItemDB(itemDB);
        SetInventory(new Inventory());
        //Save initialization
        AddSelfToSavablesList();

        SetState(CrowState.Flying);
    }
    //turns off all walking animation
    public void AnimationFlyingSuite()
    {
        birdAnimator.SetBool("isWalking", false);
        birdAnimator.SetBool("isIdle", false);
    }
    //turns off all flying animations
    public void AnimationWalkingSuite()
    {
        birdAnimator.SetBool("isFlying", false);
        birdAnimator.SetBool("isGliding", false);
        birdAnimator.SetBool("WalktoFly", false);
    }
    private void SetState(CrowState next, float addYForTakeoff = 0)
    {
        state = next;
        bool previouslyFlying = flightController.enabled;

        flightController.enabled = state == CrowState.Flying || state == CrowState.Gliding;
        walkingController.enabled = state == CrowState.Walking || state == CrowState.Idle || state == CrowState.Perching;

        flightCam.SetActive(flightController.enabled);
        walkCam.SetActive(!flightController.enabled);
        if (CrowState.Walking == state)
        {
            crow.resetModelRotation();
        }
        birdAnimator.SetBool("isFlying", state == CrowState.Flying);
        birdAnimator.SetBool("isGliding", state == CrowState.Gliding);
        birdAnimator.SetBool("isWalking", state == CrowState.Walking);
        birdAnimator.SetBool("isIdle", state == CrowState.Idle || state == CrowState.Perching);

        if (flightController.enabled && !previouslyFlying && addYForTakeoff != 0)
        {
            Vector3 pos = transform.position;
            pos.y += addYForTakeoff;
            transform.position = pos;
        }

        if (!previouslyFlying && flightController.enabled)
        {
            // pitch up on takeoff
            transform.RotateAround(transform.position, transform.right, -30);
            birdAnimator.SetBool("WalktoFly", true);
            flightCam.GetComponent<ModifyOrbitor>().ResetZero();

        }

        if (previouslyFlying && !flightController.enabled)
        {
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
                walkCam.GetComponent<ModifyOrbitor>().ResetZero();
                SetState(CrowState.Walking);
            }
        }
        else
        {
            crow.resetModelRotation();
            SetFixedPosition(transform.FindGround(transform.localScale.y / 2));
            walkCam.GetComponent<ModifyOrbitor>().ResetZero();
            SetState(CrowState.Walking);

        }
    }
    private void AttemptToLandPerch(Transform t, Transform lookAt)
    {
        crow.resetModelRotation();
        Quaternion rotation = transform.rotation;
        transform.LookAt(lookAt);
        transform.position = t.position;
        walkCam.GetComponent<ModifyOrbitor>().ResetZero();
        SetState(CrowState.Perching);


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

    }
    public void SwapWalking()
    {
        //SetState(CrowState.Flying, 2.0f);
        if (state == CrowState.Walking || state == CrowState.Idle)
            takeOffLerp();
        else if (state == CrowState.Flying || state == CrowState.Gliding)
            AttemptToLand(true);
    }
    private void OnDisable()
    {
        NPCInteraction.OnNPCInteractEvent -= EnterNPCDialogue;
        NPCInteraction.OnNPCInteractEndEvent -= ExitNPCDialogue;

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
        //Vector3 ground = transform.FindGround(transform.localScale.y / 2);
        //Vector3 npcFront = npcTransform.position + npcTransform.forward * 4.0f;
       if(npcTransform !=null)
        SetFixedPosition(npcTransform.position);

        //SetFixedPosition(new Vector3(npcFront.x, ground.y, npcFront.z));
        //SetFixedRotation(npcTransform.position);

        ControllerUI.SetActive(false);
    }

    private void ExitNPCDialogue()
    {
        ControllerUI.SetActive(true);
        //transform.position = positionBeforeDialogue;
        //transform.rotation = rotationBeforeDialogue;
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

    private void takeOff()
    {
        //increment the take off upwards
        //to do, figure that out
        //delay 2 seconds too before switching over to flightcontroller since flight mode there is constant moving forward
        float velocity = 400f;
        rb.velocity += new Vector3(0, velocity, 0) * Time.deltaTime;
    }
    private void takeOffLerp()
    {
        //increment the take off upwards
        //to do, figure that out
        //delay 2 seconds too before switching over to flightcontroller since flight mode there is constant moving forward
        Vector3 start;
        Vector3 des;
        float fraction = 0;
        float speed = .5f;
        //SetState(CrowState.Flying, 2.0f);
        float velocity = 40f;
        start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        des = new Vector3(transform.position.x, transform.position.y+200f, transform.position.z);
            fraction += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, des, fraction);
        SetState(CrowState.Flying, 2.0f);
    }
    IEnumerator WaitAndMove(float delayTime)
    {
        Vector3 start;
        Vector3 des;
        start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        des = new Vector3(transform.position.x, transform.position.y + 200f, transform.position.z);
        yield return new WaitForSeconds(delayTime); // start at time X
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 1)
        { // until one second passed
            transform.position = Vector3.Lerp(start, des, Time.time - startTime); // lerp from A to B in one second
            yield return 1; // wait for next frame
        }
    }
}
