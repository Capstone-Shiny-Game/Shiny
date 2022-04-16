using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTrade_controller : MonoBehaviour
{
    public GameObject[] acceptedPaymentPrefabs;
    public bool isCampfire;
    [SerializeField] private GameObject forSale;
    [SerializeField] Vector3 spawnVector;
    
    [SerializeField] private bool potion;

    private bool wait = false;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tradeable")  && checkAcceptedPayment(other.gameObject) && !wait)
        {
                wait = true;
                if(isCampfire)
                {
                    AkSoundEngine.PostEvent("sizzleNoise", gameObject);
                }
                else
                {
                    AkSoundEngine.PostEvent("cashRegister", gameObject);
                }
                Instantiate(forSale, (gameObject.transform.position + spawnVector), Quaternion.identity);
                Destroy(other.gameObject);
                StartCoroutine(WaitTrade());        
        }
        else
        {
            other.transform.position = gameObject.transform.position + spawnVector;
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
