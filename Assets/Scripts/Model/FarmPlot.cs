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
        //switch (objName)
        //{
        //    case string name when name.StartsWith("Acorn"):
        //        if (!hasThreeMeshes("Acorn"))
        //            break;

        //        s1.GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>("");
        //        s1.SetActive(true);
        //        s2.GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>("");
        //        s2.SetActive(false);
        //        s3.GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>("");
        //        s3.SetActive(false);

        //        break;
        //    default:
        //        Debug.Log("OnTriggerEnter(): You've tried to plant an unexpected item");
        //        break;
        //}
    }

    //private bool hasThreeMeshes(string search) => 
    //    meshFileNames.Where(name => meshFileNames.Contains(search)).ToArray().Length == 3;
}
