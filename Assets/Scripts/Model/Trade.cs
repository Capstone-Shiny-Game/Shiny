using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade : MonoBehaviour
{
    public List<TradeEntry> tradeMap;
    private GameObject player;
    private ParticleSystem particles;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // some use collisions..
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Tradeable"))
        {
            TryTrade(other.gameObject);
        }
    }

    // ..and others use triggers
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Tradeable"))
        {
            TryTrade(other.gameObject);
        }
    }

    private void TryTrade(GameObject givenItem)
    {
        bool canTrade = false;
        GameObject returned = null;
        foreach (TradeEntry entry in tradeMap)
        {
            if (givenItem.name.StartsWith(entry.given.name))
            {
                canTrade = true;
                returned = entry.returned;
                break;
            }
        }

        Vector3 randomVelocity = GetInitialVelocity();
         
        if (canTrade)
        {
            StartCoroutine(TradeItemWithDelay(returned, randomVelocity));
            Destroy(givenItem);
        }
        else
        {
            StartCoroutine(ReturnItemWithDelay(givenItem, randomVelocity));
        }
    }

    private IEnumerator TradeItemWithDelay(GameObject item, Vector3 randomVelocity)
    {
        if (particles) 
        {
            var main = particles.main;
            main.simulationSpeed = main.simulationSpeed * 4f;
        }

        yield return new WaitForSeconds(2f);

        GameObject returnedItem = Instantiate(item, transform.position, transform.rotation);
        Physics.IgnoreCollision(returnedItem.GetComponent<Collider>(), GetComponent<Collider>());
        returnedItem.GetComponent<Rigidbody>().velocity = randomVelocity;

        if (particles) 
        {
            var main = particles.main;
            main.simulationSpeed = main.simulationSpeed / 4f;
        }
    }

    private IEnumerator ReturnItemWithDelay(GameObject item, Vector3 randomVelocity) 
    {
        Physics.IgnoreCollision(item.GetComponent<Collider>(), GetComponent<Collider>());
        item.transform.position = transform.position;
        item.transform.rotation = transform.rotation;

        item.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        item.SetActive(true);

        item.GetComponent<Rigidbody>().velocity = randomVelocity;
    }

    private Vector3 GetInitialVelocity()
    {
        float ySet = 10f;

        Vector3 velocity = ((player.transform.position - transform.position) / 2)
            + new Vector3(0, ySet, 0);

        return velocity;
    }
}
