using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    public delegate void NPCInteract(Transform trans);
    public static event NPCInteract OnNPCInteractEvent;
    public delegate void NPCInteractEnd();
    public static event NPCInteractEnd OnNPCInteractEndEvent;

    [Header("Character Info")]
    public string characterName = "CHANGE ME";
    public string avatarName = "CHANGE ME";
    public Color bgColor;

    [Header("Objects")]
    public GameObject npcUI;
    public DSDialogueContainerSO dialogueContainer;
    public GameObject objectToSpawn;
    public GameObject otherNPC;

    //DELETE AFTER BETA
    public DSDialogueContainerSO dialogueContainer2;

    private DSDialogueSO currentDialogue;
    private GameObject avatar;
    private Image bgImage;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI bodyText;
    private GameObject[] buttonList;

    private Coroutine typeBodyText;

    private readonly string CONTINUE = "Continue";
    private readonly string WAIT = "Wait";

    private InputAction npcInteractAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/T");

    private BoxCollider interactionCollider;
    private Quest quest;

    private void Start()
    {
        avatar = npcUI.transform.Find("Avatars").Find(avatarName).gameObject;
        bgImage = npcUI.transform.Find("TextBg").GetComponent<Image>();
        nameText = bgImage.gameObject.transform.Find("NameDisplay").GetComponent<TextMeshProUGUI>();
        bodyText = bgImage.gameObject.transform.Find("TextDisplay").GetComponent<TextMeshProUGUI>();
        Transform buttons = npcUI.transform.Find("Buttons");
        quest = GetComponent<Quest>();
        buttonList = new GameObject[3] {
            buttons.Find("Button3").gameObject,
            buttons.Find("Button2").gameObject,
            buttons.Find("Button1").gameObject
        };
        interactionCollider = GetComponentInChildren<BoxCollider>();
    }

    private void OnEnable()
    {
        npcInteractAction.performed += ctx => TryEnterDialogue();
        npcInteractAction.Enable();
    }
    private void OnDisable()
    {
        npcInteractAction.Disable();
    }

    private void TryEnterDialogue()
    {
        if (interactionCollider.bounds.Contains(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position)) // oof
        {
            OnNPCInteractEvent?.Invoke(this.transform);
            EnterDialogue();
            ApplyTheme();
        }
    }

    private void EnterDialogue()
    {
        foreach (DSDialogueSO dialogue in dialogueContainer.UngroupedDialogues)
        {
            if (dialogue.IsStartingDialogue)
            {
                currentDialogue = dialogue;
                EnableButtons();
                typeBodyText = StartCoroutine(TypeBodyText());
                npcUI.SetActive(true);
                break;
            }
        }
    }

    public void ContinueDialogue(Button button)
    {
        DisableButtons();

        TextMeshProUGUI btnTmpGUI = button.GetComponentInChildren<TextMeshProUGUI>();
        string text = btnTmpGUI.text;

        if (typeBodyText != null)
        {
            StopCoroutine(typeBodyText);
        }

        foreach (DSDialogueChoiceData choice in currentDialogue.Choices)
        {
            if (choice.Text == WAIT)
            {
                //FADE
                if (objectToSpawn != null)
                {
                    //GameObject spawnedObject = ItemWorld.SpawnItemWorld(objectToSpawn, transform.position + new Vector3(2, 5, 0)).item.prefab;

                    if (otherNPC != null)
                    {
                        otherNPC.AddComponent<Quest>();
                        otherNPC.GetComponent<Quest>().questItemsAndPositions = new SerializableDictionary<GameObject, Vector3>();
                        otherNPC.GetComponent<Quest>().dialogueContainer = dialogueContainer2;
                        otherNPC.GetComponent<Quest>().interactButton = otherNPC.GetComponentInChildren<BillboardFX>(true).gameObject;
                        otherNPC.GetComponent<Quest>().resetList(objectToSpawn, transform.position + new Vector3(2, 5, 0));
                        otherNPC.GetComponent<Quest>().StartQuest();
                    }
                    objectToSpawn = null;
                    gameObject.GetComponentInChildren<BillboardFX>().gameObject.SetActive(false);
                }
                currentDialogue = choice.NextDialogue;
                bodyText.text = currentDialogue.Text;
                EnableButtons();
                break;
            }
            else if (text.Equals(choice.Text) || text.Equals(CONTINUE))
            {
                if (choice.NextDialogue == null)
                {
                    npcUI.SetActive(false);
                    OnNPCInteractEndEvent();
                    if (quest != null)
                    {
                        quest.StartQuest();
                    }
                    break;
                }
                currentDialogue = choice.NextDialogue;
                typeBodyText = StartCoroutine(TypeBodyText());
                EnableButtons();
                break;
            }
        }
    }

    // TODO (Jakob) : handle words that break a line
    private IEnumerator TypeBodyText()
    {
        string dialogue = currentDialogue.Text;
        bodyText.text = string.Empty;
        foreach (char c in dialogue)
        {
            bodyText.text += c;
            yield return new WaitForSeconds(0.035f);
        }
    }

    private void DisableButtons()
    {
        foreach (GameObject button in buttonList)
        {
            button.SetActive(false);
        }
    }

    private void EnableButtons()
    {
        if (currentDialogue.DSDialogueType == DSDialogueType.SingleChoice)
        {
            buttonList[0].SetActive(true);
            buttonList[0].GetComponentInChildren<TextMeshProUGUI>().text = CONTINUE;
        }
        else
        {
            for (int i = 0; i < currentDialogue.Choices.Count; i++)
            {
                buttonList[i].SetActive(true);
                buttonList[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.Choices[i].Text;
            }
        }
    }

    private void ApplyTheme()
    {
        avatar.SetActive(true);
        nameText.text = characterName;
        bgImage.color = bgColor;
    }
}
