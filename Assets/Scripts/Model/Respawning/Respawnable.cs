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
    public delegate void OnDisableCallback();
    [field: HideInInspector] public OnDisableCallback onDisableCallbackFunction;

    /// <summary>
    /// when this object is no longer active, let the parent know so that it can handle the reactivation.
    /// this is done because monobehaviors are disabled when the gameobject is deactivated, thus it can't reactivate itself.
    /// note: this is also called when this object is destroyed
    /// </summary>
    private void OnDisable()
    {
        if (onDisableCallbackFunction is null)
        {
            Debug.LogError("error with assigning callback function to respawnable object");
            return;
        }
        onDisableCallbackFunction();
    }

    
}
