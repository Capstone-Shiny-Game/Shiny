using System.Collections.Generic;
using UnityEngine;

public class QSGraphSaveDataSO : ScriptableObject
{
    [field: SerializeField] public string FileName { get; set; }
    [field: SerializeField] public List<QSGroupSaveData> Groups { get; set; }
    [field: SerializeField] public List<QSNodeSO> Nodes { get; set; }
    [field: SerializeField] public List<string> OldGroupNames { get; set; }
    [field: SerializeField] public List<string> OldUngroupedNodeNames { get; set; }
    [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

    public void Initialize(string fileName)
    {
        FileName = fileName;

        Groups = new List<QSGroupSaveData>();
        Nodes = new List<QSNodeSO>();
    }
}
