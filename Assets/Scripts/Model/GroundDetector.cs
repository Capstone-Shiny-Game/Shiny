using System;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Obsolete]
    public bool FindGround(out Vector3 groundPos, out bool isWater)
    {
        return transform.CastGround(out groundPos, out isWater, transform.localScale.y / 2);
    }
}
