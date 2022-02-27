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

    public static bool CastGround(this Transform transform, out Vector3 ground, float offset)
    {
        ground = Vector3.zero;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, transform.position.y + 200))
        {
            if (hit.collider is TerrainCollider || hit.collider.CompareTag("Terrain"))
            {
                ground = hit.point;
                ground.y += offset;
                return true;
            }
         }
        return false;
    }

    public static bool RaycastNeeded(this Transform transform)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger || collider is TerrainCollider || collider.CompareTag("Player"))
                continue;
            else
                return true;
        }
        return false;
    }
}
