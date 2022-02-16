using System;
using System.Collections;
using UnityEngine;

public class SwipeMenuChange : MonoBehaviour
{
    [SerializeField]
    private float minDistanceForSwipe = .4f;
    [SerializeField]
    private float minTimeForSwipe = 1.0f;
    [SerializeField, Range(0f,1f)]
    private float directionLeeway = .9f;
    [SerializeField]
    private GameObject trail;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private Coroutine coroutine;

    public void SwipeStart(Vector2 position, float time) 
    {
        
        startPosition = position;
        startTime = time;
        if (trail is null) {
            return;
        }
        trail.SetActive(true);
        coroutine = StartCoroutine(TrailPosition());
    }

    private IEnumerator TrailPosition() {
        while (trail.activeInHierarchy) { //every frame update trail position
            trail.transform.position = Vector3.zero;//TODO Get position for trail
            yield return null;
        }
        yield return null;
    }

    public void EndStart(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
        if (trail is null)
        {
            return;
        }
        trail.SetActive(false);
        StopCoroutine(coroutine);
    }

    private void DetectSwipe()
    {
        if (Vector2.Distance(startPosition, endPosition) < minDistanceForSwipe) {
            return;
        }
        if (endTime - startTime < minTimeForSwipe) {
            return;
        }
        Debug.DrawLine(startPosition, endPosition, Color.black, 3f);
        DetectDirection();
    }

    private void DetectDirection()
    {

        Vector2 direction = (endPosition - startPosition).normalized;
        if (Vector2.Dot(Vector2.left, direction) > directionLeeway)
        {
            //return left
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionLeeway)
        {
            //return right
        }
        else if (Vector2.Dot(Vector2.up, direction) > directionLeeway) { 
            //return up
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionLeeway)
        {
            //return down
        }
        
    }
}
