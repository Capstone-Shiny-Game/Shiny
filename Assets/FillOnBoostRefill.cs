using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillOnBoostRefill : MonoBehaviour
{
    private FlightController flightController;

    /// <summary>
    /// Called whenever this script is enabled or the object it's attached to is enabled.
    /// </summary>
    void OnEnable()
    {
        flightController = GameObject.FindGameObjectWithTag("Player").GetComponent<FlightController>();
    }
    /// <summary>
    /// Called after normal update stuff, should contribute less to performance
    /// </summary>
    void LateUpdate()
    {
        if (flightController.boostStartTime < 0)
        {
            this.gameObject.GetComponent<Image>().fillAmount = 1;
            return;
        }
        float elapsedTime = Time.time - flightController.boostStartTime;
        float fillAmount = elapsedTime / flightController.boostDuration;
        this.gameObject.GetComponent<Image>().fillAmount = fillAmount;
    }
}
