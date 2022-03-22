using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QSManager : Savable
{
    static private QSManager Instance;

    public static QSManager GetManager()
    {
        if (Instance == null)
            Instance = new QSManager();
        return Instance;
    }

    public QSGraph Graph { get; }

    private SortedSet<QSNodeSO> Inactive;

    private SortedSet<QSNodeSO> Active;

    private SortedSet<QSNodeSO> Completed;

    private HashSet<QSNodeSO> Support;

    public string NameOfEarliestActiveQuest
    {
        get
        {
            return Active.Count > 0 ? Active.Min().Name : "Have Fun!";
        }
    }

    public IEnumerable<string> NamesOfActiveQuests
    {
        get
        {
            return Active.Select(quest => quest.Name);
        }
    }

    public IEnumerable<string> NamesOfCompletedQuests
    {
        get
        {
            return Completed.Select(quest => quest.Name);
        }
    }

    private QSManager()
    {
        // Jesse ordered me to leave it named TheScalesOfJustice.
        Comparer<QSNodeSO> TheScalesOfJustice = Comparer<QSNodeSO>.Create((lhs, rhs) => rhs.Order - lhs.Order);

        Inactive = new SortedSet<QSNodeSO>(TheScalesOfJustice);
        Active = new SortedSet<QSNodeSO>(TheScalesOfJustice);
        Completed = new SortedSet<QSNodeSO>(TheScalesOfJustice);
        Support = new HashSet<QSNodeSO>();
        Graph = new QSGraph(); // actually deserialize

        foreach (QSNodeSO node in Graph.Nodes.Values)
        {
            // TODO : actually sort
            Inactive.Add(node);
        }

    }

    public void CheckUnlock() {
        throw new System.NotImplementedException();
    }

    public void CheckCompleted() {
        throw new System.NotImplementedException();
    }

    public void AddSelfToSavablesList()
    {
        throw new System.NotImplementedException();
    }

    public void GetSaveData(ref Save.SaveData saveData)
    {
        throw new System.NotImplementedException();
    }

    public void LoadData(ref Save.SaveData saveData)
    {
        throw new System.NotImplementedException();
    }
}
