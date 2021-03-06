using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnGroupOfAssets : MonoBehaviour
{

    private GameObject playerReference;
    [Tooltip("time in seconds, max is 5 irl days"), Range(0.0000001f, 432000f)]
    public float respawnTimeSeconds = 60;
    // TODO: deal with pop-in issues
    //public float minDistanceToRespawn = 100;
    [Tooltip("if this is enabled, you have to add the group respawnable script to children of this object that are acting as respawn points")]
    public bool useGroupRespawnMarkers = true;
    [Tooltip("The maximum number of items to spawn. 0 means as many as possible (no more than the number of childen).")]
    public int totalMaxAmountToSpawn = 0;
    private List<Vector3> respawnLocations;
    private List<Quaternion> respawnRotations;
    private List<Vector3> respawnScales;
    private float totalProbability;
    private List<Coroutine> coroutines;

    [Tooltip("Put the prefab to spawn in the game object slot, then set the spawn parameters for that prefab")]
    [field: SerializeField] public SerializableDictionary<GameObject, SpawnParameters> PrefabsToSpawnToSpawnParameters;
    private Dictionary<GameObject, int> prefabToCurrentNumSpawned; //tracks the current spawned objects (how many of each game object have been spawned)
    [System.Serializable]
    public struct SpawnParameters
    {
        public int maxAmountToSpawn;
        public float spawnProbability;
    }


    void Start()
    {
        coroutines = new List<Coroutine>();
        //get the player
        playerReference = GameObject.FindGameObjectWithTag("Player");
        List<Transform> respawnTransforms = new List<Transform>();
        if (useGroupRespawnMarkers)
        {
            foreach (GroupRespawnPoint respawnPoint in gameObject.GetComponentsInChildren<GroupRespawnPoint>())
            {
                respawnTransforms.Add(respawnPoint.gameObject.transform);
            }
        }
        else
        {
            respawnTransforms = new List<Transform>(gameObject.GetComponentsInChildren<Transform>());
            respawnTransforms.Remove(this.transform); //remove self from list
        }
        //unity was being weird about using the transforms of deactivated objects so make a copy.
        respawnLocations = new List<Vector3>();
        respawnRotations = new List<Quaternion>();
        respawnScales = new List<Vector3>();
        foreach (Transform transform in respawnTransforms)
        {
            respawnLocations.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            respawnRotations.Add(new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
            respawnScales.Add(new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
            transform.gameObject.SetActive(false);
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
        //totalMaxAmountToSpawn = Mathf.Min(maxSpawnable, totalMaxAmountToSpawn);
        //Debug.Log("if " + totalMaxAmountToSpawn +" >= " + respawnLocations.Count);
        if (totalMaxAmountToSpawn <= 0 || totalMaxAmountToSpawn >= respawnLocations.Count)
        {
            totalMaxAmountToSpawn = respawnLocations.Count; //set it to max
        }
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
        int LocationIndex = Random.Range(0, respawnLocations.Count); //get a random availible spawn location
        Vector3 placeToSpawn = respawnLocations[LocationIndex];
        Quaternion rotation = respawnRotations[LocationIndex];
        Vector3 scale = respawnScales[LocationIndex];
        //mark spawn location no longer availible
        respawnLocations.RemoveAt(LocationIndex);
        respawnRotations.RemoveAt(LocationIndex);
        respawnScales.RemoveAt(LocationIndex);
        //Debug.Log("spawn location : " + placeToSpawn);
        //spawn new object
        GameObject newObject = Instantiate(GetPrefabToSpawn(), placeToSpawn, rotation);
        newObject.transform.localScale = scale;
        newObject.SetActive(true);
        //add the respawnable script to it with a callback to free up it's respawn location if it is destroyed
        Respawnable script = newObject.AddComponent<Respawnable>();
        script.onDisableCallbackFunction = delegate ()
        {
            respawnLocations.Add(placeToSpawn);
            respawnRotations.Add(rotation);
            respawnScales.Add(scale);
            try
            {
                StartRespawn();
                Destroy(newObject);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                return;
            };
        };
    }

    private GameObject GetPrefabToSpawn()
    {
        calculateTotalProbability();
        float prefabToSpawn = Random.Range(0, totalProbability);//pick a random prefab based on probabilities
        GameObject result = null;
        foreach (KeyValuePair<GameObject, SpawnParameters> entry in PrefabsToSpawnToSpawnParameters)
        {
            result = entry.Key;//default
            if (entry.Value.maxAmountToSpawn == prefabToCurrentNumSpawned[result])
            {//skip things already at max count (same as in calc probability)
                continue;
            }
            if (prefabToSpawn <= entry.Value.spawnProbability)
            {//found it
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
            coroutines.Add(StartCoroutine("Respawn"));
        }
    }
    /// <summary>
    /// respawns items with specified probability after the respawn time
    /// </summary>
    /// <returns></returns>
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTimeSeconds);
        // prevents exception when game is closed
        if (this.gameObject.activeInHierarchy)
        {
            respawnItemWithProbability();
        }
    }
}
