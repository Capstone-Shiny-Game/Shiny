using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class QSSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private QSGraphView graphView;
    public void Initialize(QSGraphView qSGraphView)
    {
        graphView = qSGraphView;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Create Element")),
            new SearchTreeGroupEntry(new GUIContent("Group"), 1),
            new SearchTreeEntry(new GUIContent("Group"))
            {
                level = 2,
                userData = new Group()
            },
            new SearchTreeGroupEntry(new GUIContent("Node"), 1),
        };

        foreach ((string, Type) pair in QS.NodeTypes)
            searchTreeEntries.Add(new SearchTreeEntry(new GUIContent(pair.Item1))
            {
                level = 2,
                userData = pair.Item2
            });
        return searchTreeEntries;
    }


    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);

        switch (SearchTreeEntry.userData)
        {
            case Group _:
                graphView.CreateGroup(localMousePosition);
                return true;

            case Type type:
                QSNode node = graphView.CreateNode(type, localMousePosition, null);
                graphView.AddElement(node);
                return true;

            default:
                return false;
        }
    }
}
