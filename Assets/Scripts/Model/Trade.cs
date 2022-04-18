using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade : MonoBehaviour
{
    public List<TradeEntry> tradeMap;
    public bool isCampfire;

    private GameObject player;
    private ParticleSystem particles;
    private bool hasItem;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        particles = GetComponentInChildren<ParticleSystem>();
        hasItem = false;
    }

    // some use collisions..
    private void OnCollisionEnter(Collision other)
    {
        HandleEnteredObj(other.gameObject);
    }

    // ..and others use triggers
    private void OnTriggerEnter(Collider other) 
    {
        HandleEnteredObj(other.gameObject);
    }

    private void HandleEnteredObj(GameObject other) 
    {
        if (!hasItem && other.CompareTag("Tradeable"))
        {
            hasItem = true;
            TryTrade(other);
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

        Vector3 initialVelocity = GetInitialVelocity();
         
        if (canTrade)
        {
            Destroy(givenItem);
            StartCoroutine(TradeItemWithDelay(returned, initialVelocity));
        }
        else
        {
            StartCoroutine(ReturnItemWithDelay(givenItem, initialVelocity));
        }
    }

    // acceptable item
    private IEnumerator TradeItemWithDelay(GameObject item, Vector3 initialVelocity)
    {
        if (particles) 
        {
            var main = particles.main;
            main.simulationSpeed *= 4f;
        }

        yield return new WaitForSeconds(2f);

        AkSoundEngine.PostEvent(isCampfire ? "sizzleNoise" : "cashRegister", gameObject);

        GameObject returnedItem = Instantiate(item, transform.position, Quaternion.identity);
        Physics.IgnoreCollision(returnedItem.GetComponent<Collider>(), GetComponent<Collider>());
        returnedItem.GetComponent<Rigidbody>().AddForce(initialVelocity, ForceMode.Impulse);


        if (particles) 
        {
            var main = particles.main;
            main.simulationSpeed /= 4f;
        }

        StartCoroutine(WaitAndReset());
    }

    // unacceptable item
    private IEnumerator ReturnItemWithDelay(GameObject item, Vector3 initialVelocity) 
    {
        Physics.IgnoreCollision(item.GetComponent<Collider>(), GetComponent<Collider>());
        item.transform.position = transform.position;
        item.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(0.5f);

        item.GetComponent<Rigidbody>().AddForce(initialVelocity, ForceMode.Impulse);

        StartCoroutine(WaitAndReset());
    }

    // additional item that needs to be rejected
    //private void RejectItem(GameObject item) 
    //{
    //    Vector3 initialVelocity = GetInitialVelocity();
    //    item.GetComponent<Rigidbody>().velocity = initialVelocity;
    //}

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(1f);
        hasItem = false;
    }

    private Vector3 GetInitialVelocity()
    {
        Vector3 velocity = (player.transform.position - transform.position) / 2;
        velocity.y = 10f;
        return velocity;
    }
}
