using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public List<RoastedMap> roastedMaps;

    private Projectile projectile;

    private void Start()
    {
        projectile = new Projectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        // *use different tag?
        if (other.CompareTag("Tradeable"))
        {
            TryRoastingObject(other.gameObject.name);
        }
    }

    private void TryRoastingObject(string itemName)
    {
        bool canRoast = false;
        GameObject roastedObj = null;
        foreach (RoastedMap entry in roastedMaps)
        {
            if (entry.given.name.Equals(itemName))
            {
                canRoast = true;
                roastedObj = entry.returned;
                break;
            }
        }

        float xRand = Random.value * 3f;
        float ySet = transform.position.y + 10f;
        float zRand = Random.value * 3f;
        Vector3 offset = new Vector3(xRand, ySet, zRand);

        if (canRoast)
        {
            // spawn and move to temp location where
            // TransformExtensions can find projectile target/ground
            GameObject ro = Instantiate(roastedObj, transform.position + offset, transform.rotation);
            Vector3 target = ro.transform.FindGround(transform.localScale.y / 2);

            projectile.LaunchProjectile(
                roastedObj?.GetComponent<Rigidbody>(), roastedObj.transform.rotation, target);
        }
        else
        {

        }
    }
}
