using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()]
public class SeedCropMap : ScriptableObject
{
    [System.Serializable]
    public class SeedCropEntry
    {
        public string seedName;
        public List<string> meshNames;
        public GameObject crop;
    }

    //public SeedCropEntry[] seedCrops;
}
