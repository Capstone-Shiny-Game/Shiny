using UnityEngine;
using TMPro;
using System.Linq;

public class QuestMenuContainer : MenuContainer
{
    public GameObject questMenu;
    public GameObject activeQuestMenu;
    public GameObject completedQuestMenu;
    public TextMeshPro activeQuestText;
    public TextMeshPro completedQuestText;
    public MenuType menuType2 = MenuType.uncompletedQuestsMenu;
    public override void AfterEnableSetup(MenuType currentMenuType)
    {
        base.AfterEnableSetup(currentMenuType);
        if (questMenu is null) {
            Debug.Log("quest menu container missing reference to its self");
            return;
        }
        switch (menuType)//Set menu options for showing either completed or uncompleted quests
        {
            case MenuType.questsMenu:
                Debug.Log("showing completed quests");
                questMenu.SetActive(true);
                activeQuestMenu.SetActive(false);
                activeQuestText.text = string.Join("\n", QuestManager.ActiveQuests.Select(quest => $"[ ] {quest}"));
                completedQuestMenu.SetActive(true);
                break;
            case MenuType.uncompletedQuestsMenu:
                Debug.Log("showing uncompleted quests");
                questMenu.SetActive(true);
                activeQuestMenu.SetActive(true);
                completedQuestMenu.SetActive(false);
                completedQuestText.text = string.Join("\n", QuestManager.CompletedQuests.Select(quest => $"[ ] {quest}"));
                break;
            default:
                Debug.Log("Quest menu container was passed incorrect menu type");
                break;
        }
    }
}
