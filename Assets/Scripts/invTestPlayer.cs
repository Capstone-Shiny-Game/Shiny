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
    [SerializeField] private UI_inventory uiInventory;


    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    private void Awake()
    {
        inventory = new Inventory();
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
}