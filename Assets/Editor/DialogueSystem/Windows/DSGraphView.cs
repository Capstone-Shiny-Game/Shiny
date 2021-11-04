using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class DSGraphView : GraphView
{
    DialogSystemEditWindow editorWindow;
    private DSSearchWindow searchWindow;
    private SerializableDictionary<string, DSNodeErrorData> ungroupedNodes;
    private SerializableDictionary<string, DSGroupErrorData> groups;
    private SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>> groupedNodes;

    private int repeatedNameCount;

    public int RepeatedNameCount
    {
        get
        {
            return repeatedNameCount;
        }
        set
        {
            repeatedNameCount = value;

            if (repeatedNameCount == 0)
            {
                editorWindow.EnableSave();
            }
            else if (repeatedNameCount >= 1)
            {
                editorWindow.DisableSave();
            }
        }
    }

    public DSGraphView(DialogSystemEditWindow dSEditorWindow)
    {
        editorWindow = dSEditorWindow;
        ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();
        groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>>();
        groups = new SerializableDictionary<string, DSGroupErrorData>();

        AddSearchWindow();
        AddManipulators();
        AddGridBackground();

        OnElementsDeleted();
        OnGroupElementsAdded();
        OnGroupElementsRemoved();
        OnGroupRenamed();
        OnGraphViewChanged();

        AddStyles();
    }

    public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
    {
        Vector2 worldMousePosition = mousePosition;

        if (isSearchWindow)
        {
            worldMousePosition -= editorWindow.position.position;
        }
        Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

        return localMousePosition;
    }

    private void AddSearchWindow()
    {
        if (searchWindow == null)
        {
            searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
            searchWindow.Initialize(this);
        }
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (port.direction != startPort.direction && port.node != startPort.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    public DSNode CreateNode(string nodeName, DSDialogueType dialogueType, Vector2 position, bool shouldDraw = true)
    {
        Type nodeType = Type.GetType($"DS{dialogueType}Node");
        DSNode node = (DSNode)Activator.CreateInstance(nodeType);

        node.Initialize(nodeName, this, position);
        if (shouldDraw)
        {
            node.Draw();
        }

        AddUngroupedNode(node);

        return node;
    }

    public void AddUngroupedNode(DSNode node)
    {
        string nodeName = node.DialogueName.ToLower();

        if (!ungroupedNodes.ContainsKey(nodeName))
        {
            DSNodeErrorData nodeErrorData = new DSNodeErrorData();
            nodeErrorData.Nodes.Add(node);
            ungroupedNodes.Add(nodeName, nodeErrorData);

            return;
        }

        ungroupedNodes[nodeName].Nodes.Add(node);

        Color errorColor = ungroupedNodes[nodeName].errorData.Color;

        node.SetErrorStyle(errorColor);

        if (ungroupedNodes[nodeName].Nodes.Count == 2)
        {
            ++RepeatedNameCount;
            ungroupedNodes[nodeName].Nodes[0].SetErrorStyle(errorColor);
        }
    }

    private void OnElementsDeleted()
    {
        deleteSelection = (operationName, askUser) =>
        {
            Type groupType = typeof(DSGroup);
            Type edgeType = typeof(Edge);

            List<DSNode> nodesToDelete = new List<DSNode>();
            List<Edge> edgesToDelete = new List<Edge>();
            List<DSGroup> groupsToDelete = new List<DSGroup>();
            foreach (GraphElement element in selection)
            {
                if (element is DSNode node)
                {
                    nodesToDelete.Add(node);
                    continue;
                }

                if (element.GetType() == edgeType)
                {
                    edgesToDelete.Add((Edge)element);
                    continue;
                }

                if (element.GetType() != groupType)
                {
                    continue;
                }

                DSGroup group = (DSGroup)element;

                groupsToDelete.Add(group);
            }

            foreach (DSGroup group in groupsToDelete)
            {
                List<DSNode> groupNodes = new List<DSNode>();

                foreach (GraphElement element in group.containedElements)
                {
                    if (!(element is DSNode node))
                    {
                        continue;
                    }
                    groupNodes.Add((DSNode)element);
                }

                group.RemoveElements(groupNodes);

                RemoveGroup(group);

                RemoveElement(group);
            }

            DeleteElements(edgesToDelete);

            foreach (DSNode node in nodesToDelete)
            {
                if (node.Group != null)
                {
                    node.Group.RemoveElement(node);
                }

                RemoveUngroupedNode(node);

                node.DisconnectAllPorts();

                RemoveElement(node);
            }
        };
    }

    private void OnGroupElementsAdded()
    {
        elementsAddedToGroup = (group, elements) =>
        {
            foreach (GraphElement element in elements)
            {
                if (!(element is DSNode))
                {
                    continue;
                }
                DSGroup nodeGroup = (DSGroup)group;
                DSNode node = (DSNode)element;

                RemoveUngroupedNode(node);

                AddGroupedNode(nodeGroup, node);
            }
        };
    }

    private void OnGroupRenamed()
    {
        groupTitleChanged = (group, newTitle) =>
        {
            DSGroup dSGroup = (DSGroup)group;

            dSGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

            if (string.IsNullOrEmpty(dSGroup.title))
            {
                if (!string.IsNullOrEmpty(dSGroup.oldTitle))
                {
                    ++RepeatedNameCount;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(dSGroup.oldTitle))
                {
                    --RepeatedNameCount;
                }
            }

            RemoveGroup(dSGroup);

            dSGroup.oldTitle = dSGroup.title.ToLower();

            AddGroup(dSGroup);
        };
    }

    private void OnGraphViewChanged()
    {
        graphViewChanged = (changes) =>
        {
            if (changes.edgesToCreate != null)
            {
                foreach (Edge edge in changes.edgesToCreate)
                {
                    DSNode nextNode = (DSNode)edge.input.node;
                    DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;
                    choiceData.NodeID = nextNode.ID;
                }
            }
            if (changes.elementsToRemove != null)
            {
                Type edgeType = typeof(Edge);

                foreach (GraphElement element in changes.elementsToRemove)
                {
                    if (element.GetType() != edgeType)
                    {
                        continue;
                    }

                    Edge edge = (Edge)element;

                    DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;

                    choiceData.NodeID = "";
                }
            }

            return changes;
        };
    }

    private void OnGroupElementsRemoved()
    {
        elementsRemovedFromGroup = (group, elements) =>
        {
            foreach (GraphElement element in elements)
            {
                if (!(element is DSNode))
                {
                    continue;
                }

                DSNode node = (DSNode)element;

                RemoveGroupedNode(group, node);

                AddUngroupedNode(node);
            }
        };
    }

    public void RemoveGroupedNode(Group group, DSNode node)
    {
        string nodeName = node.DialogueName.ToLower();

        if (!groupedNodes.ContainsKey(group))
        {
            return;
        }

        groupedNodes[group][nodeName].Nodes.Remove(node);

        node.ResetStyle();

        if (groupedNodes[group][nodeName].Nodes.Count == 1)
        {
            --RepeatedNameCount;
            groupedNodes[group][nodeName].Nodes[0].ResetStyle();

            return;
        }

        if (groupedNodes[group][nodeName].Nodes.Count == 0)
        {
            groupedNodes[group].Remove(nodeName);

            if (groupedNodes[group].Count == 0)
            {
                groupedNodes.Remove(group);
            }
        }
    }

    public void AddGroupedNode(DSGroup group, DSNode node)
    {
        string nodeName = node.DialogueName.ToLower();

        node.Group = group;

        if (!groupedNodes.ContainsKey(group))
        {
            groupedNodes.Add(group, new SerializableDictionary<string, DSNodeErrorData>());
        }

        if (!groupedNodes[group].ContainsKey(nodeName))
        {
            DSNodeErrorData nodeErrorData = new DSNodeErrorData();
            nodeErrorData.Nodes.Add(node);
            groupedNodes[group].Add(nodeName, nodeErrorData);

            return;
        }

        groupedNodes[group][nodeName].Nodes.Add(node);

        Color errorColor = groupedNodes[group][nodeName].errorData.Color;

        node.SetErrorStyle(errorColor);

        if (groupedNodes[group][nodeName].Nodes.Count == 2)
        {
            ++RepeatedNameCount;
            groupedNodes[group][nodeName].Nodes[0].SetErrorStyle(errorColor);
        }
    }

    public void RemoveUngroupedNode(DSNode node)
    {
        string nodeName = node.DialogueName.ToLower();

        node.Group = null;

        ungroupedNodes[nodeName].Nodes.Remove(node);

        node.ResetStyle();

        if (ungroupedNodes[nodeName].Nodes.Count == 1)
        {
            --RepeatedNameCount;
            ungroupedNodes[nodeName].Nodes[0].ResetStyle();

            return;
        }

        if (ungroupedNodes[nodeName].Nodes.Count == 0)
        {
            ungroupedNodes.Remove(nodeName);
        }
    }

    public void AddGroup(DSGroup group)
    {
        string groupName = group.title.ToLower();
        if (!groups.ContainsKey(groupName))
        {
            DSGroupErrorData groupErrorData = new DSGroupErrorData();

            groupErrorData.Groups.Add(group);

            groups.Add(groupName, groupErrorData);

            return;
        }

        groups[groupName].Groups.Add(group);

        Color errorColor = groups[groupName].errorData.Color;
        group.SetErrorStyle(errorColor);

        if (groups[groupName].Groups.Count == 2)
        {
            ++RepeatedNameCount;
            groups[groupName].Groups[0].SetErrorStyle(errorColor);
        }
    }

    public void RemoveGroup(DSGroup group)
    {
        string groupName = group.oldTitle.ToLower();

        if (!groups.ContainsKey(groupName))
        {
            return;
        }

        groups[groupName].Groups.Remove(group);

        group.ResetStyle();

        if (groups[groupName].Groups.Count == 1)
        {
            --RepeatedNameCount;
            groups[groupName].Groups[0].ResetStyle();

            return;
        }

        if (groups[groupName].Groups.Count == 0)
        {
            groups.Remove(groupName);
        }
    }

    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DSDialogueType.MultipleChoice));

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(CreateGroupContextualMenu());
    }

    private IManipulator CreateGroupContextualMenu()
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
        );

        return contextualMenuManipulator;
    }

    public DSGroup CreateGroup(string title, Vector2 localMousePosition)
    {
        DSGroup group = new DSGroup(title, localMousePosition);

        AddGroup(group);

        AddElement(group);

        foreach (GraphElement element in selection)
        {
            if (!(element is DSNode))
            {
                continue;
            }

            DSNode node = (DSNode)element;

            group.AddElement(node);
        }

        return group;
    }

    private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("DialogueName", dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
        );

        return contextualMenuManipulator;
    }

    private void AddGridBackground()
    {
        GridBackground gridBackground = new GridBackground();
        //gridBackground.StretchToParentSize();

        Insert(0, gridBackground);
    }

    private void AddStyles()
    {
        this.AddStyleSheets(
            "Dialogue System/DSGraphViewStyles.uss",
            "Dialogue System/DSNodeStyles.uss"
        );
    }

    public void ClearGraph()
    {
        graphElements.ForEach(element => RemoveElement(element));

        groups.Clear();
        groupedNodes.Clear();
        ungroupedNodes.Clear();

        RepeatedNameCount = 0;
    }

}
