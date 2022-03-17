using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class QSGraphView : GraphView
{
    private QuestSystemEditWindow editorWindow;
    private QSSearchWindow searchWindow;
    private SerializableDictionary<string, QSNodeErrorData> ungroupedNodes;
    private SerializableDictionary<string, QSGroupErrorData> groups;
    private SerializableDictionary<Group, SerializableDictionary<string, QSNodeErrorData>> groupedNodes;

    private int repeatedNameCount;

    // TODO : enforce invariant of single start node
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

    public QSGraphView(QuestSystemEditWindow dSEditorWindow)
    {
        editorWindow = dSEditorWindow;
        ungroupedNodes = new SerializableDictionary<string, QSNodeErrorData>();
        groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, QSNodeErrorData>>();
        groups = new SerializableDictionary<string, QSGroupErrorData>();

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
            searchWindow = ScriptableObject.CreateInstance<QSSearchWindow>();
            searchWindow.Initialize(this);
        }
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (
                port.direction != startPort.direction
                && startPort.portType == port.portType
                && port.node != startPort.node
            )
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    public QSNode CreateNode(QSNodeSO nodeSO, Vector2 position)
    {
        nodeSO.InitializeEditor();
        QSNode node = (QSNode)Activator.CreateInstance(typeof(QSNode));
        node.Initialize(nodeSO, this, position);
        node.Draw();

        AddUngroupedNode(node);

        return node;
    }

    public QSNode CreateNode(Type type, Vector2 position, string enumName = null)
    {
        QSNodeSO nodeSO = (QSNodeSO)ScriptableObject.CreateInstance(type);
        if (nodeSO is QSNPCNodeSO || nodeSO is QSItemNodeSO)
            nodeSO.Name = enumName;
        nodeSO.InitializeEditor();
        QSNode node = (QSNode)Activator.CreateInstance(typeof(QSNode));

        node.Initialize(nodeSO, this, position);
        node.Draw();

        AddUngroupedNode(node);

        return node;
    }

    public void AddUngroupedNode(QSNode node)
    {
        string nodeName = node.Name.ToLower();

        if (!ungroupedNodes.ContainsKey(nodeName))
        {
            QSNodeErrorData nodeErrorData = new QSNodeErrorData();
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
            Type groupType = typeof(QSGroup);
            Type edgeType = typeof(Edge);

            List<QSNode> nodesToDelete = new List<QSNode>();
            List<Edge> edgesToDelete = new List<Edge>();
            List<QSGroup> groupsToDelete = new List<QSGroup>();
            foreach (GraphElement element in selection)
            {
                if (element is QSNode node)
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

                QSGroup group = (QSGroup)element;

                groupsToDelete.Add(group);
            }

            foreach (QSGroup group in groupsToDelete)
            {
                List<QSNode> groupNodes = new List<QSNode>();

                foreach (GraphElement element in group.containedElements)
                {
                    if (element is QSNode node)
                        groupNodes.Add(node);
                }

                group.RemoveElements(groupNodes);

                RemoveGroup(group);

                RemoveElement(group);
            }

            DeleteElements(edgesToDelete);

            foreach (QSNode node in nodesToDelete)
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
                if (!(element is QSNode))
                {
                    continue;
                }
                QSGroup nodeGroup = (QSGroup)group;
                QSNode node = (QSNode)element;

                RemoveUngroupedNode(node);

                AddGroupedNode(nodeGroup, node);
            }
        };
    }

    private void OnGroupRenamed()
    {
        groupTitleChanged = (group, newTitle) =>
        {
            QSGroup qSGroup = (QSGroup)group;

            qSGroup.title = newTitle; //.RemoveWhitespaces().RemoveSpecialCharacters();
            // TODO : determine if allowing whitespace, special characters will break anything

            if (string.IsNullOrEmpty(qSGroup.title))
            {
                if (!string.IsNullOrEmpty(qSGroup.oldTitle))
                {
                    ++RepeatedNameCount;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(qSGroup.oldTitle))
                {
                    --RepeatedNameCount;
                }
            }

            RemoveGroup(qSGroup);

            qSGroup.oldTitle = qSGroup.title.ToLower();

            AddGroup(qSGroup);
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
                    QSNode prevNode = (QSNode)edge.output.node;
                    QSNode nextNode = (QSNode)edge.input .node;
                    string prevPort = edge.output.portName;
                    string nextPort = edge. input.portName;
                    QSData data = prevNode.SO.Outputs.Find(d => d.Name == prevPort);
                    data.ConnectedToNode = nextNode.SO.ID;
                    data.ConnectedToPort = nextPort;
                    // edge.output.userData = nextNode.Name;
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
                    QSNode prevNode = (QSNode)edge.output.node;
                    string prevPort = edge.output.portName;
                    QSData data = prevNode.SO.Outputs.Find(d => d.Name == prevPort);
                    data.ConnectedToNode = "";
                    data.ConnectedToPort = "";
                    // edge.output.userData = "";
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
                if (!(element is QSNode))
                {
                    continue;
                }

                QSNode node = (QSNode)element;

                RemoveGroupedNode(group, node);

                AddUngroupedNode(node);
            }
        };
    }

    public void RemoveGroupedNode(Group group, QSNode node)
    {
        string nodeName = node.Name.ToLower();

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

    public void AddGroupedNode(QSGroup group, QSNode node)
    {
        string nodeName = node.Name.ToLower();

        node.Group = group;

        if (!groupedNodes.ContainsKey(group))
        {
            groupedNodes.Add(group, new SerializableDictionary<string, QSNodeErrorData>());
        }

        if (!groupedNodes[group].ContainsKey(nodeName))
        {
            QSNodeErrorData nodeErrorData = new QSNodeErrorData();
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

    public void RemoveUngroupedNode(QSNode node)
    {
        string nodeName = node.Name.ToLower();

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

    public void AddGroup(QSGroup group)
    {
        string groupName = group.title.ToLower();
        if (!groups.ContainsKey(groupName))
        {
            QSGroupErrorData groupErrorData = new QSGroupErrorData();

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

    public void RemoveGroup(QSGroup group)
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

        foreach ((string, Type) pair in QS.NodeTypes)
            this.AddManipulator(CreateNodeContextualMenu($"Add Node/{pair.Item1}", pair.Item2));
        foreach (QSNPC npc in Enum.GetValues(typeof(QSNPC)))
            this.AddManipulator(CreateNodeContextualMenu($"Add Node/NPC/{npc}", typeof(QSNPCNodeSO), npc.ToString()));
        foreach (ItemSO item in ItemDBUtil.GetItemDBSerializableDict().Values)
            this.AddManipulator(CreateNodeContextualMenu($"Add Node/Item/{item}", typeof(QSItemNodeSO), item.ToString()));

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(CreateGroupContextualMenu());
    }

    private IManipulator CreateGroupContextualMenu()
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => CreateGroup(GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
        );

        return contextualMenuManipulator;
    }

    public QSGroup CreateGroup(Vector2 localMousePosition)
    {
        QSGroup group = new QSGroup(localMousePosition);

        AddGroup(group);

        AddElement(group);

        foreach (GraphElement element in selection)
        {
            if (element is QSNode node)
                group.AddElement(node);
        }

        return group;
    }

    private IManipulator CreateNodeContextualMenu(string actionTitle, Type type, string enumName = null)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(type, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition), enumName)))
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
