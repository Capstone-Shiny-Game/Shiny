using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DSNode : Node
{
    public string ID { get; set; }
    public string DialogueName { get; set; }
    public List<DSChoiceSaveData> Choices { get; set; }
    public string Text { get; set; }
    public DSDialogueType DialogueType { get; set; }
    protected DSGraphView graphView;
    public DSGroup Group { get; set; }

    public virtual void Initialize(DSGraphView dsGraphView, Vector2 position)
    {
        ID = Guid.NewGuid().ToString();
        DialogueName = "DialogueName";
        Choices = new List<DSChoiceSaveData>();
        Text = "Dialogue text.";

        graphView = dsGraphView;
        SetPosition(new Rect(position, Vector2.zero));

        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");
    }

    public virtual void Draw()
    {
        //TITLE\\
        TextField dialogueNameTextField = DSElementUtilty.CreateTextField(DialogueName, null, callback =>
        {
            TextField target = (TextField)callback.target;
            target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

            if (Group == null)
            {
                graphView.RemoveUngroupedNode(this);

                DialogueName = target.value;

                graphView.AddUngroupedNode(this);

                return;
            }

            DSGroup tempGroup = Group;

            graphView.RemoveGroupedNode(Group, this);

            DialogueName = target.value;

            graphView.AddGroupedNode(tempGroup, this);

        });

        dialogueNameTextField.AddClasses("ds-node__text-field", "ds-node__filename-textfield", "ds-node__textfield__hidden");


        titleContainer.Insert(0, dialogueNameTextField);

        //INPUTS\\
        Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

        inputContainer.Add(inputPort);


        //EXTENTSION CONTAINER\\
        VisualElement customDataContainer = new VisualElement();

        customDataContainer.AddToClassList("ds-node__custom-data-container");

        Foldout textFoldout = DSElementUtilty.CreateFoldout("Dialogue Text");

        TextField textTextField = DSElementUtilty.CreateTextArea(Text);

        textTextField.AddClasses("ds-node__text-field", "ds-node__quote-textfield");

        textFoldout.Add(textTextField);

        customDataContainer.Add(textFoldout);

        extensionContainer.Add(customDataContainer);
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
            {
                continue;
            }

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
