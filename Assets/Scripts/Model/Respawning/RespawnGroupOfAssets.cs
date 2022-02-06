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
    private List<Transform> respawnLocations;
    private float TotalProbability;

    public Dictionary<GameObject, SpawnParameters> PrefabsToSpawnToSpawnParameters;
    [System.Serializable]
    public struct SpawnParameters
    {
        public int maxAmountToSpawn;
        public float spawnProbability;
        [HideInInspector] public int currentNumSpawned;
    }


    void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        respawnLocations = new List<Transform>(gameObject.GetComponentsInChildren<Transform>());
        if (totalMaxAmountToSpawn <= 0 || totalMaxAmountToSpawn >= respawnLocations.Count)
        {
            totalMaxAmountToSpawn = respawnLocations.Count;
        }
        TotalProbability = 0;
        foreach (KeyValuePair<GameObject, SpawnParameters> entry in PrefabsToSpawnToSpawnParameters)
        {
            TotalProbability += entry.Value.spawnProbability;
        }
        
        for (int i = 0; i < respawnLocations.Count; i++)
        {
            respawnLocations[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < totalMaxAmountToSpawn; i++)
        {
            respawnItemWithProbability();
        }
    }

    private void respawnItemWithProbability()
    {
        int LocationIndex = Random.Range(0, totalMaxAmountToSpawn); //get a random availible spawn location
        Transform placeToSpawn = respawnLocations[LocationIndex];
        respawnLocations.RemoveAt(LocationIndex);//mark it no longer availible
        //spawn new object
        GameObject newObject = GetPrefabToSpawn();
        newObject.SetActive(true);
        //add the respawnable script to it with a callback to free up it's respawn location if it is destroyed
        Respawnable script = newObject.AddComponent<Respawnable>();
        script.onDisableCallbackFunction = delegate () { respawnLocations.Add(placeToSpawn); StartRespawn(); };
    }

    private GameObject GetPrefabToSpawn() {
        float prefabToSpawn = Random.Range(0, TotalProbability);
        GameObject result = null;
        foreach (KeyValuePair<GameObject, SpawnParameters> entry in PrefabsToSpawnToSpawnParameters)
        {
            result = entry.Key;
            if (prefabToSpawn <= entry.Value.spawnProbability) {
                return result;
            }
            prefabToSpawn -= entry.Value.spawnProbability;
        }
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
