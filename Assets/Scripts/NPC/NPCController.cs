using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;

    void Start()
    {
        agent.SetDestination(new Vector3(-10.0f, 0.0f, -5.0f));
    }

    void Update()
    {
    }
}
