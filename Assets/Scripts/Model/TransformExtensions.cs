using UnityEngine;

public static class TransformExtensions
{
    public static Terrain NearestTerrain(this Transform transform)
    {
        Terrain nearest = null;
        float distSq = float.PositiveInfinity;
        for (int i = 0; i < Terrain.activeTerrains.Length; i++)
        {
            Terrain candidate = Terrain.activeTerrains[i];
            float candidateDistSq = (candidate.transform.position + candidate.terrainData.size / 2 - transform.position).sqrMagnitude;
            if (candidateDistSq < distSq)
            {
                nearest = candidate;
                distSq = candidateDistSq;
            }
        }
        return nearest;
    }

    public static Vector3 FindGround(this Transform transform, float offset = 0)
    {
        Vector3 ground = transform.position;
        ground.y = transform.NearestTerrain().SampleHeight(transform.position) + offset;
        return ground;
    }

    public static bool CastGround(this Transform transform, out Vector3 ground, out bool water, float offset)
    {
        ground = Vector3.zero;
        water = false;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, transform.position.y + 200))
        {
            if (hit.collider is TerrainCollider || hit.collider.CompareTag("Terrain"))
            {
                ground = hit.point;
                ground.y += offset;
                return true;
            }
            else if (hit.collider.CompareTag("Water"))
            {
                water = true;
                ground = hit.point;
                ground.y += offset;
                return true;
            }
         }
        return false;
    }

    public static void TestCollision(this Transform transform, Vector3 position, out bool collided, out bool raycastNeeded)
    {
        Collider[] colliders = Physics.OverlapSphere(position, transform.localScale.magnitude);
        collided = false;
        raycastNeeded = false;
        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger || collider is TerrainCollider || collider.CompareTag("Player"))
                continue;
            else if (collider.CompareTag("Terrain") || collider.CompareTag("Water"))
                raycastNeeded = true;
            else
            {
                collided = true;
                break;
            }
        }
    }
}
