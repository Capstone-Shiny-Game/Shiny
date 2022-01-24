using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct QSData
{
    [field: SerializeField] public string Name;
    [field: SerializeField] public Type Type;

    public QSData(string Name, Type Type)
    {
        this.Name = Name;
        this.Type = Type;
    }
}

public abstract class QSNodeSO : ScriptableObject
{
    [field: SerializeField] public string Name;
    [field: SerializeField] public string ID;
    [field: SerializeField] public List<QSData> Inputs;
    [field: SerializeField] public List<QSData> Outputs;
    [field: SerializeField] public List<QSData> Options;

    public virtual void InitializeEditor()
    {
        ID = Guid.NewGuid().ToString();
    }

    protected static readonly List<QSData> Empty = new List<QSData>();
}
