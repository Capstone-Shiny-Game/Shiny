using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSDialogueContainerSO : ScriptableObject
{
    [field: SerializeField] public string FileName { get; set; }
    [field: SerializeField] public SerializableDictionary<DSDialogueGroupSO, List<DSDialogueSO>> DialogueGroups { get; set; }
    [field: SerializeField] public List<DSDialogueSO> UngroupedDialogues { get; set; }

    public void Initialize(string fileName)
    {
        FileName = fileName;
        DialogueGroups = new SerializableDictionary<DSDialogueGroupSO, List<DSDialogueSO>>();
        UngroupedDialogues = new List<DSDialogueSO>();
    }
}
