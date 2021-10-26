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
    /// How many rocks should the player have to put into the pitcher
    /// </summary>
    public int RockThreshhold;
    // Start is called before the first frame update
    void Start()
    {        
        RockCount = 0;
        Debug.Log("Init setup was done");
    }

    // Update is called once per frame
    void Update()
    {
        // TODO:
        // If rock collides with pitcher water,
        // increment rock counter and raise water level

        // TODO:
        // If rock threshold is met,
        // Allow player to drink lower water level
        if(RockCount == RockThreshhold)
        {
            Debug.Log("threshold has been met");
        }
    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Rock")
        {
            RockCount++;
            RaiseWaterLever();
            Debug.Log("RockCount: " + RockCount);
        }
    }

    void RaiseWaterLever()
    {

    }
}
