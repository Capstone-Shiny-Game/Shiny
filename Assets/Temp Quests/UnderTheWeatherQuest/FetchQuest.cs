using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FetchQuest : MonoBehaviour
{
    public simpleQuest[] quests;

    public GameObject InteractButton;
    public bool randomQuest;
    public FetchQuest randomQuests;
    public delegate void Complete();
    public static event Complete OnQuestCompleteEvent;
    private DSDialogueContainerSO CompletionDialogue;
    private DSDialogueContainerSO[] Dialogues;
    private GameObject[] ExpectedDeliveries;
    private int ExpectedQuantity;
    private NPCInteraction dialogueSystem;
    private int currentQuest = -1;
    private int currentDialogue = 0;
    private bool questComplete = true;

    void Start()
    {
        dialogueSystem = GetComponent<NPCInteraction>();
        NPCInteraction.OnNPCInteractEndEvent += startQuest;
        swapQuest();
        InteractButton.SetActive(true);
        DayController.OnDayStartEvent += dailyQuest;
    }

    void OnDestroy()
    {
        NPCInteraction.OnNPCInteractEndEvent -= startQuest;
        DayController.OnDayStartEvent -= dailyQuest;
    }

    void OnTriggerEnter(Collider other)
    {
        if(InteractButton.activeInHierarchy || !this.enabled)
        {
            return;
        }

        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag(ExpectedDeliveries[0].tag))
        {
            if(CheckDelivery())
            {
                //if items are there swap current dialogue
                dialogueSystem.dialogueContainer = Dialogues[currentDialogue + 1];
                currentDialogue++;
                InteractButton.SetActive(true);
            }
        }
    }

    private void dailyQuest()
    {
        if(randomQuest)
            swapQuest();
    }

    private void swapQuest()
    {
        if(questComplete == false)
        {
            return;
        }

        questComplete = false;

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
        ExpectedDeliveries = quests[index].ExpectedDeliveries;
        ExpectedQuantity = quests[index].ExpectedTotalQuantity;
        Dialogues = quests[index].Dialogues;

        //TODO: Implement multidialogue
        dialogueSystem.dialogueContainer = Dialogues[0];
        CompletionDialogue = Dialogues[Dialogues.Length - 1];
        currentDialogue = 0;

        InteractButton.SetActive(true);
    }

    private void startQuest()
    {
        //Check if this dialogue end is what would have triggered this event
        if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 50)
        {
            InteractButton.SetActive(false);
            QuestManager.StartQuest(quests[currentQuest].name, dialogueSystem.characterName, quests[currentQuest].ExpectedDeliveries.Select(x => x.name));
            if(dialogueSystem.dialogueContainer == CompletionDialogue)
            {
                QuestManager.CompleteQuest(quests[currentQuest].name);
                OnQuestCompleteEvent?.Invoke();
                questComplete = true;

                if(!randomQuest && currentQuest == quests.Length-1)
                {

                    if(randomQuests != null)
                    {
                        randomQuests.enabled = true;
                        Destroy(this);
                    }
                    else
                    {
                        Destroy(dialogueSystem.npcUI);
                        Destroy(dialogueSystem);
                        Destroy(GetComponent<SphereCollider>());
                        Destroy(InteractButton);
                        Destroy(this);
                    }                     
                    return;
                }
                if(!randomQuest)
                    swapQuest();
            }
        }
    }

    private bool CheckDelivery()
    {
        List<GameObject> destroyIfDone = new List<GameObject>();

        foreach(Collider col in Physics.OverlapSphere(transform.position, 50))
        {
            foreach(GameObject obj in ExpectedDeliveries)
            {
                if(col.gameObject.name.ToLower().Contains(obj.name.ToLower()))
                {
                    if(!destroyIfDone.Contains(col.gameObject))
                    {
                        destroyIfDone.Add(col.gameObject);
                    }
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
        public DSDialogueContainerSO[] Dialogues;
        public GameObject[] ExpectedDeliveries;
        public int ExpectedTotalQuantity = 1;
    }
}
