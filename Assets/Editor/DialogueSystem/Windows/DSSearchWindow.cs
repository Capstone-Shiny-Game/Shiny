using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DSGraphView graphView;
    public void Initialize(DSGraphView dSGraphView)
    {
        graphView = dSGraphView;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Create Element")),
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
            new SearchTreeEntry(new GUIContent("Single Choice"))
            {
                level = 2,
                userData = DSDialogueType.SingleChoice
            },
            new SearchTreeEntry(new GUIContent("Multiple Choice"))
            {
                level = 2,
                userData = DSDialogueType.MultipleChoice
            },
            new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
            new SearchTreeEntry(new GUIContent("Single Group"))
            {
                level = 2,
                userData = new Group()
            }
        };

        return searchTreeEntries;
    }


    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);

        switch (SearchTreeEntry.userData)
        {
            case Group _:
                graphView.CreateGroup("DialogueGroup", localMousePosition);
                return true;

            case DSDialogueType.SingleChoice:
                DSSingleChoiceNode singleChoiceNode = (DSSingleChoiceNode)graphView.CreateNode("DialogueName", DSDialogueType.SingleChoice, localMousePosition);
                graphView.AddElement(singleChoiceNode);
                return true;

            case DSDialogueType.MultipleChoice:
                DSMultipleChoiceNode multipleChoiceNode = (DSMultipleChoiceNode)graphView.CreateNode("DialogueName", DSDialogueType.MultipleChoice, localMousePosition);
                graphView.AddElement(multipleChoiceNode);
                return true;

            default:
                return false;
        }
    }
}
