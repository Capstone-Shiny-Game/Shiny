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
    private List<bool> spawnedPrefabs;
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
            spawnedPrefabs[i] = false;
            availableToSpawn[i] = i;
            prefabs[i].SetActive(false);
        }
        for (int i = 0; i < maxAmountToSpawn; i++)
        {
            int prefabIndex = Random.Range(0, maxAmountToSpawn);
            int availableIndex = Random.Range(0, availableToSpawn.Count);
            GameObject prefabLocation = prefabs[availableToSpawn[availableIndex]];
            Transform placeToSpawn = prefabLocation.GetComponent<Transform>();
            GameObject newObject = Instantiate(prefabs[prefabIndex], placeToSpawn);
            newObject.SetActive(true);
            Respawnable script = newObject.AddComponent<Respawnable>();
            script.RespawnME = new UnityEvent();
            script.RespawnME.AddListener(StartRespawn);
            script.index = availableIndex;
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
