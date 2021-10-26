using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DSNode : Node
{
    public string DialogueName { get; set; }
    public List<string> Choices { get; set; }
    public string Text { get; set; }
    public DSDialogueType DialogueType { get; set; }

    public virtual void Initialize(Vector2 position)
    {
        DialogueName = "DialogueName";
        Choices = new List<string>();
        Text = "Dialogue text.";

        SetPosition(new Rect(position, Vector2.zero));

        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");
    }

    public virtual void Draw()
    {
        //TITLE\\
        TextField dialogueNameTextField = DSElementUtilty.CreateTextField(DialogueName);

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

}
