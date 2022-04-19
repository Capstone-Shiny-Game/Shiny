using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FarmPlot : MonoBehaviour
{
    /* Grows different plants (meshes) based on the seed it's given.
     */

    public List<SeedCropEntry> seedCropMap;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject plantedGeo;

    private GameObject s1;
    private GameObject harvestButton;

    private List<SeedCropEntry> flowerCrops;
    private List<SeedCropEntry> veggieCrops;
    private SeedCropEntry currCrop;
    private List<Mesh> currMeshes;
    private int meshIndex;
    private bool hasCrop;
    private readonly string FLOWER = "seedflower";
    private readonly string VEGGIE = "seedveg";


    private void Start()
    {
        s1 = transform.Find("stage").GetChild(0).gameObject;
        harvestButton = transform.Find("InteractButton").gameObject;
        meshIndex = -1;
        hasCrop = false;

        flowerCrops = seedCropMap.FindAll(s => s.isFlower);
        veggieCrops = seedCropMap.FindAll(s => !s.isFlower);
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
        if (other.CompareTag("Tradeable") && !hasCrop)
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
        bool isFlower = false;
        foreach (SeedCropEntry crop in seedCropMap)
        {
            // the item given to the plot is either a flower or 
            // veggie seed and has at least 1 mesh to cycle thru
            if (objName.ToLower().StartsWith(FLOWER) 
                && crop.meshNames.Count > 0)
            {
                isProperItem = true;
                isFlower = true;
                break;
            }
            else if (objName.ToLower().StartsWith(VEGGIE)
                && crop.meshNames.Count > 0) 
            {
                isProperItem = true;
                isFlower = false;
                break;
            } 
        }

        if (isProperItem)
        {
            if (isFlower)
            {
                currCrop = flowerCrops[Random.Range(0, flowerCrops.Count)];
            }
            else 
            {
                currCrop = veggieCrops[Random.Range(0, veggieCrops.Count)];
            }

            if (currCrop == null) 
            {
                Debug.Log("OnTriggerEnter(): Unexpected error");
                success = false;
                return;
            }

            currMeshes = new List<Mesh>();
            foreach (string meshName in currCrop.meshNames)
            {
                currMeshes.Add(Resources.Load<Mesh>(meshName));
            }

            if (plantedGeo != null)
            {
                plantedGeo.SetActive(true);
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
        Instantiate(currCrop.cropObj, spawnPoint.position, transform.rotation);
        if(s1.GetComponentInChildren<flowerbedScript>() != null)
        {
            Instantiate(currCrop.cropObj, spawnPoint.position, transform.rotation);
        }
        AkSoundEngine.PostEvent("plantGrow", gameObject);
        ResetFarmPlot();
    }

    private void ResetFarmPlot()
    {
        currCrop = null;
        hasCrop = false;
        meshIndex = -1;
        harvestButton.SetActive(false);
        s1.GetComponent<MeshFilter>().sharedMesh = null;

        if (plantedGeo != null)
        {
            plantedGeo.SetActive(false);
        }

        foreach (flowerbedScript scr in s1.GetComponentsInChildren<flowerbedScript>())
        {
            scr.updateMesh();
        }
    }
}
