using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade_control : MonoBehaviour
{
    public GameObject crow;
    
    
    [SerializeField] GameObject forSale;
    [SerializeField] int cost;
    public GameObject[] acceptedPaymentPrefabs; 
    public Transform objectLocation;
    public string[] daysOpen;


    private DayController dateAndTime;
    private bool trading;
    private BoxCollider tradeArea;
    private GameObject[] traded;
    private int acceptedPay = 0;

    private void Start()
    {
        tradeArea = GetComponent<BoxCollider>();
        dateAndTime = GameObject.Find("GameController").GetComponent<DayController>();
    }

    void Update()
    {
        foreach(string str in daysOpen)
        {
            //How to get time \/
            //dateAndTime.TimeOfDay
            
            if(dateAndTime.CurrentDay.ToLower() == str.ToLower())
            {
                tradeArea.enabled = true;
                break;
            }
            else
            {
                tradeArea.enabled = false;
            }
        }
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

        List<GameObject> acceptableItems = new List<GameObject>();
        foreach(Collider col in Physics.OverlapBox(tradeArea.center, tradeArea.size, Quaternion.identity))
        {
            if(col.gameObject.CompareTag("Tradeable") && checkAcceptedPayment(col.gameObject))
            {
                acceptableItems.Add(col.gameObject);

                //Fill in UI

                //Might require 1 extra, not sure. Change to cost-1 if it does
                if(acceptableItems.Count == cost)
                {
                    foreach(GameObject obj in acceptableItems)
                    {
                        Destroy(obj);
                    }
                    acceptableItems.Clear();
                    Instantiate(forSale, objectLocation.position, Quaternion.identity);
                }
            }
            else
            {
                //UI Flash or somethin idk
            }
        }
    }
    private bool checkAcceptedPayment(GameObject depositedObject)
    {
        foreach(GameObject obj in acceptedPaymentPrefabs)
        {
            if(depositedObject.name.Contains(obj.name))
            {
                return true;
            }
        }
        return false;
    }


}
