using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmPlot : MonoBehaviour
{
    /* Grows different plants (meshes) based on the seed it's given.
     */

    public SerializableDictionary<string, List<string>> files = new SerializableDictionary<string, List<string>>()
        {
            { "exampleItemName", new List<string> { "Weed_ex", "Wheat_ex", "Tulip_ex" } }
        };


    private GameObject s1, s2, s3;

    private void Start()
    {
        s1 = transform.Find("stage1").GetChild(0).gameObject;
        s2 = transform.Find("stage2").GetChild(0).gameObject;
        s3 = transform.Find("stage3").GetChild(0).gameObject;

        // TEST
        InstantiatePlot("Acorn_Prefab");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tradeable"))
        {
            InstantiatePlot(other.name);
        }
    }

    private void InstantiatePlot(string objName)
    {
        List<string> meshNames = new List<string>();
        bool isProperItem = false;
        foreach (string key in files.Keys)
        {
            // the item given to the plot is registered and
            // has exactly 3 meshes to cycle thru
            if (objName.StartsWith(key) && files[key].Count == 3)
            {
                meshNames = files[key];
                isProperItem = true;
                break;
            }
        }

        if (isProperItem)
        {
            s1.GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>(meshNames[0]);
            s1.SetActive(true);
            s2.GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>(meshNames[1]);
            s2.SetActive(false);
            s3.GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>(meshNames[2]);
            s3.SetActive(false);
        }
        else
        {
            Debug.Log("OnTriggerEnter(): You've tried to plant an unexpected item");
        }
    }
}
