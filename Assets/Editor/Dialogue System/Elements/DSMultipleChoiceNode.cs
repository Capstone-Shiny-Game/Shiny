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
            Port choicePort = CreateChoicePort("New Choice");

            Choices.Add("New Choice");

            outputContainer.Add(choicePort);
        });

        addChoiceButton.AddToClassList("ds-node__button");

        mainContainer.Insert(1, addChoiceButton);

        foreach (string choice in Choices)
        {
            Port choicePort = CreateChoicePort(choice);

            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }

    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);

        DialogueType = DSDialogueType.MultipleChoice;

        Choices.Add("New Choice");
    }

    private Port CreateChoicePort(string choice)
    {
        Port choicePort = this.CreatePort();

        Button deleteChoiceButton = DSElementUtilty.CreateButton("X");

        deleteChoiceButton.AddToClassList("ds-node__button");

        TextField choiceTextField = DSElementUtilty.CreateTextField(choice);

        choiceTextField.AddClasses("ds-node__textfield", "ds-node__choice-textfield", "ds-node__textfield__hidden");

        choicePort.Add(choiceTextField);
        choicePort.Add(deleteChoiceButton);

        return choicePort;
    }
}