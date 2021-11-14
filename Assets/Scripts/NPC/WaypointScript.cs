using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    private const float debugDrawRadius = 1.0f;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }
}
