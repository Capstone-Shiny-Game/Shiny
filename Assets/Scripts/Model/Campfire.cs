using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public List<RoastedMap> roastedMaps;

    private void OnCollisionEnter(Collision other)
    {
        // *use different tag?
        if (other.gameObject.CompareTag("Tradeable"))
        {
            TryRoastingObject(other.gameObject);
        }
    }

    private void TryRoastingObject(GameObject givenItem)
    {
        bool canRoast = false;
        GameObject roastedObj = null;
        foreach (RoastedMap entry in roastedMaps)
        {
            if (givenItem.name.StartsWith(entry.given.name))
            {
                canRoast = true;
                roastedObj = entry.returned;
                break;
            }
        }

        Vector3 randomVelocity = GetRandomInitialVelocity();
        if (canRoast)
        {
            Destroy(givenItem);
            StartCoroutine(RoastWithDelay(roastedObj, randomVelocity));
        }
        else
        {
            StartCoroutine(IgnoreCollisions(givenItem));
            givenItem.GetComponent<Rigidbody>().velocity = randomVelocity;
        }
    }

    private IEnumerator RoastWithDelay(GameObject roastedObj, Vector3 randomVelocity)
    {
        // TODO: start particle effects here

        // *** lower this for testing
        yield return new WaitForSeconds(2f);

        GameObject roastedItem = Instantiate(roastedObj, transform.position, transform.rotation);
        StartCoroutine(IgnoreCollisions(roastedItem));
        roastedItem.GetComponent<Rigidbody>().velocity = randomVelocity;

        // TODO: end particle effects here
    }

    private IEnumerator IgnoreCollisions(GameObject obj)
    {
        obj.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        obj.GetComponent<Collider>().enabled = true;
    }

    private Vector3 GetRandomInitialVelocity()
    {
        float xSign = Random.value > 0.5f ? 1f : -1f;
        float zSign = Random.value > 0.5f ? 1f : -1f;

        float xRand = xSign * 3f;
        float ySet = 10f;
        float zRand = zSign * 3f;

        return new Vector3(xRand, ySet, zRand);
    }
}
