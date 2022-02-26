using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingQuest : MonoBehaviour
{
    public DSDialogueContainerSO StartDialogue;
    public DSDialogueContainerSO CompletionDialogue;

    public GameObject ExpectedDelivery;
    public int ExpectedQuantity;
    public GameObject InteractButton;

    private NPCInteraction dialogueSystem;

    void Start()
    {
        dialogueSystem = GetComponent<NPCInteraction>();
        dialogueSystem.dialogueContainer = StartDialogue;
        NPCInteraction.OnNPCInteractEndEvent += startQuest;
        InteractButton.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag(ExpectedDelivery.tag))
        {
            if(CheckDelivery())
            {
                //if items are there swap current dialogue
                dialogueSystem.dialogueContainer = CompletionDialogue;
                InteractButton.SetActive(true);
            }
        }
    }

    private void startQuest()
    {
        //Check if this dialogue end is what would have triggered this event
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 50)
        {
            InteractButton.SetActive(false);
            if(dialogueSystem.dialogueContainer == CompletionDialogue)
            {
                Destroy(dialogueSystem.npcUI);
                Destroy(dialogueSystem);
                Destroy(GetComponent<SphereCollider>());
                Destroy(InteractButton);
                Destroy(this);
            }
        }
    }

    private bool CheckDelivery()
    {
        List<GameObject> destroyIfDone = new List<GameObject>();

        foreach(Collider col in Physics.OverlapSphere(transform.position, 50))
        {
            if(col.gameObject.name.ToLower().Contains(ExpectedDelivery.name.ToLower()))
            {
                if(!destroyIfDone.Contains(col.gameObject))
                {
                    destroyIfDone.Add(col.gameObject);
                }
            }
        }
        if(destroyIfDone.Count == ExpectedQuantity)
        {
            foreach(GameObject obj in destroyIfDone)
            {
                Destroy(obj);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
