using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatCustomizable : MonoBehaviour
{

    public PatioUtility.Hat hatName;
    private SetMaterial PatioGUI;
    public void Start()
    {

        PatioGUI = GameObject.Find("CanvasControls").GetComponent<SetMaterial>();

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PatioGUI.ShowHatMenu();

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PatioGUI.ExitMenu();
        }
    }
}
