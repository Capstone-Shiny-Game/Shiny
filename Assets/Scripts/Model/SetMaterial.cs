using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterial : MonoBehaviour
{
    public GameObject obj;
    public Material[] materials;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           SetObjectMaterial(i++);
        }
    }
    public void SetObjectMaterial(int id)
    {
        obj.GetComponent<MeshRenderer>().material = materials[id % materials.Length];
    }
}
