using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is majorly inspired by the video linked below.
///
/// https://www.youtube.com/watch?v=5q4JHuJAAcQ
/// </summary>
public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float switchDirProb = 0.2f;
    public List<WaypointScript> waypoints;

    private int currWaypointIdx;
    private bool isTravelling;
    private bool isWaiting;
    private bool doesMoveForward = true;
    private float waitTimer;


    void Start()
    {
        if (waypoints != null && waypoints.Count > 1)
        {
            currWaypointIdx = 0;
            SetDestination();
        }
        else
        {
            Debug.LogWarning("Not enough waypoints for a path");
        }
    }

    void Update()
    {
        if (isTravelling && agent.remainingDistance <= 1.0f)
        {
            isTravelling = false;
            if (waypoints[currWaypointIdx].doesWait)
            {
                isWaiting = true;
                waitTimer = 0f;
            }
            else
            {
                ChangeWaypoint();
                SetDestination();
            }
        }

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waypoints[currWaypointIdx].waitTime)
            {
                isWaiting = false;

                ChangeWaypoint();
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        Vector3 target = waypoints[currWaypointIdx].transform.position;
        agent.SetDestination(target);
        isTravelling = true;
    }

    private void ChangeWaypoint()
    {
        if (UnityEngine.Random.Range(0f, 1f) <= switchDirProb)
        {
            doesMoveForward = !doesMoveForward;
        }

        if (doesMoveForward)
        {
            currWaypointIdx++;

            if (currWaypointIdx >= waypoints.Count)
            {
                currWaypointIdx = 0;
            }
        }
        else
        {
            currWaypointIdx--;

            if (currWaypointIdx < 0)
            {
                currWaypointIdx = waypoints.Count - 1;
            }
        }
    }
}
