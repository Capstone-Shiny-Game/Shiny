using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private const float RaycastDistance = 100;

    public Vector3? FindGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, RaycastDistance)
            && hit.transform.tag == "Terrain")
            return hit.point + new Vector3(0, transform.localScale.y / 2, 0);
        else
            return null;
    }

    // TODO (Ella #65) : Add FindWater() method or similar
    // TODO (Ella #66) : Consider normal
}
