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
    public DSGraphView(DialogSystemEditWindow dSEditorWindow)
    {
        editorWindow = dSEditorWindow;

        AddSearchWindow();
        AddManipulators();
        AddGridBackground();
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

    public DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
    {
        Type nodeType = Type.GetType($"DS{dialogueType}Node");
        DSNode node = (DSNode)Activator.CreateInstance(nodeType);

        node.Initialize(position);
        node.Draw();

        return node;
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
            menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
        );

        return contextualMenuManipulator;
    }

    public Group CreateGroup(string title, Vector2 localMousePosition)
    {
        Group group = new Group()
        {
            title = title
        };

        group.SetPosition(new Rect(localMousePosition, Vector2.zero));

        return group;
    }

    private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
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

}
