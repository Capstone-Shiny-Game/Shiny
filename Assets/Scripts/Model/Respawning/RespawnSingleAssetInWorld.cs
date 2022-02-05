using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnChildInWorld : MonoBehaviour
{
    public float respawnTime;
    public Respawnable respawnable; //the child object to respawn
    void Start()
    {
        respawnable.RespawnME.AddListener(StartRespawn);
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
            respawnable.gameObject.SetActive(true);
        }
    }
}
