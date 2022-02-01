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
    public DSDialogueContainerSO[] dialogueContainers;
    private int currentDialogueValue = 0;
    [System.NonSerialized]
    public DSDialogueContainerSO dialogueContainer;
    public GameObject objectToSpawn;
    public GameObject otherNPC;
    public GameObject blackOutSquare;


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
        dialogueContainer = dialogueContainers[0];
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
                if (HasUnfinishedDialogue())
                {
                    break;
                }
                //FADE
                if (objectToSpawn != null)
                {
                    if (otherNPC != null)
                    {
                        otherNPC.AddComponent<Quest>();
                        otherNPC.GetComponent<Quest>().questItemsAndPositions = new SerializableDictionary<GameObject, Vector3>();
                        otherNPC.GetComponent<Quest>().dialogueContainer = dialogueContainer2;
                        otherNPC.GetComponent<Quest>().interactButton = otherNPC.GetComponentInChildren<BillboardFX>(true).gameObject;
                        otherNPC.GetComponent<Quest>().resetList(objectToSpawn, transform.position + new Vector3(2, 5, 0));
                        StartCoroutine(FadeBlackOutSquare());
                    }
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
                    if (HasUnfinishedDialogue())
                    {
                        break;
                    }

                    npcUI.SetActive(false);

                    currentDialogueValue++;
                    //TEMP: Switch Dialogue if multiple
                    if (currentDialogueValue < dialogueContainers.Length)
                    {
                        dialogueContainer = dialogueContainers[currentDialogueValue];
                    }


                    OnNPCInteractEndEvent();
                    if (quest != null)
                    {
                        quest.StartQuest();
                    }
                    break;
                }
                if (!HasUnfinishedDialogue())
                {
                    currentDialogue = choice.NextDialogue;
                    typeBodyText = StartCoroutine(TypeBodyText());
                    EnableButtons();
                }
                break;
            }
        }
    }

    private bool HasUnfinishedDialogue()
    {
        if (bodyText.maxVisibleCharacters < bodyText.text.Length)
        {
            StopCoroutine(typeBodyText);
            bodyText.maxVisibleCharacters = bodyText.text.Length;
            EnableButtons();
            return true;
        }
        return false;
    }

    private IEnumerator TypeBodyText()
    {
        bodyText.maxVisibleCharacters = 0;
        bodyText.text = currentDialogue.Text;
        for (int i = 1; i < bodyText.text.Length + 1; i++)
        {
            bodyText.maxVisibleCharacters = i;
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

    private IEnumerator FadeBlackOutSquare(int fadeSpeed = 1)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        while (blackOutSquare.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = blackOutSquare.GetComponent<Image>().color.a + (Time.deltaTime * fadeSpeed);
            blackOutSquare.GetComponent<Image>().color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return null;
        }

        otherNPC.GetComponent<Quest>().StartQuest();
        objectToSpawn = null;
        yield return new WaitForSeconds(0.5f);

        while (blackOutSquare.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = blackOutSquare.GetComponent<Image>().color.a - (Time.deltaTime * fadeSpeed);
            blackOutSquare.GetComponent<Image>().color = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            yield return null;
        }
    }
}
