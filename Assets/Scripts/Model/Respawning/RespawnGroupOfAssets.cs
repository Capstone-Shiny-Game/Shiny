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
    private float totalProbability;

    [Tooltip("Put the prefab to spawn in the game object slot, then set the spawn parameters for that prefab")]
    [field: SerializeField] public SerializableDictionary<GameObject, SpawnParameters> PrefabsToSpawnToSpawnParameters;
    private Dictionary<GameObject,int> prefabToCurrentNumSpawned;
    [System.Serializable]
    public struct SpawnParameters
    {
        public int maxAmountToSpawn;
        public float spawnProbability;
    }


    void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        List<Transform> respawnTransforms = new List<Transform>(gameObject.GetComponentsInChildren<Transform>());
        respawnTransforms.Remove(this.transform);
        respawnLocations = new List<Vector3>();
        foreach (Transform transform in respawnTransforms) {
            respawnLocations.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            Destroy(transform.gameObject);
        }
        Debug.Log("respawn locations number : " + respawnLocations.Count);
        totalProbability = 0;
        int maxSpawnable = 0;
        foreach (KeyValuePair<GameObject, SpawnParameters> entry in PrefabsToSpawnToSpawnParameters)
        {
            totalProbability += entry.Value.spawnProbability;
            maxSpawnable += entry.Value.maxAmountToSpawn;
        }
        if (totalMaxAmountToSpawn <= 0 || totalMaxAmountToSpawn >= respawnLocations.Count)
        {
            totalMaxAmountToSpawn = respawnLocations.Count;
        }
        totalMaxAmountToSpawn = Mathf.Min(maxSpawnable, totalMaxAmountToSpawn);
        for (int i = 0; i < totalMaxAmountToSpawn; i++)
        {
            Debug.Log(i);
            respawnItemWithProbability();
            
        }
    }

    private void respawnItemWithProbability()
    {
        int LocationIndex = Random.Range(0, totalMaxAmountToSpawn); //get a random availible spawn location
        Vector3 placeToSpawn = respawnLocations[LocationIndex];
        respawnLocations.RemoveAt(LocationIndex);//mark it no longer availible
        Debug.Log("spawn location : " + placeToSpawn);
        //spawn new object
        GameObject newObject = Instantiate(GetPrefabToSpawn(), placeToSpawn, Quaternion.identity);
        newObject.SetActive(true);
        //add the respawnable script to it with a callback to free up it's respawn location if it is destroyed
        Respawnable script = newObject.AddComponent<Respawnable>();
        script.onDisableCallbackFunction = delegate () { respawnLocations.Add(placeToSpawn); StartRespawn(); };
    }

    private GameObject GetPrefabToSpawn() {
        float prefabToSpawn = Random.Range(0, totalProbability);
        GameObject result = null;
        foreach (KeyValuePair<GameObject, SpawnParameters> entry in PrefabsToSpawnToSpawnParameters)
        {
            result = entry.Key;
            if (prefabToSpawn <= entry.Value.spawnProbability) {
                break;
            }
            prefabToSpawn -= entry.Value.spawnProbability;
        }
        prefabToCurrentNumSpawned[result]++;//TODO: add in check to make sure that we are not at max spawns
        return result;
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
            respawnItemWithProbability();
        }
    }
}
