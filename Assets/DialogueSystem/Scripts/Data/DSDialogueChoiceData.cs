using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DSDialogueChoiceData
{
    [field: SerializeField] public string Text { get; set; }
    [field: SerializeField] public DSDialogueSO NextDialogue { get; set; }
}
