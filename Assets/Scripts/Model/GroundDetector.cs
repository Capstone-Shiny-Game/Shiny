using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private const float RaycastDistance = 100;

    public bool FindGround(out Vector3 groundPos, out bool isWater)
    {
        groundPos = Vector3.zero;
        isWater = false;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, RaycastDistance)
            && (hit.transform.tag == "Terrain" || hit.transform.tag == "Water"))
        {
           // Debug.Log("HIT: " + hit.transform.tag);
            groundPos = hit.point + new Vector3(0, transform.localScale.y / 2, 0);
            isWater = hit.transform.tag == "Water";
            return true;
        }
        else
            return false;
    }

    // TODO (Ella #66) : Consider normal
}
