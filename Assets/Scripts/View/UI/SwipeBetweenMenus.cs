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
    public GameObject leftMenu;
    public GameObject rightMenu;

    public InitializeMenuVariables initializeMenuVariables;
    private InputController inputController;
    private Coroutine coroutine;

    private void OnEnable()
    {
        inputController = initializeMenuVariables.inputController;
    }

    private enum Direction
    {
        up,
        down,
        left,
        right
    }
    

    public void SwipeStart(Vector2 position, float time)
    {
        if (trail is null) {
            return;
        }
        trail.SetActive(true);
        coroutine = StartCoroutine(TrailPosition());
    }
    public void SwipeEnd(Vector2 position, float time)
    {
        if (trail is null)
        {
            return;
        }
        StopCoroutine(coroutine);
        trail.SetActive(false);
    }
    private IEnumerator TrailPosition()
    {
        while (trail.activeInHierarchy)
        { //every frame update trail position
            trail.transform.position = Vector3.zero;//TODO Get position for trail
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
