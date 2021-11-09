using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    // separate delegate to pass NPC position for Player position relocation?

    public delegate void NPCInteract();
    public static event NPCInteract OnNPCInteractEvent;

    public GameObject npcUI;
    public DSDialogueContainerSO dialogueContainer;

    private DSDialogueSO currentDialogue;

    private void OnEnable()
    {
        OnNPCInteractEvent += EnterDialogue;
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
                currentDialogue = dialogue;
                StartCoroutine(ContinueDialogue());
                break;
            }
        }
    }

    private IEnumerator ContinueDialogue()
    {
        npcUI.SetActive(true);

        while (currentDialogue != null)
        {
            //Debug.Log(currentDialogue.Text);

            yield return new WaitForFixedUpdate();
        }


    }
}
