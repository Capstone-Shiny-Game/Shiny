using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public List<RoastedMap> roastedMaps;

    public Projectile projectile;

    private void Start()
    {
        projectile = new Projectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        // *use different tag?
        if (other.CompareTag("Tradeable"))
        {
            TryRoastingObject(other.gameObject);
        }
    }

    private void TryRoastingObject(GameObject item)
    {
        bool canRoast = false;
        GameObject roastedObj = null;
        foreach (RoastedMap entry in roastedMaps)
        {
            if (item.name.StartsWith(entry.given.name))
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

        //Debug.Log($"trans.pos: {transform.position}");
        //Debug.Log($"offset: {offset}");

        if (canRoast)
        {
            Destroy(item);
            // spawn and move to location where
            // TransformExtensions can find projectile target/ground
            GameObject emptyObj = new GameObject();
            GameObject eo = Instantiate(emptyObj, transform.position + offset, Quaternion.identity);
            Vector3 target = eo.transform.FindGround(transform.localScale.y / 2);
            Destroy(eo);

            GameObject roastedItem = Instantiate(roastedObj, transform.position, transform.rotation);
            StartCoroutine(projectile.Launch(
                roastedItem?.GetComponent<Rigidbody>(), roastedItem.transform.rotation, target));
        }
        else
        {

        }
    }
}
