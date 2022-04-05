using UnityEngine;
using TMPro;
using System.Linq;

public class QuestMenuContainer : MenuContainer
{
    public GameObject questMenu;
    public GameObject activeQuestMenu;
    public GameObject completedQuestMenu;
    public TextMeshProUGUI activeQuestText;
    public TextMeshProUGUI completedQuestText;
    public MenuType menuType2 = MenuType.activeQuestsMenu; //MenuType.activeQuestsMenu | MenuType.completedQuestsMenu;
    public override void AfterEnableSetup(MenuType currentMenuType)
    {
        base.AfterEnableSetup(currentMenuType);
        if (questMenu is null) {
            Debug.Log("quest menu container missing reference to its self");
            return;
        }
        switch (menuType)//Set menu options for showing either completed or uncompleted quests
        {
            case MenuType.completedQuestsMenu:
                questMenu.SetActive(true);
                activeQuestMenu.SetActive(false);
                completedQuestMenu.SetActive(true);
                completedQuestText.text = "Active\n" + string.Join("\n", QuestManager.CompletedQuests.Select(quest => $"[ ] {quest}"));
                break;
            case MenuType.activeQuestsMenu:
                questMenu.SetActive(true);
                activeQuestMenu.SetActive(true);
                completedQuestMenu.SetActive(false);
                activeQuestText.text = "Completed\n" + string.Join("\n", QuestManager.ActiveQuests.Select(quest => $"[ ] {quest}"));
                break;
            default:
                Debug.Log("Quest menu container was passed incorrect menu type");
                break;
        }
    }
}
