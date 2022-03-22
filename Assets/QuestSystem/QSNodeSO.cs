using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QSData
{
    [field: SerializeField] public string Name;
    [field: SerializeField] public Type Type;
    [field: SerializeField] public string PrevNode;
    [field: SerializeField] public string PrevPort;

    public QSData(string Name, Type Type)
    {
        this.Name = Name;
        this.Type = Type;
        PrevNode = "";
        PrevPort = "";
        // OptionValue
    }
}

[Serializable]
public abstract class QSNodeSO : ScriptableObject
{
    [field: SerializeField] public string Name;
    [field: SerializeField] public string ID;
    [field: SerializeField] public List<QSData> Inputs;
    [field: SerializeField] public List<QSData> Outputs;
    [field: SerializeField] public List<QSData> Options;
    [field: SerializeField] public Vector2 Position;

    public int Order { get { return (int)Position.x;  } }

    public virtual void InitializeEditor()
    {
        ID = Guid.NewGuid().ToString();
    }

    public virtual void InitializeGame()
    {
    }

    protected static readonly List<QSData> Empty = new List<QSData>();
}
