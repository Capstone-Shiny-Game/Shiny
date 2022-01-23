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
            StartCoroutine(moveDown());
        }

        
    }

    IEnumerator moveDown()
    {
        int move = 1;
        while (move < 7)
        {
            transform.position = new Vector3(transform.position.x, (transform.position.y - .1f), transform.position.z);
            move++ ;
            yield return new WaitForSeconds(.5f);
        }
    }
    

 
}
