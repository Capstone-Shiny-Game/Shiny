using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class QSNode : Node
{
    public QSGroup Group { get; set; }
    private QSGraphView graphView;
    private QSNodeSO SO;
    private List<Port> Inputs;
    private List<Port> Outputs;
    private List<VisualElement> Options;

    public string Name { get => SO.Name; }

    public void Initialize(QSNodeSO so, QSGraphView qSGraphView, Vector2 position)
    {
        SO = so;
        graphView = qSGraphView;
        SetPosition(new Rect(position, Vector2.zero));

        Inputs = new List<Port>();
        Outputs = new List<Port>();
        Options = new List<VisualElement>();
        name = SO.Name;
        foreach (QSData data in SO.Inputs)
            Inputs.Add(this.CreatePort(data.Name, data.Type, Direction.Input, Port.Capacity.Single));
        foreach (QSData data in SO.Outputs)
            Outputs.Add(this.CreatePort(data.Name, data.Type, Direction.Output, Port.Capacity.Multi));
        foreach (QSData data in SO.Options)
        {
            if (data.Type == typeof(bool))
                Options.Add(DSElementUtilty.CreateCheckBox(label: data.Name, onValueChanged: changedEvent => { }));
            else if (data.Type == typeof(string))
                Options.Add(DSElementUtilty.CreateTextField(label: data.Name, onValueChanged: changedEvent => { }));
        }

        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");
    }

    public void Draw()
    {
        TextField questNameTextField = DSElementUtilty.CreateTextField(SO.Name, null, changedEvent =>
        {
            TextField target = (TextField)changedEvent.target;
            target.value = changedEvent.newValue; //.RemoveWhitespaces().RemoveSpecialCharacters();
            // TODO : determine if allowing whitespace, special characters will break anything

            if (string.IsNullOrEmpty(target.value))
            {
                if (!string.IsNullOrEmpty(SO.Name))
                {
                    ++graphView.RepeatedNameCount;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(SO.Name))
                {
                    --graphView.RepeatedNameCount;
                }
            }

            if (Group == null)
            {
                graphView.RemoveUngroupedNode(this);
                SO.Name = target.value;
                name = SO.Name;
                graphView.AddUngroupedNode(this);
                return;
            }

            QSGroup tempGroup = Group;
            graphView.RemoveGroupedNode(Group, this);
            SO.Name = target.value;
            name = SO.Name;
            graphView.AddGroupedNode(tempGroup, this);
        });

        questNameTextField.AddClasses("ds-node__text-field", "ds-node__filename-textfield", "ds-node__textfield__hidden");
        titleContainer.Insert(0, questNameTextField);
        foreach (Port port in Inputs)
            inputContainer.Add(port);
        foreach (Port port in Outputs)
            outputContainer.Add(port);
        foreach (VisualElement element in Options)
            mainContainer.Add(element);
        RefreshExpandedState();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Disconnect Inputs", actionEvent => DisconnectPorts(inputContainer));
        evt.menu.AppendAction("Disconnect Outputs", actionEvent => DisconnectPorts(outputContainer));
        base.BuildContextualMenu(evt);
    }

    public void DisconnectAllPorts()
    {
        DisconnectPorts(inputContainer);
        DisconnectPorts(outputContainer);
    }

    private void DisconnectPorts(VisualElement container)
    {
        foreach (Port port in container.Children())
        {
            if (!port.connected)
                continue;
            graphView.DeleteElements(port.connections);
        }
    }

    public void SetErrorStyle(Color color)
    {
        mainContainer.style.backgroundColor = color;
    }

    public void ResetStyle()
    {
        mainContainer.style.backgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
    }
}
