using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCustomizable : MonoBehaviour
{
    public string name;
    private SetMaterial PatioGUI;
    public void Start()
    {
        PatioGUI = GameObject.FindGameObjectWithTag("UIPatio").GetComponent<SetMaterial>();
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
