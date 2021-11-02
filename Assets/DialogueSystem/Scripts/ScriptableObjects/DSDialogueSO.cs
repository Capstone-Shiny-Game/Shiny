using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSDialogueSO : ScriptableObject
{
    [field: SerializeField] public string DialogueName { get; set; }
    [field: SerializeField] [field: TextArea()] public string Text { get; set; }
    [field: SerializeField] public List<DSDialogueChoiceData> Choices { get; set; }
    [field: SerializeField] public DSDialogueType DSDialogueType { get; set; }
    [field: SerializeField] public bool IsStartingDialogue { get; set; }

    public void Initialize(string dialogueName, string text, List<DSDialogueChoiceData> choices, DSDialogueType dialogueType, bool isStartingDialogue)
    {
        DialogueName = dialogueName;
        Text = text;
        Choices = choices;
        DSDialogueType = dialogueType;
        IsStartingDialogue = isStartingDialogue;
    }
}
