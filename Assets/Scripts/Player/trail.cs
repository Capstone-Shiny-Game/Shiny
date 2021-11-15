using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trail : MonoBehaviour
{

    private FlightController flightController;
    public GameObject trailObject;
    private float speed;

    void Start()
    {
        flightController = GetComponent<FlightController>();
    }

    void Update()
    {
        Debug.Log(flightController.GetSpeed());
    }
}
