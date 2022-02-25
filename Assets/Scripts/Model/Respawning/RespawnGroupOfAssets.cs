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
    [Tooltip("The maximum number of items to spawn. 0 means as many as possible (no more than the number of childen).")]
    public int totalMaxAmountToSpawn = 0;
    private List<Vector3> respawnLocations;
    private List<Quaternion> respawnRotations;
    private float totalProbability;

    [Tooltip("Put the prefab to spawn in the game object slot, then set the spawn parameters for that prefab")]
    [field: SerializeField] public SerializableDictionary<GameObject, SpawnParameters> PrefabsToSpawnToSpawnParameters;
    private Dictionary<GameObject,int> prefabToCurrentNumSpawned; //tracks the current spawned objects (how many of each game object have been spawned)
    [System.Serializable]
    public struct SpawnParameters
    {
        public int maxAmountToSpawn;
        public float spawnProbability;
    }


    void Start()
    {
        //get the player
        playerReference = GameObject.FindGameObjectWithTag("Player");

        //unity was being weird about using the transforms of deactivated objects so make a copy.
        List<Transform> respawnTransforms = new List<Transform>(gameObject.GetComponentsInChildren<Transform>());
        respawnTransforms.Remove(this.transform);
        respawnLocations = new List<Vector3>();
        respawnRotations = new List<Quaternion>();
        foreach (Transform transform in respawnTransforms)
        {
            respawnLocations.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            respawnRotations.Add(new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
            Destroy(transform.gameObject);
        }
        //Debug.Log("respawn locations number : " + respawnLocations.Count);
        //setup max number to spawn and dictionary
        int maxSpawnable = 0;
        prefabToCurrentNumSpawned = new Dictionary<GameObject, int>(); //tracks the current spawned objects
        foreach (KeyValuePair<GameObject, SpawnParameters> entry in PrefabsToSpawnToSpawnParameters)
        {
            prefabToCurrentNumSpawned[entry.Key] = 0; //zero of each spawned at the start
            maxSpawnable += entry.Value.maxAmountToSpawn; //sum max number of each object
        }
        //setup total probability (doesnt normalize)
        totalProbability = 0;
        calculateTotalProbability();
        //safety check,if total max amt to spawn is negative or higher than max spawn locations
        if (totalMaxAmountToSpawn <= 0 || totalMaxAmountToSpawn >= respawnLocations.Count)
        {
            totalMaxAmountToSpawn = respawnLocations.Count; //set it to max
        }
        totalMaxAmountToSpawn = Mathf.Min(maxSpawnable, totalMaxAmountToSpawn);//second saftey check
        //setup done, now spawn the items
        for (int i = 0; i < totalMaxAmountToSpawn; i++)
        {
            //Debug.Log(i);
            respawnItemWithProbability();
        }
    }

    /// <summary>
    /// sums up the total probability of each object (apples at .5 + acorn at 1 = 1.5)
    /// skips items that are already spawned at their max count
    /// </summary>
    private void calculateTotalProbability()
    {
        foreach (KeyValuePair<GameObject, SpawnParameters> entry in PrefabsToSpawnToSpawnParameters)
        {
            if (entry.Value.maxAmountToSpawn == prefabToCurrentNumSpawned[entry.Key])//skip items that are already spawned at their max count
            {
                continue;
            }
            totalProbability += entry.Value.spawnProbability;
        }
    }

    private void respawnItemWithProbability()
    {
        int LocationIndex = Random.Range(0, totalMaxAmountToSpawn); //get a random availible spawn location
        Vector3 placeToSpawn = respawnLocations[LocationIndex];
        Quaternion rotation = respawnRotations[LocationIndex];
        //mark spawn location no longer availible
        respawnLocations.RemoveAt(LocationIndex);
        respawnRotations.RemoveAt(LocationIndex);
        //Debug.Log("spawn location : " + placeToSpawn);
        //spawn new object
        GameObject newObject = Instantiate(GetPrefabToSpawn(), placeToSpawn, rotation);
        newObject.SetActive(true);
        //add the respawnable script to it with a callback to free up it's respawn location if it is destroyed
        Respawnable script = newObject.AddComponent<Respawnable>();
        script.onDisableCallbackFunction = delegate () { respawnLocations.Add(placeToSpawn); respawnRotations.Add(rotation); StartRespawn(); };
    }

    private GameObject GetPrefabToSpawn() {
        calculateTotalProbability();
        float prefabToSpawn = Random.Range(0, totalProbability);//pick a random prefab based on probabilities
        GameObject result = null;
        foreach (KeyValuePair<GameObject, SpawnParameters> entry in PrefabsToSpawnToSpawnParameters)
        {
            result = entry.Key;//default
            if (entry.Value.maxAmountToSpawn == prefabToCurrentNumSpawned[result]) {//skip things already at max count (same as in calc probability)
                continue;
            }
            if (prefabToSpawn <= entry.Value.spawnProbability) {//found it
                break;
            }
            prefabToSpawn -= entry.Value.spawnProbability;//subtract off this item's probability 
        }
        prefabToCurrentNumSpawned[result]++;//track spawning this specific prefab
        return result;//return found prefab
    }

    /// <summary>
    /// calls IEnumerator Respawn()
    /// </summary>
    void StartRespawn()
    {
        // prevents exception when game is closed
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine("Respawn");
        }
    }
    /// <summary>
    /// respawns items with specified probability after the respawn time
    /// </summary>
    /// <returns></returns>
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        // prevents exception when game is closed
        if (this.gameObject.activeInHierarchy)
        {
            respawnItemWithProbability();
        }
    }
}
