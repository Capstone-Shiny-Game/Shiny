using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

[Serializable]
public class QSGroupSaveData
{
    [field: SerializeField] public string ID { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Vector2 Position { get; set; }
}

public class QSNodeGroupSO : ScriptableObject
{
    [field: SerializeField] public string GroupName { get; set; }

    public void Initialize(string groupName)
    {
        GroupName = groupName;
    }
}

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

public static class QSIOUtility
{
    private static QSGraphView graphView;
    private static string graphFileName;
    private static string containerFolderPath;
    private static List<QSGroup> groups;
    private static List<QSNode> nodes;
    private static Dictionary<string, QSNodeGroupSO> createdGroups;
    private static Dictionary<string, QSNodeSO> createdNodes;
    private static Dictionary<string, QSGroup> loadedGroups;
    private static Dictionary<string, QSNode> loadedNodes;

    public static void Initialize(string graphName, QSGraphView _graphView)
    {
        graphFileName = graphName;
        containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}";
        graphView = _graphView;
        createdGroups = new Dictionary<string, QSNodeGroupSO>();
        createdNodes = new Dictionary<string, QSNodeSO>();
        loadedGroups = new Dictionary<string, QSGroup>();
        loadedNodes = new Dictionary<string, QSNode>();

        groups = new List<QSGroup>();
        nodes = new List<QSNode>();
    }

    #region LoadMethods

    public static void Load()
    {
        QSGraphSaveDataSO graphData = LoadAsset<QSGraphSaveDataSO>("Assets/Editor/QuestSystem/Graphs", graphFileName);
        if (graphData == null)
        {
            EditorUtility.DisplayDialog("Couldn't Load File",
                "The file at the following path could not be found:\n\n" +
                $"Assets/Editor/QuestSystem/Graphs/{graphFileName}\n\n" +
                "Make sure you choose the right file and it's placed at the folder path mentioned above.",
                "Continue"
            );
            return;
        }

        QuestSystemEditWindow.UpdateFilename(graphData.FileName);

        LoadGroups(graphData.Groups);
        LoadNodes(graphData.Nodes);
        LoadNodesConnections();
    }

    private static void LoadGroups(List<QSGroupSaveData> _groups)
    {
        foreach (QSGroupSaveData group in _groups)
        {
            QSGroup newGroup = graphView.CreateGroup(group.Position);
            newGroup.name = group.Name;
            newGroup.ID = group.ID;
            loadedGroups.Add(group.ID, newGroup);
        }
    }

    private static void LoadNodes(List<QSNodeSO> _nodes)
    {
        foreach (QSNodeSO node in _nodes)
        {
            // TODO : add position to SO
            QSNode newNode = graphView.CreateNode(node.GetType(), Vector2.zero); //node.Position, null, false);
            // newNode.Initialize(node, graphView, Vector2.zero);// node.Position);
            newNode.SO = node;

            newNode.Draw();

            newNode.Draw();

            graphView.AddElement(newNode);

            loadedNodes.Add(node.ID, newNode);

            // TODO : add group to SO
            //if (string.IsNullOrEmpty(node.Group))
            //{
            //    continue;
            //}
            //QSGroup group = loadedGroups[node.Group];
            //newNode.Group = group;
            //group.AddElement(newNode);
        }
    }

    private static void LoadNodesConnections()
    {
        // TODO : actually set user data in graph editor
        foreach (KeyValuePair<string, QSNode> loadedNode in loadedNodes)
        {
            foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
            {
                string saveData = choicePort.userData as string;

                if (saveData is null || string.IsNullOrEmpty(saveData))
                {
                    continue;
                }

                QSNode nextNode = loadedNodes[saveData];

                Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();

                Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                graphView.AddElement(edge);

                loadedNode.Value.RefreshPorts();
            }
        }
    }

    #endregion


    #region Save Methdods
    public static void Save()
    {
        CreateStaticFolders();

        GetElementsFromGraphView();

        QSGraphSaveDataSO graphData = CreateAsset<QSGraphSaveDataSO>("Assets/Editor/QuestSystem/Graphs", $"{graphFileName}Graph");

        graphData.Initialize(graphFileName);

        SaveGroups(graphData);

        SaveNodes(graphData);

        SaveAsset(graphData);
    }

    #region Nodes

    private static void SaveNodes(QSGraphSaveDataSO graphData)
    {
        SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
        List<string> ungroupedNodeNames = new List<string>();
        foreach (QSNode node in nodes)
        {
            SaveNodeToGraph(node, graphData);
            SaveNodeToScriptableObject(node);

            if (node.Group != null)
            {
                groupedNodeNames.AddItem(node.Group.title, node.Name);

                continue;
            }

            ungroupedNodeNames.Add(node.Name);
        }

        UpdateOldGroupedNodes(groupedNodeNames, graphData);
        UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
    }

    private static void SaveNodeToGraph(QSNode node, QSGraphSaveDataSO graphData)
    {
        graphData.Nodes.Add(node.SO);
    }

    private static void SaveNodeToScriptableObject(QSNode node)
    {
        createdNodes.Add(node.SO.ID, node.SO);

        SaveAsset(node.SO);
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

    private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, QSGraphSaveDataSO graphData)
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

    private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, QSGraphSaveDataSO graphData)
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

    private static void SaveGroups(QSGraphSaveDataSO graphData)
    {
        List<string> groupNames = new List<string>();
        foreach (QSGroup group in groups)
        {
            SaveGroupToGraph(group, graphData);
            SaveGroupToScriptableObject(group);

            groupNames.Add(group.title);
        }

        UpdateOldGroups(groupNames, graphData);
    }

    private static void SaveGroupToGraph(QSGroup group, QSGraphSaveDataSO graphData)
    {
        QSGroupSaveData groupData = new QSGroupSaveData()
        {
            ID = group.ID,
            Name = group.title,
            Position = group.GetPosition().position
        };

        graphData.Groups.Add(groupData);
    }

    private static void SaveGroupToScriptableObject(QSGroup group)
    {
        string groupName = group.title;
        CreateFolder($"{containerFolderPath}/Groups", groupName);
        CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Quests");

        QSNodeGroupSO nodeGroup = CreateAsset<QSNodeGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);

        nodeGroup.Initialize(groupName);

        createdGroups.Add(group.ID, nodeGroup);

        SaveAsset(nodeGroup);
    }

    private static void UpdateOldGroups(List<string> currentGroupsNames, QSGraphSaveDataSO graphData)
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
        Type groupType = typeof(QSGroup);
        graphView.graphElements.ForEach(graphElement =>
        {
            if (graphElement is QSNode node)
            {
                nodes.Add(node);

                return;
            }

            if (graphElement.GetType() == groupType)
            {
                groups.Add((QSGroup)graphElement);

                return;
            }
        });
    }

    #endregion

    #region Creation Methods

    private static void CreateStaticFolders()
    {
        CreateFolder("Assets/Editor/QuestSystem", "Graphs");

        CreateFolder("Assets", "QuestSystem");
        CreateFolder("Assets/QuestSystem", "Quests");
        CreateFolder("Assets/QuestsSystem/Quests", graphFileName);
        CreateFolder(containerFolderPath, "Global");
        CreateFolder(containerFolderPath, "Groups");
        CreateFolder($"{containerFolderPath}/Global", "Quests");
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

        T asset = LoadAsset<T>(path, assetName);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, fullPath);
        }

        return asset;
    }

    private static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        return AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");
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