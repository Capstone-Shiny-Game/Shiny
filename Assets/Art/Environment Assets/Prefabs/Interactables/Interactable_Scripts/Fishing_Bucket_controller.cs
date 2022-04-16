using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing_Bucket_controller : MonoBehaviour
{
    public List<GameObject> fishable;
    [SerializeField] GameObject Bucket;
    [SerializeField] GameObject fishMesh;
    
    // [SerializeField] Vector3 spawnPosition;



    [SerializeField] GameObject DropButton;
    [SerializeField] GameObject PickupButton;

    private GameObject crowPlayer;
    private GameObject crowPos;
    private bool full = false;
    private GameObject fished;
    private Vector3 spawnLocation;

    private void Start()
    {
        crowPlayer = GameObject.FindGameObjectWithTag("Player");
        crowPos = GameObject.Find("L_footCTR");
        spawnLocation = gameObject.transform.position;
        
    }

   


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //enable water in bucket
        }

        if (other.CompareTag("FishingSpot"))
        {

            if (other.transform.parent.name.ToLower().Contains("fish") && !full) {

                fishMesh.SetActive(true);
                //fished = Instantiate<GameObject>(, (Bucket.transform.position + spawnPosition), Quaternion.identity, Bucket.transform);
                fished = fishable[Random.Range(0, fishable.Count - 1)];
               // fished.transform.localScale = new Vector3(2, 2, 2);
            /*    
                
                fished.GetComponent<CapsuleCollider>().enabled = false;
                fished.GetComponent<Rigidbody>().isKinematic = false;
                fished.GetComponent<Rigidbody>().useGravity = false;
                fished.GetComponent<SphereCollider>().enabled = false;*/
                Destroy(other.gameObject.transform.parent.gameObject);
                full = true;
                AkSoundEngine.PostEvent("fishSplash", gameObject);
            }

           if ((other.transform.parent.name.ToLower().Contains("cooler")) && full)
            {
                Debug.Log("cooler");

                //  fished.transform.parent = null;
                Instantiate<GameObject>(fished, (other.transform.position + new Vector3(3, 3, 3)), Quaternion.identity);
               // Debug.Log(other.transform.position);
               // fished.transform.position = other.transform.position + new Vector3(2,2,2);
                /* fished.GetComponent<CapsuleCollider>().enabled = true;
                 fished.GetComponent<Rigidbody>().useGravity = true;
                 fished.GetComponent<SphereCollider>().enabled = true;*/
                fishMesh.SetActive(false);
                full = false;
                
             
                
            }
            

        }

/*
        if (other.CompareTag("Player") && pickup)
        {
            Debug.Log("bucket");
            gameObject.transform.parent = other.transform;
            gameObject.transform.localPosition = new Vector3(0, 1, 0);

            DropButton.GetComponentInChildren<Canvas>().gameObject.SetActive(true);
            PickupButton.GetComponentInChildren<Canvas>().gameObject.SetActive(false);

            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;


        }
*/
        
        

    }

    public void DropBucket()
    {

        DropButton.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        PickupButton.GetComponentInChildren<Canvas>().gameObject.SetActive(true);

        gameObject.transform.parent = null;
        
        if (crowPlayer.GetComponent<PlayerController>().state != PlayerController.CrowState.Flying)
        {
            gameObject.transform.position = crowPlayer.transform.position + new Vector3(0, 2, -3);
        }

        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
       


    }
    
    public void PickupBucket()
    {


        DropButton.GetComponentInChildren<Canvas>().gameObject.SetActive(true);
        PickupButton.GetComponentInChildren<Canvas>().gameObject.SetActive(false);

        gameObject.transform.rotation = Quaternion.identity;
        //gameObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
        gameObject.transform.parent = crowPos.transform;
        gameObject.transform.localPosition = new Vector3(0, -.25f, 0);

        

        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
    }

   
    
    private void OnEnable()
    {
        DayController.OnDayStartEvent += returnToSpawn;
    }

    private void OnDisable()
    {
        DayController.OnDayStartEvent -= returnToSpawn;
    }

    private void returnToSpawn()
    {
        if(gameObject.transform.parent == null)
        {
            gameObject.transform.position = spawnLocation;
            gameObject.transform.rotation = Quaternion.identity;
        }
        
    }

}
