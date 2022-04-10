using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A helper class for boids, used to equally space points on a sphere.
/// Essentially, it works by rotating around the center point a certain angle
/// before moving outwards. https://www.geogebra.org/m/YThycjQK#material/T8eKzDu5
/// provides a good visualization for how it works with r=0.618034
/// </summary>
public class PointsOnSphere
{
    // An array holding the directions calculated
    public readonly Vector3[] directions;

    /// <summary>
    /// The method that actually calculates different directions
    /// </summary>
    public PointsOnSphere(int numViewDirections = 300)
    {
        // Initialize the array of 3D vectors to hold `numViewDirections` vectors
        directions = new Vector3[numViewDirections];

        // Literally the definition of the golden ratio mathematically speaking
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        // Turn angle in radians
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        // For each direction to calculate, do some fancy math to calculate
        // the directions on the sphere
        for (int i = 0; i < numViewDirections; i++)
        {
            float t = (float)i / numViewDirections;
            // Angle of rotation (I think counterclockwise?)
            float inclination = Mathf.Acos(1 - 2 * t);
            // Angle up from flat
            float azimuth = angleIncrement * i;

            // Given those two angles, calculate the point on the sphere
            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            // And then update the array at that position
            directions[i] = new Vector3(x, y, z);
        }
    }

}