using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetMaterial : MonoBehaviour
{
    [Serializable]
    public struct PatioItems
    {
        public string name;
        public GameObject obj;
        public MaterialPair[] mats;
    }
    [Serializable]
    public struct MaterialPair
    {
        public string mName;
        public Material material;

    }
    public PatioItems[] items;
    private GameObject obj;
    private Material currMat;
    public Dictionary<String, Tuple<GameObject, MaterialPair[]>> patioObjs;
    private Dictionary<String, Material> currMats;
    private Tuple<GameObject, MaterialPair[]> current;

    TMP_Text title;
    Text[] texts;
    Button[] options;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
      title = GetComponentInChildren<TMP_Text>();
        title.text = items[0].name;
        texts = GetComponentsInChildren<Text>(true);
        options = GetComponentsInChildren<Button>(true);
        patioObjs = new Dictionary<String, Tuple<GameObject, MaterialPair[]>>();
        foreach (PatioItems i in items)
        {
            patioObjs.Add(i.name.ToLower(), new Tuple<GameObject, MaterialPair[]>(i.obj, i.mats));

        }
    }


    public void ShowMaterials(String name)
    {
        Debug.Log("SHOW: " + name);
        current = patioObjs[name.ToLower()];
        obj = current.Item1;
        //TO DO: add to UI
        int i = 0; 
        foreach(MaterialPair p in current.Item2)
        {
            texts[i].text = p.mName;
            options[i++].onClick.AddListener(delegate { SetMaterials(p.mName); });
        }

    }


    public void SetMaterials(String matName)
    {
        Debug.Log("BUTTON CLICKED");
        foreach (MaterialPair p in current.Item2)
        {
            if (p.mName.ToLower().Equals(matName.ToLower()))
            {
                current.Item1.GetComponent<MeshRenderer>().material = p.material;
                return;
            }
        }
    }
    public void SetObjectMaterial(int id)
    {
        obj.GetComponent<MeshRenderer>().material = currMat;
    }
}
