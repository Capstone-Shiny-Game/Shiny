using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invTestPlayer : MonoBehaviour
{
    // Movement
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;

    private Inventory inventory;
    [SerializeField] private UI_inventory uiInventory; //this variable holds the ui_inventory object from the scene


    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        //inventory initialization
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);

        //testing stuffs below, to be deleted
        ItemWorld.SpawnItemWorld(new Vector3(20,2,20), new Item { itemType = Item.ItemType.shiny, amount = 1});
        ItemWorld.SpawnItemWorld(new Vector3(-20, 2, 20), new Item { itemType = Item.ItemType.food, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(0, 2, 20), new Item { itemType = Item.ItemType.potion, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(10, 2, 10), new Item { itemType = Item.ItemType.potion, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-10, 2, 10), new Item { itemType = Item.ItemType.potion, amount = 1 });
    }

    void Update()
    {
        //Vertical Movement
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //Horizontal movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }


        //inventory testing stuffs

    }

    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("touching item");
        //add items to inventory
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        if (itemWorld != null) {
            //touching item
            Debug.Log(itemWorld.GetItem().GetType());
            //TODO : add weights here
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();

        }
    }
}