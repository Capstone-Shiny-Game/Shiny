using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ItemCustomizable : MonoBehaviour
{


    public PatioUtility.Furniture name;
    private SetMaterial PatioGUI;
    public void Start()
    {
        PatioGUI = GameObject.Find("CanvasControls").GetComponent<SetMaterial>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             PatioGUI.ShowMaterials(name);

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
