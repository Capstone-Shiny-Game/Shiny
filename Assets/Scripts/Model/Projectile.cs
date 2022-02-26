using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 0.4f;
    private float launchAngle = 45f;

    public void LaunchProjectile(Rigidbody rb, Quaternion initalRotation, Vector3 target)
    {
        StartCoroutine(Launch(rb, initalRotation, target));
    }

    private IEnumerator Launch(Rigidbody rb, Quaternion initialRotation, Vector3 target)
    {
                // may need to just pass in the GameObject
        while (Vector3.Distance(rb.position, target) > 1f)
        {
                                                                  // get ground?
            Vector3 projectileXZPos = new Vector3(rb.transform.position.x, 0.0f, rb.transform.position.z);
                                                        // ?
            float R = Vector3.Distance(projectileXZPos, target);
            float G = Physics.gravity.y;
            float tanAlpha = Mathf.Tan(launchAngle * Mathf.Deg2Rad);
            float H = target.y - rb.transform.position.y;

            float Vz = Mathf.Sqrt(G * R * R / (speed * (H - R * tanAlpha)));
            float Vy = tanAlpha * Vz;

            Vector3 localVelocity = new Vector3(0f, Vy, Vz);
            Vector3 globalVelocity = rb.transform.TransformDirection(localVelocity);

            //This makes the projectile go forward
            rb.velocity = globalVelocity;
            //This rotates the projectile correctly on the arc
            rb.transform.rotation = Quaternion.LookRotation(rb.velocity) * initialRotation;

            yield return null;
        }
    }
}
