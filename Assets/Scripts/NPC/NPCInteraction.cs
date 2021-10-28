using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    // separate delegate to pass NPC position for Player position relocation?

    public delegate void PlayerCollided(Vector3 npcPosition);
    public static event PlayerCollided OnPlayerCollided;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (OnPlayerCollided != null)
            {
                OnPlayerCollided(this.transform.position + this.transform.forward * 3.0f);
            }
        }
    }
}
