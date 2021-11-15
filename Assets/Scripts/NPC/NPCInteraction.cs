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
    public delegate void NPCInteractEnd();
    public static event NPCInteractEnd OnNPCInteractEndEvent;

    [Header("Character Info")]
    public string characterName = "CHANGE ME";
    public string avatarName = "CHANGE ME";
    public Color bgColor;

    [Header("Objects")]
    public GameObject npcUI;
    public DSDialogueContainerSO dialogueContainer;

    private DSDialogueSO currentDialogue;
    private GameObject avatar;
    private Image BGImage;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI bodyText;
    private GameObject[] buttonList;


    private readonly string OK = "Continue";

    private void Start()
    {
        avatar = npcUI.transform.Find("Avatars").Find(avatarName).gameObject;
        BGImage = npcUI.transform.Find("TextBg").GetComponent<Image>();
        nameText = BGImage.gameObject.transform.Find("NameDisplay").GetComponent<TextMeshProUGUI>();
        bodyText = BGImage.gameObject.transform.Find("TextDisplay").GetComponent<TextMeshProUGUI>();
        Transform buttons = npcUI.transform.Find("Buttons");
        buttonList = new GameObject[3] {
            buttons.Find("Button3").gameObject,
            buttons.Find("Button2").gameObject,
            buttons.Find("Button1").gameObject
        };
    }

    private void OnEnable()
    {
        OnNPCInteractEvent += EnterDialogue;
        OnNPCInteractEvent += ApplyTheme;
        // ContinueDialogue event here*
    }
    private void OnDisable()
    {
        OnNPCInteractEvent -= EnterDialogue;
        OnNPCInteractEvent -= ApplyTheme;
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
                EnableButtons();
                bodyText.text = currentDialogue.Text;
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

        foreach (DSDialogueChoiceData choice in currentDialogue.Choices)
        {
            if (text.Equals(choice.Text) || text.Equals(OK))
            {
                if (choice.NextDialogue == null)
                {
                    npcUI.SetActive(false);
                    OnNPCInteractEndEvent();
                    break;
                }
                currentDialogue = choice.NextDialogue;
                this.bodyText.text = currentDialogue.Text; // breaks with MultipleChoice node
                EnableButtons();
                break;
            }
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
        if (currentDialogue.Choices.Count == 1)
        {
            buttonList[0].SetActive(true);
            buttonList[0].GetComponentInChildren<TextMeshProUGUI>().text = OK;
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
        BGImage.color = bgColor;
    }
}
