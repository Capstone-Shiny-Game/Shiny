using UnityEngine;

public static class TransformExtensions
{
    public static Vector3 FindGround(this Transform transform, float offset = 0)
    {
        Vector3 ground = transform.position;
        ground.y = transform.NearestTerrain().SampleHeight(transform.position) + offset;
        return ground;
    }

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
}
