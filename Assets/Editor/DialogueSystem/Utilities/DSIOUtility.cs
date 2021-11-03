using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class DSIOUtility
{
    private static DSGraphView graphView;
    private static string graphFileName;
    private static string containerFolderPath;
    private static List<DSGroup> groups;
    private static List<DSNode> nodes;
    private static Dictionary<string, DSDialogueGroupSO> createdDialogueGroups;
    private static Dictionary<string, DSDialogueSO> createdDialogues;

    public static void Initialize(string graphName, DSGraphView _graphView)
    {
        graphFileName = graphName;
        containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}";
        graphView = _graphView;
        createdDialogueGroups = new Dictionary<string, DSDialogueGroupSO>();
        createdDialogues = new Dictionary<string, DSDialogueSO>();

        groups = new List<DSGroup>();
        nodes = new List<DSNode>();
    }

    #region Save Methdods
    public static void Save()
    {
        CreateStaticFolders();

        GetElementsFromGraphView();

        DSGraphSaveDataSO graphData = CreateAsset<DSGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}Graph");

        graphData.Initialize(graphFileName);

        DSDialogueContainerSO dialogueContainer = CreateAsset<DSDialogueContainerSO>(containerFolderPath, graphFileName);

        dialogueContainer.Initialize(graphFileName);

        SaveGroups(graphData, dialogueContainer);

        SaveNodes(graphData, dialogueContainer);

        SaveAsset(graphData);
        SaveAsset(dialogueContainer);
    }

    #region Nodes

    private static void SaveNodes(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
    {
        SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
        List<string> ungroupedNodeNames = new List<string>();
        foreach (DSNode node in nodes)
        {
            SaveNodeToGraph(node, graphData);
            SaveNodeToScriptableObject(node, dialogueContainer);

            if (node.Group != null)
            {
                groupedNodeNames.AddItem(node.Group.title, node.DialogueName);

                continue;
            }

            ungroupedNodeNames.Add(node.DialogueName);
        }

        UpdateDialogueChoicesConnections();

        UpdateOldGroupedNodes(groupedNodeNames, graphData);
        UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
    }

    private static void SaveNodeToGraph(DSNode node, DSGraphSaveDataSO graphData)
    {
        List<DSChoiceSaveData> choices = new List<DSChoiceSaveData>();

        foreach (DSChoiceSaveData choice in node.Choices)
        {
            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = choice.Text,
                NodeID = choice.NodeID
            };
            choices.Add(choiceData);
        }

        DSNodeSaveData nodeData = new DSNodeSaveData()
        {
            ID = node.ID,
            Name = node.DialogueName,
            Choices = choices,
            Text = node.Text,
            GroupID = node.Group?.ID,
            DialogueType = node.DialogueType,
            Position = node.GetPosition().position
        };
        graphData.Nodes.Add(nodeData);
    }

    private static void SaveNodeToScriptableObject(DSNode node, DSDialogueContainerSO dialogueContainer)
    {
        DSDialogueSO dialogue;

        if (node.Group != null)
        {
            dialogue = CreateAsset<DSDialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

            dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], dialogue);
        }
        else
        {
            dialogue = CreateAsset<DSDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);

            dialogueContainer.UngroupedDialogues.Add(dialogue);
        }

        dialogue.Initialize(node.DialogueName, node.Text, ConvertNodeChoicesToDialogueChoices(node.Choices), node.DialogueType, node.IsStartingNode());

        createdDialogues.Add(node.ID, dialogue);

        SaveAsset(dialogue);
    }

    private static List<DSDialogueChoiceData> ConvertNodeChoicesToDialogueChoices(List<DSChoiceSaveData> nodeChoices)
    {
        List<DSDialogueChoiceData> dialogueChoices = new List<DSDialogueChoiceData>();

        foreach (DSChoiceSaveData choice in nodeChoices)
        {
            DSDialogueChoiceData dialogueChoice = new DSDialogueChoiceData()
            {
                Text = choice.Text
            };
            dialogueChoices.Add(dialogueChoice);
        }

        return dialogueChoices;
    }

    private static void UpdateDialogueChoicesConnections()
    {
        foreach (DSNode node in nodes)
        {
            DSDialogueSO dialogue = createdDialogues[node.ID];
            for (int choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
            {
                DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];

                if (string.IsNullOrEmpty(nodeChoice.NodeID))
                {
                    continue;
                }

                dialogue.Choices[choiceIndex].NextDialogue = createdDialogues[nodeChoice.NodeID];

                SaveAsset(dialogue);
            }
        }
    }

    private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, DSGraphSaveDataSO graphData)
    {
        if (graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
        {
            foreach (KeyValuePair<string, List<string>> oldGroupNode in graphData.OldGroupedNodeNames)
            {
                List<string> nodesToRemove = new List<string>();

                if (currentGroupedNodeNames.ContainsKey(oldGroupNode.Key))
                {
                    nodesToRemove = oldGroupNode.Value.Except(currentGroupedNodeNames[oldGroupNode.Key]).ToList();
                }

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Groups/{oldGroupNode.Key}/Dialogues", nodeToRemove);
                }
            }
        }

        graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
    }

    private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, DSGraphSaveDataSO graphData)
    {
        if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
        {
            List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

            foreach (string nodeName in nodesToRemove)
            {
                RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeName);
            }
        }

        graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
    }

    #endregion

    #region Groups

    private static void SaveGroups(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
    {
        List<string> groupNames = new List<string>();
        foreach (DSGroup group in groups)
        {
            SaveGroupToGraph(group, graphData);
            SaveGroupToScriptableObject(group, dialogueContainer);

            groupNames.Add(group.title);
        }

        UpdateOldGroups(groupNames, graphData);
    }

    private static void SaveGroupToGraph(DSGroup group, DSGraphSaveDataSO graphData)
    {
        DSGroupSaveData groupData = new DSGroupSaveData()
        {
            ID = group.ID,
            Name = group.title,
            Position = group.GetPosition().position
        };

        graphData.Groups.Add(groupData);
    }

    private static void SaveGroupToScriptableObject(DSGroup group, DSDialogueContainerSO dialogueContainer)
    {
        string groupName = group.title;
        CreateFolder($"{containerFolderPath}/Groups", groupName);
        CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

        DSDialogueGroupSO dialogueGroup = CreateAsset<DSDialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);

        dialogueGroup.Initialize(groupName);

        createdDialogueGroups.Add(group.ID, dialogueGroup);

        dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DSDialogueSO>());

        SaveAsset(dialogueGroup);
    }

    private static void UpdateOldGroups(List<string> currentGroupsNames, DSGraphSaveDataSO graphData)
    {
        if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
        {
            List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupsNames).ToList();
            foreach (string groupName in groupsToRemove)
            {
                RemoveFolder($"{containerFolderPath}/Groups/{groupName}");
            }
        }

        graphData.OldGroupNames = new List<string>(currentGroupsNames);
    }
    #endregion

    #endregion

    #region Fetchers

    private static void GetElementsFromGraphView()
    {
        Type groupType = typeof(DSGroup);
        graphView.graphElements.ForEach(graphElement =>
        {
            if (graphElement is DSNode node)
            {
                nodes.Add(node);

                return;
            }

            if (graphElement.GetType() == groupType)
            {
                groups.Add((DSGroup)graphElement);

                return;
            }
        });
    }

    #endregion

    #region Creation Methods

    private static void CreateStaticFolders()
    {
        CreateFolder("Assets/Editor/DialogueSystem", "Graphs");

        CreateFolder("Assets", "DialogueSystem");
        CreateFolder("Assets/DialogueSystem", "Dialogues");
        CreateFolder("Assets/DialogueSystem/Dialogues", graphFileName);
        CreateFolder(containerFolderPath, "Global");
        CreateFolder(containerFolderPath, "Groups");
        CreateFolder($"{containerFolderPath}/Global", "Dialogues");
    }



    #endregion

    #region Utilities

    private static void SaveAsset(UnityEngine.Object asset)
    {
        EditorUtility.SetDirty(asset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        string fullPath = $"{path}/{assetName}.asset";

        T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, fullPath);
        }

        return asset;
    }

    private static void RemoveAsset(string path, string assetName)
    {
        string fullPath = $"{path}/{assetName}.asset";

        AssetDatabase.DeleteAsset(fullPath);
    }

    private static void CreateFolder(string path, string foldername)
    {
        if (AssetDatabase.IsValidFolder($"{path}/{foldername}"))
        {
            return;
        }

        AssetDatabase.CreateFolder(path, foldername);
    }

    private static void RemoveFolder(string path)
    {
        FileUtil.DeleteFileOrDirectory($"{path}.meta");
        FileUtil.DeleteFileOrDirectory($"{path}/");
    }

    #endregion
}