using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetMaterial : MonoBehaviour, Savable
{
    [Serializable]
    public struct Item
    {
        public PatioUtility.Furniture name;
        public GameObject obj;
        public MaterialPair[] mats;
    }
    [Serializable]
    public struct MaterialPair
    {
        public string mName;
        public Material material;
    }
    [Serializable]
    public struct CurrentSelection
    {
        public PatioUtility.Furniture objName;
        public string matName;
    }


    public CurrentSelection[] currentMats;
    public Item[] items;
    public GameObject PatioMenu;
    /// <summary>
    /// KEY: Enum of object name, VALUE: [obj of object, materials for obj]
    /// </summary>
    private Dictionary<PatioUtility.Furniture, Tuple<GameObject, MaterialPair[]>> patioObjs;
    private GameObject currObj;
    private Material currMat;


    private Tuple<GameObject, MaterialPair[]> current;

    TMP_Text title;
    Text[] tOptions;
    Button[] bOptions;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
       

        patioObjs = new Dictionary<PatioUtility.Furniture, Tuple<GameObject, MaterialPair[]>>();
        foreach (Item i in items)
        {
            patioObjs.Add(i.name, new Tuple<GameObject, MaterialPair[]>(i.obj, i.mats));

        }
        LoadCurrentMaterials();
        setButtonsActive(false);

    }
    /// <summary>
    /// Given the title of the gameobject and the materials, sets up the UI for the user
    /// </summary>
    /// <param name="titleText"></param>
    /// <param name="mats"></param>
    private void SetupGUI(string titleText, MaterialPair[] mats)
    {


        title.text = titleText;

        int i = 0;
       foreach (MaterialPair p in mats)
        {
            tOptions[i].text = p.mName;
            bOptions[i++].onClick.AddListener(delegate { SetMaterials(p.mName); });
        }
    }
    /// <summary>
    /// Reads the currentMaterials array and sets the textures 
    /// </summary>
    private void LoadCurrentMaterials()
    {
        foreach (CurrentSelection cs in currentMats)
        {
            SetMaterials(cs.matName, patioObjs[cs.objName]);
        }
    }
    /// <summary>
    /// Given an object name, return the name of its current material
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    private string GetCurrentMaterialName(PatioUtility.Furniture objName)
    {
        foreach (CurrentSelection cs in currentMats)
        {
            if (cs.objName == objName)
            {
                return cs.matName;
            }

        }
        return "";
    }

    private void setButtonsActive(bool isActive)
    {
        if(isActive && title == null)
        {
            title = PatioMenu.GetComponentInChildren<TMP_Text>();
            tOptions = PatioMenu.GetComponentsInChildren<Text>(true);
            bOptions = PatioMenu.GetComponentsInChildren<Button>(true);
        }
        PatioMenu.SetActive(isActive);

    }
    public void ExitMenu()
    {
        setButtonsActive(false);
    }
    /// <summary>
    /// Given a piece of furniture, show the UI to the user
    /// </summary>
    /// <param name="name"></param>
    public void ShowMaterials(PatioUtility.Furniture name)
    {
        setButtonsActive(true);

        current = patioObjs[name];
        SetupGUI(PatioUtility.GetPrettyName(name), current.Item2);
    }


    /// <summary>
    /// Given the material name, sets the texture for the current gameobject
    /// </summary>
    /// <param name="matName"></param>
    public void SetMaterials(String matName)
    {
        SetMaterials(matName, current);
    }
    private void SetMaterials(String matName, Tuple<GameObject, MaterialPair[]> selection)
    {
        foreach (MaterialPair p in selection.Item2)
        {
            if (p.mName.ToLower().Equals(matName.ToLower()))
            {
                selection.Item1.GetComponent<MeshRenderer>().material = p.material;
                return;
            }
        }
    }

    public void AddSelfToSavablesList()
    {
        Save.AddSelfToSavablesList(this);
    }

    public void GetSaveData(ref Save.SaveData saveData)
    {
        //KEY: object, VALUE: current material
        if (saveData.materials == null)
        {
            saveData.materials = new Dictionary<PatioUtility.Furniture, string>();

        }
        foreach (PatioUtility.Furniture objName in patioObjs.Keys) //save the current material for each object in the patio
        {
            if (saveData.materials.ContainsKey(objName))
            {
                saveData.materials[objName] = GetCurrentMaterialName(objName);
            }
            else
            {
                saveData.materials.Add(objName, GetCurrentMaterialName(objName));

            }

        }
        return;
    }

    public void LoadData(ref Save.SaveData saveData)
    {
        CurrentSelection[] temp = new CurrentSelection[saveData.materials.Count];
        int i = 0;
        foreach (KeyValuePair<PatioUtility.Furniture, string> curr in saveData.materials) //save the current material for each object in the patio
        {
            CurrentSelection c = new CurrentSelection();
            c.objName = curr.Key;
            c.matName = curr.Value;

            temp[i] = c;

        }
        currentMats = temp;
        LoadCurrentMaterials();
    }
}
