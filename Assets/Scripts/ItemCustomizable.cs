using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCustomizable : MonoBehaviour
{
    public string name;
    private SetMaterial PatioGUI;
    public void Start()
    {
        PatioGUI = GameObject.Find("PatioMenu").GetComponent<SetMaterial>();

    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT BATH");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Choose material");
             PatioGUI.ShowMaterials(name);
        }
    }
}
