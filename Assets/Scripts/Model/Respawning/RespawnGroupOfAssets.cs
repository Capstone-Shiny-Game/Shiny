using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnGroupOfAssets : MonoBehaviour
{
    //TODO deal with pop-in issues
    public float respawnTime;
    public List<Respawnable> respawnable; //the child objects to respawn
    void Start()
    {
        //respawnable.RespawnME.AddListener(StartRespawn);
    }
    void StartRespawn()
    {
        if (this.gameObject.activeInHierarchy)//prevents exception when game is closed
        {
            StartCoroutine("Respawn");
        }
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        if (this.gameObject.activeInHierarchy)//prevents exception when game is closed
        {
            //respawnable.gameObject.SetActive(true);
        }
    }
}
