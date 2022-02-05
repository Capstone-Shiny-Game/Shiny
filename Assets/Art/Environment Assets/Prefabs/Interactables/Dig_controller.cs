using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig_controller : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Vector3 spawnLocation = Vector3.zero;


    // when the button is pressed
    public void OnButtonInteraction()
    {
        animator.SetTrigger("Dig");
        Destroy(GetComponentInChildren<Canvas>().gameObject);
        var sphere = GetComponentInChildren<MeshRenderer>().gameObject;
        GetComponentInChildren<ParticleSystem>().Play();
    }

    /// <summary>
    /// Okay. So. Unity is kinda dumb and doesn't have a way of setting
    /// callbacks for when animations are done playing programmatically.
    /// Instead, the workaround used here involves going into the animator
    /// object (the same one passed to this class), and clicking "Add
    /// behaviour". From here, a script is added which extends
    /// StateMachineBehaviour. There's a method called `OnStateExit` which can
    /// be overridden to essentially use as a callback. In there, you can define
    /// a reference to a class (like this one) and call a method within that class.
    /// For an example, see `DigFinishPlaying.cs`
    /// </summary>
    public void spawnObject()
    {
        var sphere = GetComponentInChildren<MeshRenderer>().gameObject;
        Destroy(sphere);
        Instantiate(objectToSpawn, transform.position + spawnLocation, transform.rotation);
    }
}
