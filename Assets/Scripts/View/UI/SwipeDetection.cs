using System;
using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField]
    private float minDistanceForSwipe = .4f;
    [SerializeField]
    private float minTimeForSwipe = 1.0f;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    public delegate void OnSwipe(Vector2 startPosition,float startTime,Vector2 endPosition, float endTime);
    public event OnSwipe broadcastSwipe;

    public void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }
    

    public void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
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
        broadcastSwipe?.Invoke(startPosition,startTime,endPosition, endTime);
    }

    
}
