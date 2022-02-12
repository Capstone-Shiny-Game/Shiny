using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightTimer : MonoBehaviour
{
    [Range(0, 24)] public float OnTime;
    [Range(0, 24)] public float OffTime;
    private DayController controller;
    private Light self;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<DayController>();
        self = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.TimeOfDay < OnTime && controller.TimeOfDay > OffTime)
        {
            self.enabled = false;
        }
        else
        {
            self.enabled = true;
        }
    }
}
