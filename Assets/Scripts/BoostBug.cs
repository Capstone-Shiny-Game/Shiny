using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBug : MonoBehaviour
{
    // Start is called before the first frame update

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
        StartCoroutine("Respawn");
    }

    // Update is called once per frame
    void Update()
    {
        Sparkles.transform.localPosition = new Vector3(0, UpDown(), 0);
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        Sparkles.gameObject.SetActive(true);
        Debug.Log("BoostBug");
    }


    public float UpDown() {
        return movementRange * Mathf.Sin(frequency * (Time.time));
    }
}
