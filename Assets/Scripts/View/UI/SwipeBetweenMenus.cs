using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeBetweenMenus : MonoBehaviour
{

    [SerializeField]
    [Tooltip("A particle effect trail where the player taps")]
    private GameObject particleTrail;
    [SerializeField, Range(0f, 1f)]
    private float directionLeeway = .9f;

    // How swipe menu will work:
    // 1 - enumerate the menus
    // private enum menus {
    //     PauseMenu, // NOTE: This will need to be the *instance* from parent
    //     ConfirmMenu,
    //     SaveMenu,
    //     SettingsMenu,
    //     flightUI
    // }
    // 2 - keep a copy of the current "active" menu
    // private GameObject currentMenu;
    // 3 - define transitions between menus, for example from the default pause
    // menu, swiping left takes you to the quest menu, swiping down from there
    // takes you to completed quest menu, etc
    // 4 - enable/disable menus based on current state
    // 5 - on swipe, do transition

    public InitializeMenuVariables initializeMenuVariables;
    private InputController inputController;
    private Coroutine coroutine;

    private void OnEnable()
    {
        if (inputController is null)
        {
            return;
        }
        inputController = initializeMenuVariables.inputController;
        inputController.OnStartTouch.AddListener(SwipeStart);
        inputController.OnEndTouch.AddListener(SwipeEnd);
        if (!(initializeMenuVariables.swipeDetection is null))
        {
            initializeMenuVariables.swipeDetection.broadcastSwipe += DetectDirection;
        }
    }
    private void OnDisable()
    {
        if (inputController is null)
        {
            return;
        }
        inputController.OnStartTouch.RemoveListener(SwipeStart);
        inputController.OnEndTouch.RemoveListener(SwipeEnd);
    }

    private enum Direction
    {
        up,
        down,
        left,
        right
    }

    //listens to the inputController for touch start and activates trail
    public void SwipeStart(Vector2 position, float time)
    {
        if (particleTrail is null)
        {
            return;
        }
        particleTrail.SetActive(true);
        coroutine = StartCoroutine(TrailPosition());
    }
    //listens to the inputController for touch end and deactivates trail
    public void SwipeEnd(Vector2 position, float time)
    {
        if (particleTrail is null)
        {
            return;
        }
        StopCoroutine(coroutine);
        particleTrail.SetActive(false);
    }
    //coroutine for updating trail position
    private IEnumerator TrailPosition()
    {
        while (particleTrail.activeInHierarchy)
        { //every frame update trail position
            particleTrail.transform.position = inputController.PrimaryPosition();
            yield return null;
        }
        yield return null;
    }

    private void ChangeMenus(Direction direction)
    {
        switch (direction)
        {
            case Direction.right:
                // right menu
                break;
            case Direction.left:
                // left menu
                break;
            case Direction.down:
                // down menu
                break;
            case Direction.up:
                // up menu
                break;
        }
    }
    // listener to on swipe in swipe detection
    public void DetectDirection(Vector2 startPosition, float startTime, Vector2 endPosition, float endTime)
    {

        Vector2 direction = (endPosition - startPosition).normalized;
        if (Vector2.Dot(Vector2.left, direction) > directionLeeway)
        {
            ChangeMenus(Direction.left);
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionLeeway)
        {
            ChangeMenus(Direction.right);
        }
        else if (Vector2.Dot(Vector2.up, direction) > directionLeeway)
        {
            ChangeMenus(Direction.up);
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionLeeway)
        {
            ChangeMenus(Direction.down);
        }
    }
}
