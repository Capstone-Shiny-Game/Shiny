using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnGroupOfAssets : MonoBehaviour
{
    // TODO: deal with pop-in issues
    private GameObject playerReference;
    public float respawnTime = 60;
    public float minDistanceToRespawn = 100;
    [Tooltip("The maximum number of items to spawn. 0 means as many as possible (no more than the number of children).")]
    public int maxAmountToSpawn = 0;
    // the child objects to respawn
    private List<int> availableToSpawn;
    private List<GameObject> prefabs;
    
    void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        prefabs = new List<GameObject>(gameObject.GetComponentsInChildren<GameObject>());
        if (maxAmountToSpawn <= 0)
        {
            maxAmountToSpawn = prefabs.Count;
        }
        for (int i = 0; i < maxAmountToSpawn; i++)
        {
            availableToSpawn[i] = i;
            prefabs[i].SetActive(false);
        }
        for (int i = 0; i < maxAmountToSpawn; i++)
        {
            int prefabIndex = Random.Range(0, maxAmountToSpawn); //get a random prefab from the children of this object
            //get a random location that doesnt currently have an item in it
            int availableIndex = Random.Range(0, availableToSpawn.Count); 
            GameObject prefabLocation = prefabs[availableToSpawn[availableIndex]];
            Transform placeToSpawn = prefabLocation.GetComponent<Transform>();
            //spawn new object
            GameObject newObject = Instantiate(prefabs[prefabIndex], placeToSpawn);
            newObject.SetActive(true);
            //add the respawnable script to it with a callback to free up it's index if it is destroyed
            Respawnable script = newObject.AddComponent<Respawnable>();
            script.onDisableCallbackFunction = delegate() { availableToSpawn[availableIndex] = availableIndex; };
        }

        // respawnable.RespawnME.AddListener(StartRespawn);
    }

    void StartRespawn()
    {
        // prevents exception when game is closed
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine("Respawn");
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        // prevents exception when game is closed
        if (this.gameObject.activeInHierarchy)
        {
            // respawnable.gameObject.SetActive(true);
        }
    }
}
