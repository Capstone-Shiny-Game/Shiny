using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class QuestMenuContainer : MenuContainer
{
    public GameObject questMenu;
    public GameObject activeQuestMenu;
    public GameObject completedQuestMenu;
    public TextMeshProUGUI activeQuestText;
    public TextMeshProUGUI completedQuestText;

    public MenuType subMenuType = MenuType.activeQuestsMenu;
    public override void AfterEnableSetup(MenuType currentMenuType)
    {
        base.AfterEnableSetup(currentMenuType);
        if (questMenu is null) {
            Debug.Log("quest menu container missing reference to its self");
            return;
        }
        switch (subMenuType)//Set menu options for showing either completed or uncompleted quests
        {
            case MenuType.completedQuestsMenu:
                questMenu.SetActive(true);
                activeQuestMenu.SetActive(false);
                completedQuestMenu.SetActive(true);
                completedQuestText.text = "<u>Completed:</u>\n" + string.Join("\n", QuestManager.CompletedQuests.Select(quest => $"<s>[x] {quest}</s>\n    {QuestManager.DescribeQuest(quest)}"));
                Debug.Log(completedQuestText.text);
                break;
            case MenuType.activeQuestsMenu:
                questMenu.SetActive(true);
                activeQuestMenu.SetActive(true);
                completedQuestMenu.SetActive(false);
                activeQuestText.text = "<u>Active:</u>\n" + string.Join("\n", QuestManager.ActiveQuests.Select(quest => $"[ ] {quest}\n    {QuestManager.DescribeQuest(quest)}"));
                Debug.Log(activeQuestText.text);
                break;
            default:
                Debug.Log("Quest menu container was passed incorrect menu type");
                break;
        }
    }
}
