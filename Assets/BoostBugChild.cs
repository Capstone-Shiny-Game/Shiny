using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoostBugChild : MonoBehaviour
{
    public UnityEvent RespawnME;
    private void Start()
    {
        if (RespawnME == null)
            RespawnME = new UnityEvent();

    }
    private void OnDisable()
    {
        RespawnME.Invoke();
    }
}
