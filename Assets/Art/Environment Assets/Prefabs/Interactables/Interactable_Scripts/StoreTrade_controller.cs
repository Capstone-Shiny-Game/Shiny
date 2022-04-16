using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTrade_controller : MonoBehaviour
{
    public GameObject[] acceptedPaymentPrefabs;
    [SerializeField] private GameObject forSale;
    [SerializeField] private GameObject spawnLocation;

    private bool wait = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tradeable")  && checkAcceptedPayment(other.gameObject) && !wait)
        {

            
                wait = true;

                Instantiate(forSale, (gameObject.transform.position + new Vector3(4, 4 , 4)), Quaternion.identity);
                Destroy(other.gameObject);
                StartCoroutine(WaitTrade());
            
        }
        else
        {
            other.transform.position = gameObject.transform.position + new Vector3(4, 4, 4);
        }

    }

    private bool checkAcceptedPayment(GameObject depositedObject)
    {
        foreach (GameObject obj in acceptedPaymentPrefabs)
        {
            if (depositedObject.name.Contains(obj.name))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator WaitTrade()
    {
        yield return new WaitForSeconds(1);
        wait = false;
    }
}
