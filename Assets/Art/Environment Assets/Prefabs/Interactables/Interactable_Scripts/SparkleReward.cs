using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleReward : MonoBehaviour
{
    bool wait = false;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !wait)
        {
            GetComponentInChildren<ParticleSystem>().Play();
            AkSoundEngine.PostEvent("ambientShine", gameObject);
            wait = true;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10);
        wait = false;
    }

}
