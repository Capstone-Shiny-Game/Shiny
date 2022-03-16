using System.Collections.Generic;
using UnityEngine;

public class FetchQuest : MonoBehaviour
{
    public simpleQuest[] quests;

    public GameObject InteractButton;
    public bool randomQuest;

    private DSDialogueContainerSO StartDialogue;
    private DSDialogueContainerSO CompletionDialogue;
    private GameObject ExpectedDelivery;
    private int ExpectedQuantity;
    private NPCInteraction dialogueSystem;
    private int currentQuest = -1;

    void Start()
    {
        dialogueSystem = GetComponent<NPCInteraction>();
        NPCInteraction.OnNPCInteractEndEvent += startQuest;
        swapQuest();
        InteractButton.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if(InteractButton.activeInHierarchy)
        {
            return;
        }

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

    private void swapQuest()
    {
        if(randomQuest)
        {
            updateQuest(Random.Range(0, quests.Length-1));
        }
        else
        {
            updateQuest(currentQuest + 1);
            currentQuest++;
        }
    }

    private void updateQuest(int index)
    {
        StartDialogue = quests[index].StartDialogue;
        CompletionDialogue = quests[index].CompletionDialogue;
        ExpectedDelivery = quests[index].ExpectedDelivery;
        ExpectedQuantity = quests[index].ExpectedQuantity;
        dialogueSystem.dialogueContainer = StartDialogue;
        InteractButton.SetActive(true);
    }

    private void startQuest()
    {
        //Check if this dialogue end is what would have triggered this event
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 50)
        {
            InteractButton.SetActive(false);
            if(dialogueSystem.dialogueContainer == CompletionDialogue)
            {
                if(currentQuest == quests.Length-1)
                {
                    Destroy(dialogueSystem.npcUI);
                    Destroy(dialogueSystem);
                    Destroy(GetComponent<SphereCollider>());
                    Destroy(InteractButton);
                    Destroy(this);
                    return;
                }

                swapQuest();
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

    [System.Serializable]
    public class simpleQuest
    {
        public string name;

        public DSDialogueContainerSO StartDialogue;
        public DSDialogueContainerSO CompletionDialogue;

        public GameObject ExpectedDelivery;
        public int ExpectedQuantity = 1;
    }
}
