using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionRequestQuest : MonoBehaviour
{
    public string QuestName;

    public DSDialogueContainerSO StartDialogue;
    public DSDialogueContainerSO P2Dialogue;
    public DSDialogueContainerSO CompletionDialogue;

    public GameObject Deliver1;
    public GameObject Deliver2;
    public GameObject Deliver3;
    public int ExpectedQuantity;
    public GameObject InteractButton;

    public delegate void Complete();
    public static event Complete OnQuestCompleteEvent;

    private NPCInteraction dialogueSystem;

    void Start()
    {
        dialogueSystem = GetComponent<NPCInteraction>();
        dialogueSystem.dialogueContainer = StartDialogue;
        NPCInteraction.OnNPCInteractEndEvent += startQuest;
        DayController.OnEveningEvent += timeSensitive;
        InteractButton.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag(Deliver1.tag) || other.gameObject.CompareTag(Deliver2.tag) || other.gameObject.CompareTag(Deliver3.tag))
        {
            if(CheckDelivery())
            {
                //if items are there swap current dialogue
                dialogueSystem.dialogueContainer = P2Dialogue;
                InteractButton.SetActive(true);
            }
        }
    }

    private void startQuest(string dialogue)
    {
        QuestManager.StartQuest(QuestName, dialogueSystem.characterName, new string[] {Deliver1.name, Deliver2.name, Deliver3.name});
        QuestManager.RecordDialogue(QuestName, dialogueSystem.characterName, dialogue);
        //Check if this dialogue end is what would have triggered this event
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 50)
        {
            InteractButton.SetActive(false);
            if(dialogueSystem.dialogueContainer == CompletionDialogue)
            {
                QuestManager.CompleteQuest(QuestName);
                OnQuestCompleteEvent?.Invoke();
                Destroy(dialogueSystem.npcUI);
                Destroy(dialogueSystem);
                Destroy(GetComponent<SphereCollider>());
                Destroy(InteractButton);
                Destroy(this);
            }
        }
    }

    private void timeSensitive()
    {
        if(dialogueSystem.dialogueContainer == P2Dialogue && InteractButton.activeInHierarchy == false)
        {
            dialogueSystem.dialogueContainer = CompletionDialogue;
            InteractButton.SetActive(true);
        }
    }

    private bool CheckDelivery()
    {
        List<GameObject> destroyIfDone = new List<GameObject>();

        foreach(Collider col in Physics.OverlapSphere(transform.position, 50))
        {
            if(col.gameObject.name.ToLower().Contains(Deliver1.name.ToLower()) || col.gameObject.name.ToLower().Contains(Deliver2.name.ToLower()) || col.gameObject.name.ToLower().Contains(Deliver3.name.ToLower()))
            {
                QuestManager.StrikeItem(QuestName, col.gameObject.name);
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
