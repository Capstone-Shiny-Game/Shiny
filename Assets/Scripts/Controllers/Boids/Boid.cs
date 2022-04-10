using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main boid class.
/// </summary>
public class Boid : MonoBehaviour
{
    // To get this to work:
    // 1. Create two empty game objects in-scene
    // 2. Place this script (Boid.cs) on one game object. Place the boid you
    //    want to spawn as a child of the same game object.
    // 3. Place the BoidManager.cs script on the other game object. Drag the
    //    first game object in as the prefab for the boid manager script.
    // 4. Attach or create and atach some BoidSettings SO to the BoidManager.
    // 5. Optionally, create another game object to use as a target and attach
    //    it to the boid manager instance.
    // 6. Also optionally, modify the number of threads both in the
    //    BoidManager.cs script *AND* the BoidCompute.compute shader.

    // A settings file for how this boid will act
    BoidSettings settings;


    // The position, forward direction, and velocity variables
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    // The acceleration of the boid
    Vector3 acceleration;
    // Direction of the average boid in the flock
    [HideInInspector]
    public Vector3 avgFlockHeading;
    // Direction the average boid is avoiding
    [HideInInspector]
    public Vector3 avgAvoidanceHeading;
    // The central point of all flockmates
    [HideInInspector]
    public Vector3 centerOfFlockmates;
    // Number of flockmates that this particular boid can see
    [HideInInspector]
    public int numPerceivedFlockmates;

    // Cached material, position, and target to help speed things up
    Material material = null;
    Transform cachedTransform;
    Transform target;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Cache the material and transform for faster lookups later
        try
        {
            material = transform.GetComponentInChildren<MeshRenderer>().material;
        }
        catch
        {

        }
        cachedTransform = transform;
    }

    /// <summary>
    /// Starts setup for the boid
    /// </summary>
    /// <param name="settings">The settings file that this boid will use</param>
    /// <param name="target">The target this boid is trying to move towards</param>
    public void Initialize(BoidSettings settings, Transform target)
    {
        // Set this particular boid's target and settings to the provided ones
        this.target = target;
        Initialize(settings);
    }
    /// <summary>
    /// Starts setup for the boid
    /// </summary>
    /// <param name="settings">The settings file that this boid will use</param>
    public void Initialize(BoidSettings settings)
    {
        // Set this particular boid's target and settings to the provided ones
        this.settings = settings;

        // Also cache the position and forward direction
        position = cachedTransform.position;
        forward = cachedTransform.forward;

        // Set the starting speed to halfway between the min and max speed
        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        // And then multiply that speed by the forward vector to get its velocity
        velocity = transform.forward * startSpeed;
    }

    /// <summary>
    /// Sets the color of this boid
    /// </summary>
    /// <param name="col"></param>
    public void SetColour(Color col)
    {
        // If the material is cached
        if (material != null)
        {
            // Then set its color to col
            material.color = col;
        }
    }

    /// <summary>
    /// Update the boid based on various factors going on around it and its settings file
    /// </summary>
    public void UpdateBoid()
    {
        // Initially, assume that the boid doesn't need to accelerate.
        // We'll build on this by adding to the acceleration
        Vector3 acceleration = Vector3.zero;

        // If the target is cached / exists
        if (target != null)
        {
            // Calculate the current offset of the boid to its target position
            Vector3 offsetToTarget = (target.position - position);
            // Then, if the offset has a magnitude greater than `distanceEpsilon`
            // (basically if the boid is outside of a sphere with that radius)
            if (offsetToTarget.magnitude > settings.distanceEpsilon)
            {
                // Then overwrite the acceleration by steering towards the offset,
                // but multiply by targetWeight, so that the farther away the boid is,
                // the more the boid tries to turn towards the target
                acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
            }
        }

        // Next, if this boid can see flockmates
        if (numPerceivedFlockmates > 0)
        {
            // Divide the center by the number of flockmates this boid sees
            centerOfFlockmates /= numPerceivedFlockmates;

            // Calculate the offset to the center
            Vector3 offsetToFlockmatescenter = (centerOfFlockmates - position);

            // Next, get acceleration towards the average flock direction, towards
            // the flock center, and away from crashing into other boids (each with their own weight)
            var alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
            var cohesionForce = SteerTowards(offsetToFlockmatescenter) * settings.cohesionWeight;
            var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.separateWeight;

            // Add each force to the total acceleration
            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        // If this boid is heading for a collision
        if (IsHeadingForCollision())
        {
            // Cast out equally spaced rays to try to find a good avoidance direction
            Vector3 collisionAvoidDir = ObstacleRays();
            // Using what is returned from `ObstacleRays()`, multiply the direction by
            // the weight for avoiding collisions
            Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
            // Then add that force to the acceleration
            acceleration += collisionAvoidForce;
        }

        // To the current velocity, add the calculated acceleration times the
        // frame time that has passed
        velocity += acceleration * Time.deltaTime;
        // Separate the speed (magnitude) out from the direction of the velocity
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        // So that you can limit the speed value to be between the min and max
        // speeds specified in the settings file
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        // Now that the value is guaranteed to be between `minSpeed` and `maxSpeed`,
        // re-apply the speed to the velocity vector
        velocity = dir * speed;

        // Then add the calculated velocity times the frame time to cache
        cachedTransform.position += velocity * Time.deltaTime;
        // Set the forward direction as the velocity's direction to cache
        cachedTransform.forward = dir;
        // Apply the cached position to the actual transform
        position = cachedTransform.position;
        // And the same thing with the direction
        forward = dir;
    }

    /// <summary>
    /// Checks whether this boid is heading for a collision with an object.
    /// The object must not be ignored in the settings file for the boid to avoid it.
    /// </summary>
    /// <returns></returns>
    bool IsHeadingForCollision()
    {
        // A variable that may be assigned in the `Physics.SphereCast` call
        RaycastHit hit;
        // If casting a sphere forwards ends up colliding with something, return true
        if (Physics.SphereCast(position, //Origin of cast
                               settings.boundsRadius, // Radius to check
                               forward, // Direction to push the sphere
                               out hit, // Information of a hit, if it exists
                               settings.collisionAvoidDst, // Maximum length of cast
                               settings.obstacleMask // Layer mask to ignore colliders
                               ))
        {
            return true;
        }
        // Otherwise, nothing is in the way
        return false;
    }

    /// <summary>
    /// Casts rays out in a spiral on a sphere, to try to find a direction to go
    /// that avoids all obstacles
    /// </summary>
    /// <returns>A direction that (hopefully) is clear of obstacles</returns>
    Vector3 ObstacleRays()
    {
        // All the directions the rays can be cast. More information is in the
        // `BoidHelper` class.
        Vector3[] rayDirections = new PointsOnSphere().directions;

        // For each pre-calculated direction
        for (int i = 0; i < rayDirections.Length; i++)
        {
            // Set dir to the direction of a raycast
            Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
            // Create a ray to cast out
            Ray ray = new Ray(position, dir);
            // If the ray does not collide with anything, `dir` must be a clear
            // direction, so return it
            if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask))
            {
                return dir;
            }
        }

        // Otherwise, return forward? I'm confused by this, and I think the boid
        // would just continue forward until they crashed if they could not find
        // a clear avoidance path
        return forward;
    }

    /// <summary>
    /// A helper method to steer boids towards a Vector3
    /// </summary>
    /// <param name="vector">The vector3 to steer towards</param>
    /// <returns>A Vector3 with a magnitude <= `maxSteerForce`</returns>
    Vector3 SteerTowards(Vector3 vector)
    {
        // Creates a new vector from the given vector that is normalized times
        // `maxSpeed`, then subtracts the current velocity from that.
        Vector3 newVector = vector.normalized * settings.maxSpeed - velocity;
        // Limit the maximum turn to `maxSteerForce`
        return Vector3.ClampMagnitude(newVector, settings.maxSteerForce);
    }

}