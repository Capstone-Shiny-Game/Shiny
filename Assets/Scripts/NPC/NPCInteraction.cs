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

    private DSDialogueSO currentDialogue;
    private GameObject avatar;
    private Image bgImage;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI bodyText;
    private GameObject[] buttonList;

    private readonly string CONTINUE = "Continue";

    private InputAction npcInteractAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/T");

    private void Start()
    {
        avatar = npcUI.transform.Find("Avatars").Find(avatarName).gameObject;
        bgImage = npcUI.transform.Find("TextBg").GetComponent<Image>();
        nameText = bgImage.gameObject.transform.Find("NameDisplay").GetComponent<TextMeshProUGUI>();
        bodyText = bgImage.gameObject.transform.Find("TextDisplay").GetComponent<TextMeshProUGUI>();
        Transform buttons = npcUI.transform.Find("Buttons");
        buttonList = new GameObject[3] {
            buttons.Find("Button3").gameObject,
            buttons.Find("Button2").gameObject,
            buttons.Find("Button1").gameObject
        };
    }

    private void OnEnable()
    {
        // OnNPCInteractEvent += EnterDialogue;
        // OnNPCInteractEvent += ApplyTheme;

        npcInteractAction.performed += ctx => TryEnterDialogue();
        npcInteractAction.Enable();
    }
    private void OnDisable()
    {
        // OnNPCInteractEvent -= EnterDialogue;
        // OnNPCInteractEvent -= ApplyTheme;

        npcInteractAction.Disable();
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         OnNPCInteractEvent?.Invoke();
    //     }
    // }

    private void TryEnterDialogue() 
    {
        if (true) // replace
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
            if (text.Equals(choice.Text) || text.Equals(CONTINUE))
            {
                if (choice.NextDialogue == null)
                {
                    npcUI.SetActive(false);
                    OnNPCInteractEndEvent();
                    break;
                }
                currentDialogue = choice.NextDialogue;
                this.bodyText.text = currentDialogue.Text;
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
