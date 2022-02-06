using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSingleAssetInWorld : MonoBehaviour
{
    [Tooltip("this component is used to reactivate(respawn) an object that is deactivated.")]
    public float respawnTime;
    [Tooltip("the object to reactivate after the respawn time completes.")]
    public Respawnable respawnable;
    void Start()
    {
        respawnable.onDisableCallbackFunction = delegate () { StartRespawn(); };
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
