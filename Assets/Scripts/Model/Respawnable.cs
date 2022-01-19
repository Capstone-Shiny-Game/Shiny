using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// this script should be used as a component for the object that is going to be respawning.
/// the above object should have a parent with RespawnChildInWorld script attached as a component.
/// that script will activate and deactivate this gameobject.
/// </summary>
public class Respawnable : MonoBehaviour
{
    [field: HideInInspector] public UnityEvent RespawnME;
    private void Start()
    {
        if (RespawnME == null)
            RespawnME = new UnityEvent();

    }
    /// <summary>
    /// when this object is no longer active, let the parent know so that it can handle the reactivation.
    /// this is done because monobehaviors are disabled when the gameobject is deactivated, thus it can't reactivate its self.
    /// </summary>
    private void OnDisable()
    {
        RespawnME.Invoke();
    }
}
