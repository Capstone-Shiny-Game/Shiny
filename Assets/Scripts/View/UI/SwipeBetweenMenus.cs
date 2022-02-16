using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeBetweenMenus : MonoBehaviour
{
    [SerializeField]
    private GameObject trail;
    [SerializeField, Range(0f, 1f)]
    private float directionLeeway = .9f;

    public GameObject upMenu;
    public GameObject downMenu;
    [SerializeField]
    public GameObject[] MenusInOrder;

    public InitializeMenuVariables initializeMenuVariables;
    private InputController inputController;
    private Coroutine coroutine;

    private void OnEnable()
    {
        inputController = initializeMenuVariables.inputController;
        inputController.OnStartTouch.AddListener(SwipeStart);
        inputController.OnEndTouch.AddListener(SwipeEnd);
        initializeMenuVariables.swipeDetection.broadcastSwipe += DetectDirection;
    }
    private void OnDisable()
    {
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
        if (trail is null) {
            return;
        }
        trail.SetActive(true);
        coroutine = StartCoroutine(TrailPosition());
    }
    //listens to the inputController for touch end and deactivates trail
    public void SwipeEnd(Vector2 position, float time)
    {
        if (trail is null)
        {
            return;
        }
        StopCoroutine(coroutine);
        trail.SetActive(false);
    }
    //coroutine for updating trail position
    private IEnumerator TrailPosition()
    {
        while (trail.activeInHierarchy)
        { //every frame update trail position
            trail.transform.position = inputController.PrimaryPosition();
            yield return null;
        }
        yield return null;
    }

    private void ChangeMenus(Direction direction) { 
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
        else if (Vector2.Dot(Vector2.up, direction) > directionLeeway) {
            ChangeMenus(Direction.up);
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionLeeway)
        {
            ChangeMenus(Direction.down);
        }
    }
}
