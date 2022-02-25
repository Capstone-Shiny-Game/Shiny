using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeToLook : MonoBehaviour
{
    public InitializeMenuVariables canvasControls;
    private InputController inputController;

    private void Start()
    {
        inputController = canvasControls.inputController;
        if (inputController is null)
        {
            return;
        }
        if (!(canvasControls.swipeDetection is null))
        {
            canvasControls.swipeDetection.broadcastSwipe += DetectDirection;
        }
    }
    //listens to the inputController for touch start and activates trail
    public void SwipeStart(Vector2 position, float time)
    {
    }

    //listens to the inputController for touch end and deactivates trail
    public void SwipeEnd(Vector2 position, float time)
    {
    }
    public void DetectDirection(Vector2 startPosition, float startTime, Vector2 endPosition, float endTime)
    {
        if (!this.gameObject.activeInHierarchy)
        {
            return;//do nothing when paused
        }
        Debug.Log("look swipe");
        //TODO fix this then uncomment cause This is real jank 
        //inputController.flightLookHandler?.Invoke(endPosition.x, endPosition.y);
    }
}
