using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressureplate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickupable"))
        {

            GetComponent<Collider>().enabled = false;
            StartCoroutine(moveDown(transform.position, new Vector3(transform.position.x, (transform.position.y - .7f), transform.position.z)));
        }


    }

    IEnumerator moveDown(Vector3 oldPos, Vector3 newPos)
    {
        //  int move = 1;
        // while (move < 7)
        // {
        //    transform.position = new Vector3(transform.position.x, (transform.position.y - .1f), transform.position.z);
        //     move++ ;
        //     yield return new WaitForSeconds(.5f);
        //}

        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / 3;
            transform.position = Vector3.Lerp(oldPos, newPos, t);

            yield return null;
        }

    }



}
