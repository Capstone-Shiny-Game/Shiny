using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DSSingleChoiceNode : DSNode
{
    public override void Draw()
    {
        base.Draw();

        foreach (DSChoiceSaveData choice in Choices)
        {
            Port choicePort = this.CreatePort(choice.Text);

            choicePort.userData = choice;

            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }

    public override void Initialize(DSGraphView dsGraphView, Vector2 position)
    {
        base.Initialize(dsGraphView, position);

        DialogueType = DSDialogueType.SingleChoice;

        DSChoiceSaveData choiceData = new DSChoiceSaveData()
        {
            Text = "Next Dialogue",
        };

        Choices.Add(choiceData);
    }
}
