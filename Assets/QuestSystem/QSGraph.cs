using System;
using UnityEngine;

[Serializable]
public class QSGraph : ScriptableObject
{
    // TODO : consolidate with QSGraphSaveDataSO
    [SerializeField]
    public SerializableDictionary<string, QSNodeSO> Nodes = new SerializableDictionary<string, QSNodeSO>();
}
