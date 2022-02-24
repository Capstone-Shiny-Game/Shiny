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
        Debug.Log("HIT BATH");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Choose material");
             PatioGUI.ShowMaterials(name);
            PatioGUI.gameObject.SetActive(true);

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PatioGUI.gameObject.SetActive(false);
           
        }
    }
}
