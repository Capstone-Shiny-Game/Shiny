using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade_control : MonoBehaviour
{
    public GameObject crow;
    
    
    [SerializeField] GameObject forSale;
    [SerializeField] int cost;
    public GameObject[] acceptedPaymentPrefabs; 



    private bool trading;
    private Collider tradeArea;
    private GameObject[] traded;
    private int acceptedPay = 0;

    private void Start()
    {
        //

        tradeArea = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trading = true;
            //move player to "perch_pos"

            //lock takeoff
            // display trade ui

        }
    }

    private void Update()
    {
        if (trading)
        {
            //check the dropped items

            while (acceptedPay < cost)
            {
                //if it fits payment
               // if (tradeArea.bounds.Contains ("gameObject with "Tradeable" tag" ))
                //save in "traded"
                
                //destroy obj
                //fill in trade ui

                //else spit out
                //display "unnacetpable trade item" ui until ok button pressed 
            }

            trading = false;
            //spawn forSale item
            //display "trade another?"



        }
    }
    public void onCancelButton()
    {
        trading = false;

        
        if (acceptedPay < cost)
        {
            //respawn traded items if transaction wasnt complete
        }
        //crow takeoff

    }


}
