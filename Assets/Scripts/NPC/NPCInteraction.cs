using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    // separate delegate to pass NPC position for Player position relocation?

    public delegate void NPCInteract();
    public static event NPCInteract OnNPCInteractEvent;

    public GameObject npcUI;
    public GameObject npcConvoText;
    private TextMeshProUGUI tmpGUI;

    public DSDialogueContainerSO dialogueContainer;

    private DSDialogueSO currentDialogue;

    private void Start()
    {
        tmpGUI = npcConvoText.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        OnNPCInteractEvent += EnterDialogue;
        // ContinueDialogue event here*
    }
    private void OnDisable()
    {
        OnNPCInteractEvent -= EnterDialogue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (OnNPCInteractEvent != null)
            {
                OnNPCInteractEvent();
            }
        }
    }

    private void EnterDialogue()
    {
        foreach (DSDialogueSO dialogue in dialogueContainer.UngroupedDialogues)
        {
            if (dialogue.IsStartingDialogue)
            {
                npcUI.SetActive(true);
                currentDialogue = dialogue;
                tmpGUI.text = currentDialogue.Text;
                break;
            }
        }
    }

    public void ContinueDialogue(Button button)
    {
        TextMeshProUGUI tmpGUI = button.GetComponentInChildren<TextMeshProUGUI>();
        string text = tmpGUI.text;
        foreach (DSDialogueChoiceData choice in currentDialogue.Choices)
        {
            if (text.Equals(choice.Text))
            {
                currentDialogue = choice.NextDialogue;
                tmpGUI.text = currentDialogue.Text;
                break;
            }
        }
    }
}
