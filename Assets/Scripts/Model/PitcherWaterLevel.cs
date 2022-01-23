using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherWaterLevel : MonoBehaviour
{
    /// <summary>
    /// Private tracker of how many rocks player has put in pitcher
    /// </summary>
    private int RockCount;

    /// <summary>
    /// Private bool to keep track of when threshold has been met
    /// </summary>
    private bool CanGrow;

    /// <summary>
    /// Private delta by which to scale the water level
    /// </summary>
    private Vector3 ScaleChange;

    /// <summary>
    /// Private delta to move water level, so it looks like it stays in place with the scale
    /// </summary>
    private Vector3 PosChange;

    /// <summary>
    /// How many rocks should the player have to put into the pitcher
    /// </summary>
    public int RockThreshhold;

    // Start is called before the first frame update
    void Start()
    {
        RockCount = 0;
        CanGrow = true;

        // If you need to change these values, the only thing you need to keep in mind
        // is that the PosChange value needs to be exactly 1/2 of the ScaleChange value.
        //
        // This is so that the illusion of the water level stays in place and only scales upward
        ScaleChange = new Vector3(0f, 0.1f, 0f);
        PosChange = new Vector3(0f, 0.05f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO:
        // If rock threshold is met,
        // Allow player to drink lower water level
        //
        // Do not allow any more rocks to be put in
        if (RockCount == RockThreshhold)
        {
            CanGrow = false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        // Destroy the rock to avoid duplicate collisions
        Destroy(col.collider.gameObject);

        if (CanGrow && col.gameObject.tag == "Rock")
        {
            RockCount++;
            RaiseWaterLever();
            Debug.Log("RockCount: " + RockCount);
        }
    }

    void RaiseWaterLever()
    {
        this.transform.localScale += ScaleChange;
        this.transform.position += PosChange;
    }
}
