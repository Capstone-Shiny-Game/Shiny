using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBug : MonoBehaviour
{
    public float frequency;
    public float movementRange;
    public float respawnTime;
    public BoostBugChild Sparkles;
    void Start()
    {
        Sparkles.RespawnME.AddListener(StartRespawn);
    }
    void StartRespawn()
    {
        if (this.gameObject.activeInHierarchy)//prevents exception when game is closed
        {
            StartCoroutine("Respawn");
        }
    }

    void Update()
    {
        Sparkles.transform.localPosition = new Vector3(0, UpDown(), 0);
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        if (this.gameObject.activeInHierarchy)//prevents exception when game is closed
        {
            Sparkles.gameObject.SetActive(true);
        }
    }
    public float UpDown() {
        return movementRange * Mathf.Sin(frequency * (Time.time));
    }
}
