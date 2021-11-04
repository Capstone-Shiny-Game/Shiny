using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DSMultipleChoiceNode : DSNode
{
    public override void Draw()
    {
        base.Draw();

        Button addChoiceButton = DSElementUtilty.CreateButton("Add Choice", () =>
        {
            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "New Choice"
            };

            Choices.Add(choiceData);

            Port choicePort = CreateChoicePort(choiceData);

            outputContainer.Add(choicePort);
        });

        addChoiceButton.AddToClassList("ds-node__button");

        mainContainer.Insert(1, addChoiceButton);

        foreach (DSChoiceSaveData choice in Choices)
        {
            Port choicePort = CreateChoicePort(choice);

            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }

    public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
    {
        base.Initialize(nodeName, dsGraphView, position);

        DialogueType = DSDialogueType.MultipleChoice;

        DSChoiceSaveData choiceData = new DSChoiceSaveData()
        {
            Text = "New Choice"
        };

        Choices.Add(choiceData);
    }

    private Port CreateChoicePort(object userData)
    {
        Port choicePort = this.CreatePort();

        choicePort.userData = userData;

        DSChoiceSaveData choiceData = (DSChoiceSaveData)userData;

        Button deleteChoiceButton = DSElementUtilty.CreateButton("X", () =>
        {
            if (Choices.Count == 1)
            {
                return;
            }
            if (choicePort.connected)
            {
                graphView.DeleteElements(choicePort.connections);
            }

            Choices.Remove(choiceData);
            graphView.RemoveElement(choicePort);
        });

        deleteChoiceButton.AddToClassList("ds-node__button");

        TextField choiceTextField = DSElementUtilty.CreateTextField(choiceData.Text, null, callback =>
        {
            choiceData.Text = callback.newValue;
        });

        choiceTextField.AddClasses("ds-node__textfield", "ds-node__choice-textfield", "ds-node__textfield__hidden");

        choicePort.Add(choiceTextField);
        choicePort.Add(deleteChoiceButton);

        return choicePort;
    }
}