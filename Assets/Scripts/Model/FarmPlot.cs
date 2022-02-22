using System.Collections.Generic;
using UnityEngine;

public class FarmPlot : MonoBehaviour
{
    /* Grows different plants (meshes) based on the seed it's given.
     */

    public SerializableDictionary<string, List<string>> itemMeshes;

    private GameObject s1, s2, s3;
    private int currMesh;


    private void Start()
    {
        s1 = transform.Find("stage1").GetChild(0).gameObject;
        s2 = transform.Find("stage2").GetChild(0).gameObject;
        s3 = transform.Find("stage3").GetChild(0).gameObject;
        currMesh = -1;

        // TEST
        TryInstantiatePlot("Acorn_Prefab");
    }

    private void OnEnable()
    {
        DayController.OnMidDayEvent += LoadNextMesh;
    }

    private void OnDisable()
    {
        DayController.OnMidDayEvent -= LoadNextMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tradeable"))
        {
            TryInstantiatePlot(other.name);
        }
    }

    private void TryInstantiatePlot(string objName)
    {
        List<string> meshNames = new List<string>();
        bool isProperItem = false;
        foreach (string key in itemMeshes.Keys)
        {
            // the item given to the plot is registered and
            // has exactly 3 meshes to cycle thru
            if (objName.StartsWith(key) && itemMeshes[key].Count == 3)
            {
                meshNames = itemMeshes[key];
                isProperItem = true;
                break;
            }
        }

        if (isProperItem)
        {
            s1.GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>(meshNames[0]);
            s1.SetActive(false);

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

    private void LoadNextMesh()
    {
        if (currMesh < 2)
        {
            switch (++currMesh)
            {
                case 0:
                    s1.SetActive(true);
                    break;
                case 1:
                    s1.SetActive(false);
                    s2.SetActive(true);
                    break;
                case 2:
                    s2.SetActive(false);
                    s3.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
