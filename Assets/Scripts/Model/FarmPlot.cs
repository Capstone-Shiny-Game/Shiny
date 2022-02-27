using System.Collections.Generic;
using UnityEngine;

public class FarmPlot : MonoBehaviour
{
    /* Grows different plants (meshes) based on the seed it's given.
     */

    public List<SeedCropEntry> seedCropMap;

    private GameObject s1;
    private GameObject harvestButton;

    private SeedCropEntry currCrop;
    private List<Mesh> currMeshes;
    private int meshIndex;
    private bool hasCrop;


    private void Start()
    {
        s1 = transform.Find("stage").GetChild(0).gameObject;
        harvestButton = transform.Find("InteractButton").gameObject;
        meshIndex = -1;
        hasCrop = false;

        // TEST
        //TryInstantiatePlot("Acorn_Prefab");
    }

    private void OnEnable()
    {
        DayController.OnMorningEvent += LoadNextMesh;
        
    }

    private void OnDisable()
    {
        DayController.OnMorningEvent -= LoadNextMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tradeable"))
        {
            TryInstantiatePlot(other.name, out bool plantedCrop);
            if (plantedCrop)
            {
                hasCrop = true;
                Destroy(other.gameObject);
            }
        }
    }

    private void TryInstantiatePlot(string objName, out bool success)
    {
        bool isProperItem = false;
        foreach (SeedCropEntry crop in seedCropMap)
        {
            // the item given to the plot is registered and
            // has at least 1 mesh to cycle thru
            if (objName.ToLower().StartsWith(crop.seedName.ToLower()) 
                && crop.meshNames.Count > 0)
            {
                isProperItem = true;
                currCrop = crop;
                break;
            }
        }

        if (isProperItem)
        {
            currMeshes = new List<Mesh>();
            foreach (string meshName in currCrop.meshNames)
            {
                currMeshes.Add(Resources.Load<Mesh>(meshName));
            }

            s1.SetActive(true);
            LoadNextMesh();
            success = true;
        }
        else
        {
            Debug.Log("OnTriggerEnter(): You've tried to plant an unexpected item");
            success = false;
        }
    }

    private void LoadNextMesh()
    {
        if (hasCrop)
        {
            if (++meshIndex < currCrop.meshNames.Count)
            {
                // TODO: Add particle effect here
                s1.GetComponent<MeshFilter>().sharedMesh = currMeshes[meshIndex];
                foreach(flowerbedScript scr in s1.GetComponentsInChildren<flowerbedScript>())
                {
                    scr.updateMesh();
                }
            }

            // last mesh, load the interact button for harvesting
            if (meshIndex == currCrop.meshNames.Count - 1)
            {
                harvestButton.SetActive(true);
            }
        }
    }

    public void HarvestItem()
    {
        // TODO: Add particle effect here
        Instantiate(currCrop.cropObj, transform.position + new Vector3(4f, 4f, 0), transform.rotation);
        ResetFarmPlot();
    }

    private void ResetFarmPlot()
    {
        hasCrop = false;
        meshIndex = -1;
        harvestButton.SetActive(false);
        s1.GetComponent<MeshFilter>().sharedMesh = null;

    }
}
