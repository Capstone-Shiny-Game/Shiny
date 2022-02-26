using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTempNPCs : MonoBehaviour
{
    public SerializableDictionary<Transform, GameObject> NPCandSpawnPos;
    
    void Start()
    {
        DayController.OnDayStartEvent += respawnNPC;
    }

    private void respawnNPC()
    {
        foreach(Transform npc in NPCandSpawnPos.Keys)
        {
            if(npc.gameObject.GetComponentInChildren<SphereCollider>() == null)
            {
                Destroy(npc.transform.GetChild(0).gameObject);
                Instantiate<GameObject>(NPCandSpawnPos[npc], npc.position, Quaternion.Euler(0,0,0), npc).transform.localRotation = Quaternion.Euler(0,0,0);
            }
        }
    }
}
