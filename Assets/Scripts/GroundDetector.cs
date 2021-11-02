using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private RaycastHit hit;

    private float distance = 100f;

    public Vector3? FindGround()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, distance))
        {
            return hit.point += new Vector3(0, transform.localScale.y / 2, 0);
        }

        return null;
    }
}
